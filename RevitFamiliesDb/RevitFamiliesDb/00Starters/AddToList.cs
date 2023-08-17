#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
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

            var sel = uidoc.Selection.GetElementIds();

            WallType wallType = new FilteredElementCollector(doc).OfClass(typeof(WallType)).First(i => i.Id == sel.First()) as WallType;
            Trace.Write("1");
            var test = wallType.GetCompoundStructure().GetLayers()[0].MaterialId;
            Trace.Write("2");
            var Yo = new FilteredElementCollector(doc).OfClass(typeof(Material)).First(i => i.Id == test) as Material;
            Trace.Write("3");
            var nah = new FilteredElementCollector(doc).OfClass(typeof(AppearanceAssetElement)).First(i => i.Id == Yo.AppearanceAssetId) as AppearanceAssetElement;
            Trace.Write("4");
            var nam = (new FilteredElementCollector(doc).OfClass(typeof(AppearanceAssetElement)).First() as AppearanceAssetElement).GetRenderingAsset();

            Trace.Write("5");

            using (var tx = new Transaction(doc))
            {
                tx.Start("Douche bag");

                nah.SetRenderingAsset(nam);

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
