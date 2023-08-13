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
    public class CommandSaveMaterial : IExternalCommand
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

            Trace.Write("1");

            List<DemElement> demSelectedElements = Helper.CreateFromSelection(uidoc);

            List<DemMaterial> mat = new List<DemMaterial>();

            mat.Add(Helper.GetDemMaterialFromAndForElement(demSelectedElements[0], doc)[0]);

            Trace.Write("2");
            
            Trace.Write("4");

            Trace.Write("3");
            File.WriteAllText(Global.TheMaterialPath, JsonConvert.SerializeObject(mat));

            Trace.Write("5");

            return Result.Succeeded;
        }
    }

    

}
