using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
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

namespace RevitFamiliesDb.Objects
{
    public class DemStructuralAsset
    {
        public int Behavior { get; set; }
        public double ConcreteBendingReinforcement { get; set; }
        public double ConcreteCompression { get; set; }
        public double ConcreteShearReinforcement { get; set; }
        public double ConcreteShearStrengthReduction { get; set; }
        public double Density { get; set; }
        public bool IsValidObject { get; set; }
        public bool Lightweight { get; set; }
        public double MetalReductionFactor { get; set; }
        public double MetalResistanceCalculationStrength { get; set; }
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
                    ConcreteShearStrengthReduction = asset.ConcreteShearStrengthReduction;
                    Density = asset.Density;
                    IsValidObject = asset.IsValidObject;
                    Lightweight = asset.Lightweight;
                    MetalReductionFactor = asset.MetalReductionFactor;
                    MetalResistanceCalculationStrength = asset.MetalResistanceCalculationStrength;
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

            var doc = mat.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            var existingPropertySets = collector.OfClass(typeof(PropertySetElement)).ToElements();

            string temporaryName = Name + " - " + Guid.NewGuid().ToString();
            //int counter = 1;
            //foreach (Element elem in existingPropertySets)
            //{
            //    PropertySetElement existingPropertySet = elem as PropertySetElement;
            //    if (existingPropertySet != null && existingPropertySet.Name == temporaryName)
            //    {
            //        temporaryName = $"{Name}_{counter}";
            //        counter++;
            //    }
            //}

            var strAsset = new StructuralAsset(temporaryName, (StructuralAssetClass)StructuralAssetClass);
            Trace.Write("Creating Material8");
            strAsset.Behavior = (StructuralBehavior)Behavior;
            strAsset.Density = Density;
            strAsset.Lightweight = Lightweight;
            switch (StructuralAssetClass)
            {
                case 2:
                    strAsset.MinimumTensileStrength = MinimumTensileStrength;
                    strAsset.MinimumYieldStress = MinimumYieldStress;
                    break;
                case 3:
                    strAsset.MetalReductionFactor = MetalReductionFactor;
                    strAsset.MetalResistanceCalculationStrength = MetalResistanceCalculationStrength;
                    strAsset.MetalThermallyTreated = MetalThermallyTreated;
                    goto case 2; // Reuse the code for case 2 properties
                case 4:
                    strAsset.ConcreteBendingReinforcement = ConcreteBendingReinforcement;
                    strAsset.ConcreteCompression = ConcreteCompression;
                    strAsset.ConcreteShearReinforcement = ConcreteShearReinforcement;
                    strAsset.ConcreteShearStrengthReduction = ConcreteShearStrengthReduction;
                    break;
                case 5:
                    strAsset.WoodBendingStrength = WoodBendingStrength;
                    strAsset.WoodGrade = WoodGrade;
                    strAsset.WoodParallelCompressionStrength = WoodParallelCompressionStrength;
                    strAsset.WoodParallelShearStrength = WoodParallelShearStrength;
                    strAsset.WoodPerpendicularCompressionStrength = WoodPerpendicularCompressionStrength;
                    strAsset.WoodPerpendicularShearStrength = WoodPerpendicularShearStrength;
                    strAsset.WoodSpecies = WoodSpecies;
                    break;
            }

            if (Behavior == 1 || Behavior == 2)
            {
                strAsset.PoissonRatio = PoissonRatio.CreateThisMF();
                strAsset.ShearModulus = ShearModulus.CreateThisMF();
                strAsset.ThermalExpansionCoefficient = ThermalExpansionCoefficient.CreateThisMF();
                strAsset.YoungModulus = YoungModulus.CreateThisMF();
            }
            else if (Behavior == 0)
            {
                strAsset.SetPoissonRatio(PoissonRatio.X);
                strAsset.SetShearModulus(ShearModulus.X);
                strAsset.SetThermalExpansionCoefficient(ThermalExpansionCoefficient.X);
                strAsset.SetYoungModulus(YoungModulus.X);
            }

            strAsset.SubClass = SubClass;

            Trace.Write("Creating Material9");
            if (mat.Document.IsModifiable)
            {
                PropertySetElement pse = PropertySetElement.Create(doc, strAsset);
                mat.SetMaterialAspectByPropertySet(MaterialAspect.Structural, pse.Id);
            }
            else
            {
                Trace.WriteLine("Document is not valid or modifiable.");
            }


        }

    }
}
