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
            Trace.Write("1");
            List<DemElement> demSelectedElements = Helper.CreateFromSelection(uidoc);
            Trace.Write("2");
            List<DemElement> demExistingElements = Helper.LoadDemElementsFromFile();
            Trace.Write("3");
            demExistingElements.AddRange(demSelectedElements);
            Trace.Write("4");
            Helper.SaveDemElementsToFile(demExistingElements);
            Trace.Write("5");

            return Result.Succeeded;
        }
    }

    

}
