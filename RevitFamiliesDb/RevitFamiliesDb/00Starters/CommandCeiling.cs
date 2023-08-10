#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Newtonsoft.Json;
using RevitFamiliesDb.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

#endregion

namespace RevitFamiliesDb
{
    [Transaction(TransactionMode.Manual)]
    public class CommandCeiling : IExternalCommand
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

            ElementId elId = sel.GetElementIds().FirstOrDefault();

            if (elId == null) return Result.Succeeded;

            // Retrieve elements from database
            var element = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .FirstOrDefault(x => x.Id == elId) as CeilingType;

            string path = Global.TheCeilingPath;


            DemCeilingType floor = new DemCeilingType(element);


            List<DemCeilingType> demObjects = new List<DemCeilingType>();

            try
            {
                demObjects = JsonConvert.DeserializeObject<List<DemCeilingType>>(File.ReadAllText(path));

            }
            catch
            {
                var dialog = new TaskDialog("Debug")
                {
                    MainContent = "Something -   "
                };
                dialog.Show();
            }

            demObjects.Add(floor);

            File.WriteAllText(path, JsonConvert.SerializeObject(demObjects));






            return Result.Succeeded;
        }
    }

    

}
