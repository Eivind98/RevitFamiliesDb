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
        public int AppearanceAssetId { get; set; }
        public DemAppearanceAssetElement DemAppearanceAsset { get; set; }
        public DemColor Color { get; set; }
        public DemColor CutBackgroundPatternColor { get; set; }
        public int CutBackgroundPatternId { get; set; }
        public DemColor CutForegroundPatternColor { get; set; }
        public int CutForegroundPatternId { get; set; }
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
        public DemThermalAsset DemThermalAsset { get; set; }
        public int Transparency { get; set; }
        public bool UseRenderAppearanceForShading { get; set; }

        public DemMaterial() { }

        public DemMaterial(Material material) : base(material)
        {
            AppearanceAssetId = material.AppearanceAssetId.IntegerValue;
            DemAppearanceAsset = new DemAppearanceAssetElement(material);
            Color = new DemColor(material.Color) ?? null;
            CutBackgroundPatternColor = new DemColor(material.CutBackgroundPatternColor) ?? null;
            CutBackgroundPatternId = material.CutBackgroundPatternId.IntegerValue;
            CutForegroundPatternColor = new DemColor(material.CutForegroundPatternColor) ?? null;
            CutForegroundPatternId = material.CutForegroundPatternId.IntegerValue;
            MaterialCategory = material.MaterialCategory;
            MaterialClass = material.MaterialClass;
            Shininess = material.Shininess;
            Smoothness = material.Smoothness;
            StructuralAssetId = material.StructuralAssetId.IntegerValue;
            DemStructuralAsset = material.StructuralAssetId.IntegerValue == -1 ? null : new DemStructuralAsset(material);
            SurfaceBackgroundPatternColor = new DemColor(material.SurfaceBackgroundPatternColor) ?? null;
            SurfaceBackgroundPatternID = material.SurfaceBackgroundPatternId.IntegerValue;
            SurfaceForegroundPatternColor = new DemColor(material.SurfaceForegroundPatternColor) ?? null;
            SurfaceForegroundPatternID = material.SurfaceForegroundPatternId.IntegerValue;
            ThermalAssetId = material.ThermalAssetId.IntegerValue;
            DemThermalAsset = material.ThermalAssetId.IntegerValue == -1 ? null : new DemThermalAsset(material);
            Transparency = material.Transparency;
            UseRenderAppearanceForShading = material.UseRenderAppearanceForShading;
        }

        public override ElementId CreateThisMF(Document doc)
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

            ElementId eleId = Material.Create(doc, temporaryName);
            Material thisFucker = doc.GetElement(eleId) as Material;
            //thisFucker.AppearanceAssetId = AppearanceAssetId.CreateThisMF(doc) ?? null;
            thisFucker.Color = Color.ConvertToRevitColor() ?? null;
            thisFucker.CutBackgroundPatternColor = CutBackgroundPatternColor.ConvertToRevitColor() ?? null;
            thisFucker.CutBackgroundPatternId = new ElementId(CutBackgroundPatternId) ?? null;
            thisFucker.CutForegroundPatternColor = CutForegroundPatternColor.ConvertToRevitColor() ?? null;
            thisFucker.CutForegroundPatternId = new ElementId(CutForegroundPatternId) ?? null;
            thisFucker.MaterialCategory = MaterialCategory;
            thisFucker.MaterialClass = MaterialClass;
            thisFucker.Shininess = Shininess;
            thisFucker.Smoothness = Smoothness;
            thisFucker.StructuralAssetId = new ElementId(StructuralAssetId) ?? null;
            thisFucker.SurfaceBackgroundPatternColor = SurfaceBackgroundPatternColor.ConvertToRevitColor() ?? null;
            thisFucker.SurfaceBackgroundPatternId = new ElementId(SurfaceBackgroundPatternID) ?? null;
            thisFucker.SurfaceForegroundPatternColor = SurfaceForegroundPatternColor.ConvertToRevitColor() ?? null;
            thisFucker.SurfaceForegroundPatternId = new ElementId(SurfaceForegroundPatternID) ?? null;
            thisFucker.ThermalAssetId = new ElementId(ThermalAssetId) ?? null;
            thisFucker.Transparency = Transparency;
            thisFucker.UseRenderAppearanceForShading = UseRenderAppearanceForShading;

            if (DemStructuralAsset != null)
            {
                DemStructuralAsset.CreateThisMF(thisFucker);
            }
            else
            {
                thisFucker.StructuralAssetId = ElementId.InvalidElementId;
            }

            if(DemThermalAsset != null)
            {
                DemThermalAsset.CreateThisMF(thisFucker);
            }
            else
            {
                thisFucker.ThermalAssetId = ElementId.InvalidElementId;
            }

            foreach (DemParameter para in this.DemParameter)
            {
                para.CreateThoseMF(thisFucker);

            }

            return eleId;
        }

    }
}
