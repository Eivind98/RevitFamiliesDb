using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RevitFamiliesDb.Objects
{
    public class DemAppearanceAssetElement : DemElement
    {

        public List<DemParentSchema> TheOneAndOnlyAsset { get; set; } = new List<DemParentSchema>();

        public DemAppearanceAssetElement()
        {


        }

        public DemAppearanceAssetElement(Material material) : base(material)
        {
            var assetTest = new FilteredElementCollector(material.Document).OfClass(typeof(AppearanceAssetElement)).First(i => i.Id == material.AppearanceAssetId) as AppearanceAssetElement;
            var renderingAsset = assetTest.GetRenderingAsset();

            for (int idx = 0; idx < renderingAsset.Size; idx++)
            {
                AssetProperty ap = renderingAsset[idx];
                TheOneAndOnlyAsset.Add(new DemParentSchema(ap));
                
            }

        }

        public override ElementId CreateThisMF(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(AppearanceAssetElement));
            AppearanceAssetElement test3 = (collector.FirstOrDefault() as AppearanceAssetElement).Duplicate("RandoName");



            return test3.Id;
        }



        void ChangeRenderingTexturePath(Document doc)
        {
            // As there is only one material in the sample 
            // project, we can use FilteredElementCollector 
            // and grab the first result

            Material mat = new FilteredElementCollector(doc)
              .OfClass(typeof(Material))
              .FirstElement() as Material;

            // Fixed path for new texture
            // Texture included in sample files

            string texturePath = Path.Combine(
              Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
              "new_texture.png");

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Changing material texture path");

                using (AppearanceAssetEditScope editScope
                  = new AppearanceAssetEditScope(doc))
                {
                    Asset editableAsset = editScope.Start(
                      mat.AppearanceAssetId);

                    // Getting the correct AssetProperty
                    AssetProperty assetProperty = editableAsset.FindByName("generic_diffuse");

                    Asset connectedAsset = assetProperty
                      .GetConnectedProperty(0) as Asset;

                    // Getting the right connected Asset

                    if (connectedAsset.Name == "UnifiedBitmapSchema")
                    {
                        AssetPropertyString path
                          = connectedAsset.FindByName(
                            UnifiedBitmap.UnifiedbitmapBitmap)
                              as AssetPropertyString;

                        if (path.IsValidValue(texturePath))
                            path.Value = texturePath;
                    }
                    editScope.Commit(true);
                }
                TaskDialog.Show("Material texture path",
                    "Material texture path changed to:\n" + texturePath);

                t.Commit();
                t.Dispose();
            }
        }

    }




}
