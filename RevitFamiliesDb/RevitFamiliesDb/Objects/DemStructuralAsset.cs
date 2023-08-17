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
            


            var strAsset = new StructuralAsset(Name, (StructuralAssetClass)StructuralAssetClass);

            strAsset.Behavior = (StructuralBehavior)Behavior;
            strAsset.Name = Name;
            strAsset.ConcreteBendingReinforcement = ConcreteBendingReinforcement;
            strAsset.ConcreteCompression = ConcreteCompression;
            strAsset.ConcreteShearReinforcement = ConcreteShearReinforcement;
            strAsset.Density = Density;
            strAsset.Lightweight = Lightweight;
            strAsset.MetalReductionFactor = MetalReductionFactor;
            strAsset.MetalResistanceCalculationStrength = MetalResistanceCalculationStrenth;
            strAsset.MetalThermallyTreated = MetalThermallyTreated;
            strAsset.MinimumTensileStrength = MinimumTensileStrength;
            strAsset.MinimumYieldStress = MinimumYieldStress;
            strAsset.PoissonRatio = PoissonRatio.CreateThisMF();
            strAsset.ShearModulus = ShearModulus.CreateThisMF();
            strAsset.SubClass = SubClass;
            strAsset.ThermalExpansionCoefficient = ThermalExpansionCoefficient.CreateThisMF();
            strAsset.WoodBendingStrength = WoodBendingStrength;
            strAsset.WoodGrade = WoodGrade;
            strAsset.WoodParallelCompressionStrength = WoodParallelCompressionStrength;
            strAsset.WoodParallelShearStrength = WoodParallelShearStrength;
            strAsset.WoodPerpendicularCompressionStrength = WoodPerpendicularCompressionStrength;
            strAsset.WoodPerpendicularShearStrength = WoodPerpendicularShearStrength;
            strAsset.WoodSpecies = WoodSpecies;
            strAsset.YoungModulus = YoungModulus.CreateThisMF();

            var pse = PropertySetElement.Create(mat.Document, strAsset);

            mat.SetMaterialAspectByPropertySet(MaterialAspect.Structural, pse.Id);

        }

    }
}
