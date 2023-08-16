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
using System.Windows.Media.Imaging;

namespace RevitFamiliesDb.Objects
{
    public class DemAssetProperties
    {
        public string FamilyName { get; set; }
        public string ImagePath { get; set; }
        public Asset test { get; set; }


        public DemAssetProperties()
        {


        }

        public DemAssetProperties(Asset property)
        {
            
        }



    }
}
