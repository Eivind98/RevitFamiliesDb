using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using RevitFamiliesDb.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitFamiliesDb
{
    public class DemMaterial : DemElement
    {
        public int AppearanceAssetId { get; set; }
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
        public DemColor SurfaceBackgroundPatternColor { get; set; }
        public int SurfaceBackgroundPatternID { get; set; }
        public DemColor SurfaceForegroundPatternColor { get; set; }
        public int SurfaceForegroundPatternID { get; set; }
        public int ThermalAssetId { get; set; }
        public int Transparency { get; set; }
        public bool UseRenderAppearanceForShading { get; set; }

        public DemMaterial(Material material) : base(material)
        {
            AppearanceAssetId = material.AppearanceAssetId.IntegerValue;
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
            SurfaceBackgroundPatternColor = new DemColor(material.SurfaceBackgroundPatternColor);
            SurfaceBackgroundPatternID = material.SurfaceBackgroundPatternId.IntegerValue;
            SurfaceForegroundPatternColor = new DemColor(material.SurfaceForegroundPatternColor);
            SurfaceForegroundPatternID = material.SurfaceForegroundPatternId.IntegerValue;
            ThermalAssetId = material.ThermalAssetId.IntegerValue;
            Transparency = material.Transparency;
            UseRenderAppearanceForShading = material.UseRenderAppearanceForShading;
        }

        public void CreateThisMF(Document doc)
        {
            Material thisFucker = doc.GetElement(Material.Create(doc, this.Name)) as Material;

            thisFucker.Name = Name;
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

        }

    }
}
