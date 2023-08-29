#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
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

            Application los = uiapp.Application;

            IList<Asset> unfilteredList = los.GetAssets((AssetType)4);

            Material mat = new FilteredElementCollector(doc).OfClass(typeof(Material)).First(i => i.Id == new ElementId(423)) as Material;
            AppearanceAssetElement appearanceElem = mat.Document.GetElement(mat.AppearanceAssetId) as AppearanceAssetElement;
            var collector = new FilteredElementCollector(doc).OfClass(typeof(AppearanceAssetElement));

            //List<AppearanceAssetElement> ls = new List<AppearanceAssetElement>();

            //foreach (AppearanceAssetElement ele in collector)
            //{
            //    ls.Add(ele);

            //}

            Trace.Write(unfilteredList.Count);

            Asset renderAss = appearanceElem.GetRenderingAsset();

            int thatAss = renderAss.Size;

            List<string> thoseAssets = new List<string>();

            for (int idx = 0; idx < thatAss; idx++)
            {
                AssetProperty ap = renderAss[idx];

                thoseAssets.Add(ap.Name);
                Trace.WriteLine(ap.Name);
            }




            IList<Asset> filteredList = unfilteredList.Where(i => i.Size == thatAss).ToList();

            Asset theOne = null;
            foreach(var asset in filteredList)
            {
                bool boho = true;


                foreach(var po in thoseAssets)
                {
                    try
                    {
                        asset.FindByName(po);
                    }
                    catch
                    {
                        boho = false;
                        break;
                    }
                }
                
                if(boho)
                {
                    theOne = asset;
                    break;
                }
            }

            if (theOne != null)
            {
                thoseAssets.Sort();
                Trace.WriteLine("---------------------------------");
                Trace.WriteLine(thoseAssets.Count);
                foreach (var po in thoseAssets)
                {
                    Trace.WriteLine(po);
                }
                Trace.WriteLine("---------------------------------");
                Trace.WriteLine(theOne.Size);
                for (int idx = 0; idx < theOne.Size; idx++)
                {
                    AssetProperty ap = theOne[idx];
                    Trace.WriteLine(ap.Name);
                }
            }


            //foreach (Asset ele in ls)
            //{
            //    Trace.Write($"{ele.Size} - {ele.Name}");
            //}

            

            //using (Transaction t = new Transaction(doc))
            //{
            //    t.Start("Changing material texture path");

            //    var ka = unfilteredList.First(i => i.Name == "ACADGen-010");

            //    Trace.WriteLine(ka.Name);

            //    appearanceElem.SetRenderingAsset(ka);

            //    t.Commit();
            //    t.Dispose();
            //}

            Trace.WriteLine(appearanceElem.GetRenderingAsset().Name);


            //using (Transaction t = new Transaction(doc))
            //{
            //    t.Start("Changing material texture path");

            //    using (AppearanceAssetEditScope editScope = new AppearanceAssetEditScope(doc))
            //    {
            //        Material mat = new FilteredElementCollector(doc).OfClass(typeof(Material)).First(i => i.Id == new ElementId(404414)) as Material;
            //        //string texturePath = "Another Folder\\Absolutely.jpg";

            //        ElementId appearanceId = mat.AppearanceAssetId;
            //        AppearanceAssetElement appearanceElem = mat.Document.GetElement(appearanceId) as AppearanceAssetElement;
            //        Asset theAsset = appearanceElem.GetRenderingAsset();

            //        Asset editableAsset = editScope.Start(appearanceId);

            //        //AssetPropertyDoubleArray4d mg = new AssetPropertyDoubleArray4d("", );

            //        Trace.Write(editableAsset.IsValidSchemaIdentifier("Metal"));


            //        var la = editableAsset.FindByName("generic_diffuse");


            //        editScope.Commit(true);
            //    }

            //    t.Commit();
            //    t.Dispose();
            //}





            //using (Transaction t = new Transaction(doc))
            //{
            //    t.Start("Changing material texture path");

            //    using (AppearanceAssetEditScope editScope = new AppearanceAssetEditScope(doc))
            //    {

            //        Asset editableAsset = editScope.Start(mat.AppearanceAssetId);


            //        List<AssetProperty> someJoke = new List<AssetProperty>();
            //        for (int idx = 0; idx < editableAsset.Size; idx++)
            //        {
            //            AssetProperty ap = editableAsset[idx];

            //            someJoke.Add(ap);
            //        }

            //        foreach(AssetProperty ap in someJoke)
            //        {
            //            Trace.Write(ap.Name);
            //        }

            //        if (editableAsset.IsEditable())
            //        {
            //            Trace.Write("Is Edible");
            //        }
            //        else
            //        {
            //            Trace.Write("Is Not Edible");
            //        }

            //        //editableAsset.AddConnectedAsset("UnifiedBitmapSchema");
            //        AssetProperty theAssetWeWant = null;
            //        for (int idx = 0; idx < editableAsset.Size; idx++)
            //        {
            //            AssetProperty ap = editableAsset[idx];

            //            if(ap.Name == "generic_diffuse")
            //            {
            //                theAssetWeWant = ap;
            //            }

            //            someJoke.Add(ap);
            //        }
            //        if(theAssetWeWant != null)
            //        {
            //            Trace.Write(theAssetWeWant.Name);
            //            Trace.Write(theAssetWeWant.Type);
            //            Trace.Write(theAssetWeWant.IsReadOnly);
            //            Trace.Write(theAssetWeWant.IsValidObject);
            //            Trace.Write(theAssetWeWant.IsEditable());
            //            Trace.Write(theAssetWeWant.GetAllConnectedProperties().Count);

            //            var bro = theAssetWeWant.GetConnectedProperty(0);

            //            Trace.Write(bro.Name);
            //            Trace.Write(bro.Type);
            //            Trace.Write(bro.IsReadOnly);
            //            Trace.Write(bro.IsValidObject);
            //            Trace.Write(bro.IsEditable());
            //            Trace.Write(bro.GetAllConnectedProperties().Count);


            //        }


            //        var yo = editableAsset.FindByName("UnifiedBitmapSchema");



            //        editScope.Commit(true);
            //    }

            //    t.Commit();
            //    t.Dispose();
            //}



            //int anin = assets.IndexOf(tas);

            //theAsset.GetConnectedProperty(anin);

            //Trace.Write(tes);


            //dem = dem.OrderBy(ap => ap.Name).ToList();
            //Trace.WriteLine(dem.Count);
            //foreach (var asset in dem)
            //{
            //    Trace.Write($" ({asset.ValueType.Name}) - {asset.Name}: {asset.GetValue}");
            //}


            // order the properties!
            //assets = assets.OrderBy(ap => ap.Name).ToList();
            //for (int idx = 0; idx < assets.Count; idx++)
            //{
            //    AssetProperty ap = assets[idx];
            //    Type type = ap.GetType();
            //    object apVal = null;
            //    try
            //    {
            //        // using .net reflection to get the value
            //        var prop = type.GetProperty("Value");
            //        if (prop != null && prop.GetIndexParameters().Length == 0)
            //        {
            //            apVal = prop.GetValue(ap);
            //        }
            //        else
            //        {
            //            apVal = "<No Value Property>";
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        apVal = ex.GetType().Name + "-" + ex.Message;
            //    }

            //    if (apVal is DoubleArray)
            //    {
            //        var doubles = apVal as DoubleArray;
            //        apVal = doubles.Cast<double>().Aggregate("", (s, d) => s + Math.Round(d, 5) + ",");
            //    }
            //    Trace.Write(idx + " : [" + ap.Type + "] " + ap.Name + " = " + apVal);
            //}



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

        private void AddTexturePath(AssetProperty asset, string texturePath)
        {
            Asset connectedAsset = null;

            if (asset.NumberOfConnectedProperties == 0)
            {
                asset.AddConnectedAsset("UnifiedBitmapSchema");
            }

            connectedAsset = (Asset)asset.GetConnectedProperty(0);
            AssetPropertyString path = (AssetPropertyString)connectedAsset.FindByName(UnifiedBitmap.UnifiedbitmapBitmap);

            //if (!path.IsValidValue(texturePath))
            //{
            //    File.Create("texture.png");
            //    texturePath = Path.GetFullPath("texture.png");
            //}

            path.Value = texturePath;

        }

    }
}
