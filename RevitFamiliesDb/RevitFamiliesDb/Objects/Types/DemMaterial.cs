using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using RevitFamiliesDb.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RevitFamiliesDb
{
    public class DemMaterial : DemElement
    {
        public DemAppearanceAssetElement AppearanceAssetId { get; set; }
        public DemColor Color { get; set; }
        public DemColor CutBackgroundPatternColor { get; set; }
        public int CutBackgroundPatternId { get; set; }
        public DemColor CutForegroundPatternColor { get;set; }
        public int CutForegroundPatternId { get;set; }
        public string MaterialCategory { get; set; }
        public string MaterialClass { get; set; }
        public int Shininess { get; set; }
        public int Smoothness { get; set; }
        public int StructuralAssetId { get; set; }
        public DemStructuralAsset DemStructuralAsset { get; set; }
        public DemColor SurfaceBackgroundPatternColor { get; set; }
        public int SurfaceBackgroundPatternID { get; set; }
        public DemColor SurfaceForegroundPatternColor { get; set; }
        public int SurfaceForegroundPatternID { get; set; }
        public int ThermalAssetId { get; set; }
        public int Transparency { get; set; }
        public bool UseRenderAppearanceForShading { get; set; }

        public DemMaterial() { }

        public DemMaterial(Material material) : base(material)
        {
            var assetTest = new FilteredElementCollector(material.Document).OfClass(typeof(AppearanceAssetElement)).First(i => i.Id == material.Id) as AppearanceAssetElement;

            AppearanceAssetId = new DemAppearanceAssetElement(assetTest);
            Color = new DemColor(material.Color);
            CutBackgroundPatternColor = new DemColor(material.CutBackgroundPatternColor);
            CutBackgroundPatternId = material.CutBackgroundPatternId.IntegerValue;
            CutForegroundPatternColor = new DemColor(material.CutForegroundPatternColor);
            CutForegroundPatternId = material.CutForegroundPatternId.IntegerValue;
            MaterialCategory = material.MaterialCategory;
            MaterialClass = material.MaterialClass;
            Shininess = material.Shininess;
            Smoothness = material.Smoothness;
            StructuralAssetId = material.StructuralAssetId.IntegerValue;
            DemStructuralAsset = new DemStructuralAsset(material);
            SurfaceBackgroundPatternColor = new DemColor(material.SurfaceBackgroundPatternColor);
            SurfaceBackgroundPatternID = material.SurfaceBackgroundPatternId.IntegerValue;
            SurfaceForegroundPatternColor = new DemColor(material.SurfaceForegroundPatternColor);
            SurfaceForegroundPatternID = material.SurfaceForegroundPatternId.IntegerValue;
            ThermalAssetId = material.ThermalAssetId.IntegerValue;
            Transparency = material.Transparency;
            UseRenderAppearanceForShading = material.UseRenderAppearanceForShading;
        }

        public ElementId CreateThisMF(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Material));
            string temporaryName = Name;
            int counter = 1;
            foreach (Element elem in collector)
            {
                Material existingMaterial = elem as Material;
                if (existingMaterial != null && existingMaterial.Name == temporaryName)
                {
                    temporaryName = $"{Name}_{counter}";
                    counter++;
                }
            }


            Trace.Write("Creating Material Yush!!");
            ElementId eleId = Material.Create(doc, temporaryName);
            Trace.Write("Creating Material1");
            Material thisFucker = doc.GetElement(eleId) as Material;
            Trace.Write("Creating Material2");

            thisFucker.AppearanceAssetId = AppearanceAssetId.CreateThisMF(doc);
            thisFucker.Color = Color.ConvertToRevitColor();
            thisFucker.CutBackgroundPatternColor = CutBackgroundPatternColor.ConvertToRevitColor();
            thisFucker.CutBackgroundPatternId = new ElementId(CutBackgroundPatternId);
            thisFucker.CutForegroundPatternColor = CutForegroundPatternColor.ConvertToRevitColor();
            thisFucker.CutForegroundPatternId = new ElementId(CutForegroundPatternId);
            thisFucker.MaterialCategory = MaterialCategory;
            thisFucker.MaterialClass = MaterialClass;
            thisFucker.Shininess = Shininess;
            thisFucker.Smoothness = Smoothness;
            thisFucker.StructuralAssetId = new ElementId(StructuralAssetId);
            thisFucker.SurfaceBackgroundPatternColor = SurfaceBackgroundPatternColor.ConvertToRevitColor();
            thisFucker.SurfaceBackgroundPatternId = new ElementId(SurfaceBackgroundPatternID);
            thisFucker.SurfaceForegroundPatternColor = SurfaceForegroundPatternColor.ConvertToRevitColor();
            thisFucker.SurfaceForegroundPatternId = new ElementId(SurfaceForegroundPatternID);
            thisFucker.ThermalAssetId = new ElementId(ThermalAssetId);
            thisFucker.Transparency = Transparency;
            thisFucker.UseRenderAppearanceForShading = UseRenderAppearanceForShading;
            DemStructuralAsset.CreateThisMF(thisFucker);
            Trace.Write("Creating Material3");
            return eleId;
        }

    }
}
