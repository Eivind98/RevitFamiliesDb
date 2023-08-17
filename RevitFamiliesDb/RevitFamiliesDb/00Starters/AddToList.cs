#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

#endregion

namespace RevitFamiliesDb
{
    [Transaction(TransactionMode.Manual)]
    public class AddToList : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var app = uiapp.Application;
            var doc = uidoc.Document;

            var sel = uidoc.Selection.GetElementIds();

            //WallType wallType = new FilteredElementCollector(doc).OfClass(typeof(WallType)).First(i => i.Id == sel.First()) as WallType;
            //Trace.Write("1");
            //var test = wallType.GetCompoundStructure().GetLayers()[0].MaterialId;
            //Trace.Write("2");
            //var Yo = new FilteredElementCollector(doc).OfClass(typeof(Material)).First(i => i.Id == test) as Material;
            //Trace.Write("3");
            //var nah = new FilteredElementCollector(doc).OfClass(typeof(AppearanceAssetElement)).First(i => i.Id == Yo.AppearanceAssetId) as AppearanceAssetElement;
            //Trace.Write("4");
            //var nam = (new FilteredElementCollector(doc).OfClass(typeof(AppearanceAssetElement)).First() as AppearanceAssetElement).GetRenderingAsset();

            //Trace.Write("5");

            //using (var tx = new Transaction(doc))
            //{
            //    tx.Start("Douche bag");

            //    nah.SetRenderingAsset(nam);

            //    tx.Commit();
            //}

            using (var transaction = new Transaction(doc))
            {
                transaction.Start("CreateMaterial");

                // Create new material
                var newMaterial = Material.Create(doc, "AAJustCreated1");
                var material = doc.GetElement(newMaterial) as Material;


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

                var pse = PropertySetElement.Create(doc, thAsset);

                material.SetMaterialAspectByPropertySet(MaterialAspect.Thermal, pse.Id);

                var strAsset = new StructuralAsset("Thermal1", StructuralAssetClass.Generic);

                strAsset.Name = "yourname";
                strAsset.Behavior = StructuralBehavior.Isotropic;
                strAsset.YoungModulus = new XYZ(0, 0, 0);
                strAsset.Density = 0.0;

                var pses = PropertySetElement.Create(doc, strAsset);

                material.SetMaterialAspectByPropertySet(MaterialAspect.Structural, pses.Id);

                // Add attach to class
                transaction.Commit();
            }




            return Result.Succeeded;
        }



    }



}
