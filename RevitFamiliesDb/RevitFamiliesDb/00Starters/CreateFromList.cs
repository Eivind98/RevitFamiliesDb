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

            //List<FamilyTypeObject> demObjects = JsonConvert.DeserializeObject<List<FamilyTypeObject>>(File.ReadAllText("C:\\Users\\eev_9\\OneDrive\\02 - Projects\\Programming stuff\\Test.json"));

            //using (var tx = new Transaction(doc))
            //{
            //    tx.Start("Douche bag");

            //    demObjects[0].CreateElement(doc);

            //    tx.Commit();
            //}


            //var sel = uidoc.Selection.GetElementIds();

            //WallType wallType = new FilteredElementCollector(doc).OfClass(typeof(WallType)).First(i => i.Id == sel.First()) as WallType;

            //var yo = wallType.GetCompoundStructure().GetLayers().First().MaterialId;

            Material mat = new FilteredElementCollector(doc).OfClass(typeof(Material)).First(i => i.Id == new ElementId(423)) as Material;
            string texturePath = "";


            ElementId appearanceId = mat.AppearanceAssetId;
            AppearanceAssetElement appearanceElem = mat.Document.GetElement(appearanceId) as AppearanceAssetElement;
            Asset theAsset = appearanceElem.GetRenderingAsset();
            List<AssetProperty> assets = new List<AssetProperty>();
            for (int idx = 0; idx < theAsset.Size; idx++)
            {
                AssetProperty ap = theAsset[idx];
                assets.Add(ap);
            }
            // order the properties!
            assets = assets.OrderBy(ap => ap.Name).ToList();
            for (int idx = 0; idx < assets.Count; idx++)
            {
                AssetProperty ap = assets[idx];
                Type type = ap.GetType();
                object apVal = null;
                try
                {
                    // using .net reflection to get the value
                    var prop = type.GetProperty("Value");
                    if (prop != null && prop.GetIndexParameters().Length == 0)
                    {
                        apVal = prop.GetValue(ap);
                    }
                    else
                    {
                        apVal = "<No Value Property>";
                    }
                }
                catch (Exception ex)
                {
                    apVal = ex.GetType().Name + "-" + ex.Message;
                }

                if (apVal is DoubleArray)
                {
                    var doubles = apVal as DoubleArray;
                    apVal = doubles.Cast<double>().Aggregate("", (s, d) => s + Math.Round(d, 5) + ",");
                }
                Trace.Write(idx + " : [" + ap.Type + "] " + ap.Name + " = " + apVal);
            }



            //using (Transaction t = new Transaction(doc))
            //{
            //    t.Start("Changing material texture path");

            //    using (AppearanceAssetEditScope editScope = new AppearanceAssetEditScope(doc))
            //    {
            //        //Asset jo = Asset;

            //        //var jo = AppearanceAssetElement.Create(doc, "", asus)


            //        Asset editableAsset = editScope.Start(mat.AppearanceAssetId);

            //        // Getting the correct AssetProperty
            //        AssetProperty assetProperty = editableAsset.FindByName("generic_diffuse");

            //        Trace.Write(editableAsset.Size);




            //        for (int i = 0; i < editableAsset.Size; i++)
            //        {
            //            try
            //            {
            //                Trace.Write(editableAsset[i].Type);
            //                Trace.Write(editableAsset[i].Name);
            //                Trace.Write(editableAsset[i].IsReadOnly);
            //                Trace.Write(editableAsset[i].IsValidObject);
            //                Trace.Write(editableAsset[i].NumberOfConnectedProperties);
                            
            //            }
            //            catch(Exception e)
            //            {
            //                Trace.Write(e.Message + " +?" + "- - - What is wroong");
            //            }

            //            if (editableAsset[i].Name == "")
            //            {

            //            }

            //            //var tust = new Asset(doc, "dfg");

            //        }

            //        //doing some testing
            //        //var test = editableAsset.GetAllConnectedProperties();
            //        //foreach (var property in test)
            //        //{
            //        //    Trace.Write(property.Name);
            //        //    Trace.Write(property.NumberOfConnectedProperties);



            //        //}



            //        // Getting the right connected Asset

            //        //if (connectedAsset.Name == "UnifiedBitmapSchema")
            //        //{
            //        //    AssetPropertyString path = connectedAsset.FindByName(UnifiedBitmap.UnifiedbitmapBitmap) as AssetPropertyString;

            //        //    if (path.IsValidValue(texturePath))
            //        //    {
            //        //        path.Value = texturePath;
            //        //    }


            //        //}
            //        editScope.Commit(true);
            //    }
                
            //    t.Commit();
            //    t.Dispose();
            //}




            return Result.Succeeded;
        }
    }
}
