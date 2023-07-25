#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

#endregion

namespace RevitFamiliesDb
{
    [Transaction(TransactionMode.Manual)]
    public class AddToList : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var app = uiapp.Application;
            var doc = uidoc.Document;

            // Access current selection

            var sel = uidoc.Selection;

            var elId = sel.GetElementIds().FirstOrDefault();

            if (elId == null) return Result.Succeeded;

            var element = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .FirstOrDefault(x => x.Id == elId) as FloorType;

            List<FamilyTypeObject> demObjects = new List<FamilyTypeObject>();

            try
            {
                demObjects = JsonConvert.DeserializeObject<List<FamilyTypeObject>>(File.ReadAllText("C:\\Users\\eev_9\\OneDrive\\02 - Projects\\Programming stuff\\Test.json"));

            }
            catch
            {

            }


            using (var tx = new Transaction(doc))
            {
                tx.Start("Douche bag");

                demObjects.Add(new FamilyTypeObject(elId, doc));
                
                tx.Commit();
            }

            File.WriteAllText("C:\\Users\\eev_9\\OneDrive\\02 - Projects\\Programming stuff\\Test.json", FamilyTypeObject.PrintTypeObject(demObjects));

            return Result.Succeeded;
        }
    }
}
