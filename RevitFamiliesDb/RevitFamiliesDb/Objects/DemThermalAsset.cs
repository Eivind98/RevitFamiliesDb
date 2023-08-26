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
    public class DemThermalAsset
    {
        public int Behavior { get; set; }
        public double Compressibility { get; set; }
        public double Density { get; set; }
        public double ElecricalResistivity { get; set; }
        public double Emissivity { get; set; }
        public double GasViscosity { get; set; }
        public bool IsValidObject { get; set; }
        public double LiquidViscosity { get; set; }
        public string Name { get; set; }
        public double Permeability { get; set; }
        public double Porosity { get; set; }
        public double Reflectivity { get; set; }
        public double SpecificHeat { get; set; }
        public double SpecificHeatOfVaporization { get; set; }
        public double ThermalConductivity { get; set; }
        public int ThermalMaterialType { get; set; }
        public bool TransmitsLight { get; set; }
        public double VaporPressure { get; set; }


        public DemThermalAsset()
        {


        }

        public DemThermalAsset(Material mat)
        {
            var ThermAssetId = mat.ThermalAssetId;
            if (ThermAssetId != ElementId.InvalidElementId)
            {
                PropertySetElement pse = mat.Document.GetElement(ThermAssetId) as PropertySetElement;
                if (pse != null)
                {
                    ThermalAsset asset = pse.GetThermalAsset();

                    Behavior = (int)asset.Behavior;
                    Compressibility = asset.Compressibility;
                    Density = asset.Density;
                    ElecricalResistivity = asset.ElectricalResistivity;
                    Emissivity = asset.Emissivity;
                    GasViscosity = asset.GasViscosity;
                    IsValidObject = asset.IsValidObject;
                    LiquidViscosity = asset.LiquidViscosity;
                    Name = asset.Name;
                    Permeability = asset.Permeability;
                    Porosity = asset.Porosity;
                    Reflectivity = asset.Reflectivity;
                    SpecificHeat = asset.SpecificHeat;
                    SpecificHeatOfVaporization = asset.SpecificHeatOfVaporization;
                    ThermalConductivity = asset.ThermalConductivity;
                    ThermalMaterialType = (int)asset.ThermalMaterialType;
                    TransmitsLight = asset.TransmitsLight;
                    VaporPressure = asset.VaporPressure;
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

            var ThermAsset = new ThermalAsset(temporaryName, (Autodesk.Revit.DB.ThermalMaterialType)ThermalMaterialType);

            switch (ThermalMaterialType)
            {
                case 0:

                    break;
                case 1:
                    ThermAsset.GasViscosity = GasViscosity;
                    ThermAsset.LiquidViscosity = LiquidViscosity;
                    break;
                case 2:

                    break;
                case 3:

                    break;
            }

            ThermAsset.Behavior = (StructuralBehavior)Behavior;
            ThermAsset.Compressibility = Compressibility;
            ThermAsset.Density = Density;
            ThermAsset.ElectricalResistivity = ElecricalResistivity;
            ThermAsset.Emissivity = Emissivity;

            

            ThermAsset.Permeability = Permeability;
            ThermAsset.Porosity = Porosity;
            ThermAsset.Reflectivity = Reflectivity;
            ThermAsset.SpecificHeat = SpecificHeat;
            ThermAsset.SpecificHeatOfVaporization = SpecificHeatOfVaporization;
            ThermAsset.ThermalConductivity = ThermalConductivity;
            ThermAsset.TransmitsLight = TransmitsLight;
            ThermAsset.VaporPressure = VaporPressure;

            PropertySetElement pse = PropertySetElement.Create(doc, ThermAsset);
            mat.SetMaterialAspectByPropertySet(MaterialAspect.Structural, pse.Id);

        }

    }
}
