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
    public class CreateFromList : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;

            List<FamilyTypeObject> demObjects = JsonConvert.DeserializeObject<List<FamilyTypeObject>>(File.ReadAllText("C:\\Users\\eev_9\\OneDrive\\02 - Projects\\Programming stuff\\Test.json"));

            using (var tx = new Transaction(doc))
            {
                tx.Start("Douche bag");

                demObjects[0].CreateElement(doc);

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
