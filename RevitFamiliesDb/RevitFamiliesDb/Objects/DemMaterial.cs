using Autodesk.Revit.DB;
using RevitFamiliesDb.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RevitFamiliesDb
{
    public class DemMaterial : DemElement
    {
        public int AppearanceAssetId { get; set; }
        public string Color { get; set; }
        public string CutBackgroundPatternColor { get; set; }
        public int CutBackgroundPatternId { get; set; }
        public string CutForegroundPatternColor { get;set; }
        public int CutForegroundPatternId { get;set; }
        public string MaterialCategory { get; set; }
        public string MaterialClass { get; set; }
        public string Shininess { get; set; }
        public string Smoothness { get; set; }
        public string StructuralAssetId { get; set; }
        public string SurfaceBackgroundPatternColor { get; set; }
        public string SurfaceBackgroundPatternID { get; set; }
        public string SurfaceForegroundPatternColor { get; set; }
        public string SurfaceForegroundPatternID { get; set; }
        public string ThermalAssetId { get; set; }
        public string Transparency { get; set; }
        public string UseRenderAppearanceForShading { get; set; }

        public DemMaterial(Autodesk.Revit.DB.Material material) : base(material as Element)
        {






        }

    }
}
