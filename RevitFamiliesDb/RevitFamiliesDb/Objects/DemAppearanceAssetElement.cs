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
using System.Windows.Media.Imaging;

namespace RevitFamiliesDb.Objects
{
    public class DemAppearanceAssetElement : DemElement
    {
        
        public DemAsset TheOneAndOnlyAsset { get; set; }
        public Asset TestAsset { get; set; }
        public AppearanceAssetElement test2 { get; set; }


        public DemAppearanceAssetElement()
        {


        }

        public DemAppearanceAssetElement(AppearanceAssetElement assetElement) : base(assetElement)
        {


            //Trace.Write("12");
            //test2 = assetElement.Duplicate("TestName");
            //Trace.Write("13");
            //TestAsset = assetElement.GetRenderingAsset();
            //Trace.Write("14");

            //test2 = AppearanceAssetElement.Create(element.Document, "someName", new Asset());



        }

        public ElementId CreateThisMF(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(AppearanceAssetElement));

            AppearanceAssetElement test3 = (collector.FirstOrDefault() as AppearanceAssetElement).Duplicate("RandoName");

            test3.SetRenderingAsset(TestAsset);

            //test3.SetRenderingAsset(TheOneAndOnlyAsset.CreateThisMF(test3.GetRenderingAsset()));



            return test3.Id;
        }


    }
}
