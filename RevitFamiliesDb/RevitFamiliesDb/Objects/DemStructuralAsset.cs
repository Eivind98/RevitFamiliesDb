using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RevitFamiliesDb.Objects
{
    public class DemStructuralAsset
    {
        public int Behavior { get; set; }
        public double ConcreteBendingReinforcement { get; set; }
        public double ConcreteCompression { get; set; }
        public double ConcreteShearReinforcement { get; set; }
        public double Density { get; set; }
        public bool IsValidObject { get; set; }
        public bool Lightweight { get; set; }
        public double MetalReductionFactor { get; set; }
        public double MetalResistanceCalculationStrenth { get; set; }
        public bool MetalThermallyTreated { get; set; }
        public double MinimumTensileStrength { get; set; }
        public double MinimumYieldStress { get; set; }
        public string Name { get; set; }
        public DemXYZ PoissonRatio { get; set; }
        public DemXYZ ShearModulus { get; set; }
        public int StructuralAssetClass { get; set; }
        public string SubClass { get; set; }
        public DemXYZ ThermalExpansionCoefficient { get; set; }
        public double WoodBendingStrength { get; set; }
        public string WoodGrade { get; set; }
        public double WoodParallelCompressionStrength { get; set; }
        public double WoodParallelShearStrength { get; set; }
        public double WoodPerpendicularCompressionStrength { get; set; }
        public double WoodPerpendicularShearStrength { get; set; }
        public string WoodSpecies { get; set; }
        public DemXYZ YoungModulus { get; set; }


        public DemStructuralAsset()
        {


        }

        public DemStructuralAsset(Material mat)
        {
            var strucAssetId = mat.StructuralAssetId;
            if (strucAssetId != ElementId.InvalidElementId)
            {
                PropertySetElement pse = mat.Document.GetElement(strucAssetId) as PropertySetElement;
                if (pse != null)
                {
                    StructuralAsset asset = pse.GetStructuralAsset();

                    Behavior = (int)asset.Behavior;
                    ConcreteBendingReinforcement = asset.ConcreteBendingReinforcement;
                    ConcreteCompression = asset.ConcreteCompression;
                    ConcreteShearReinforcement = asset.ConcreteShearReinforcement;
                    Density = asset.Density;
                    IsValidObject = asset.IsValidObject;
                    Lightweight = asset.Lightweight;
                    MetalReductionFactor = asset.MetalReductionFactor;
                    MetalResistanceCalculationStrenth = asset.MetalResistanceCalculationStrength;
                    MetalThermallyTreated = asset.MetalThermallyTreated;
                    MinimumTensileStrength = asset.MinimumTensileStrength;
                    MinimumYieldStress = asset.MinimumYieldStress;
                    Name = asset.Name;
                    PoissonRatio = new DemXYZ(asset.PoissonRatio);
                    ShearModulus = new DemXYZ(asset.ShearModulus);
                    StructuralAssetClass = (int)asset.StructuralAssetClass;
                    SubClass = asset.SubClass;
                    ThermalExpansionCoefficient = new DemXYZ(asset.ThermalExpansionCoefficient);
                    WoodBendingStrength = asset.WoodBendingStrength;
                    WoodGrade = asset.WoodGrade;
                    WoodParallelCompressionStrength = asset.WoodParallelCompressionStrength;
                    WoodParallelShearStrength = asset.WoodParallelShearStrength;
                    WoodPerpendicularCompressionStrength = asset.WoodPerpendicularCompressionStrength;
                    WoodPerpendicularShearStrength = asset.WoodPerpendicularShearStrength;
                    WoodSpecies = asset.WoodSpecies;
                    YoungModulus = new DemXYZ(asset.YoungModulus);


                }
            }


        }

        public void CreateThisMF(Material mat)
        {
            


            var thAsset = new ThermalAsset("Thermal1", ThermalMaterialType.Solid);
            thAsset.Name = "SomeThermalAsset";
            thAsset.Behavior = StructuralBehavior.Isotropic;
            thAsset.ThermalConductivity = 0.00;
            thAsset.SpecificHeatOfVaporization = 0.0;
            thAsset.Porosity = 0.0;
            thAsset.Density = 0.0;
            thAsset.Emissivity = 0.0;
            thAsset.Reflectivity = 0.0;
            thAsset.Permeability = 0.0;
            thAsset.ElectricalResistivity = 0.0;
            thAsset.Compressibility = 0.0;
            var pse = PropertySetElement.Create(mat.Document, thAsset);

            mat.SetMaterialAspectByPropertySet(MaterialAspect.Thermal, pse.Id);

        }

    }
}
