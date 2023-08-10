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
    public class CommandFloor : IExternalCommand
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
                .FirstOrDefault(x => x.Id == elId) as FloorType;

            string path = Global.TheFloorPath;

            DemFloorType floor = new DemFloorType(element);

            List<DemFloorType> demObjects = new List<DemFloorType>();

            try
            {
                demObjects = JsonConvert.DeserializeObject<List<DemFloorType>>(File.ReadAllText(path));

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
