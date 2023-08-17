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
    public class DemXYZ
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }


        public DemXYZ()
        {


        }

        public DemXYZ(XYZ Coord)
        {
            X = Coord.X;
            Y = Coord.Y;
            Z = Coord.Z;


        }

        public Asset CreateThisMF(Asset assetElement)
        {
            Asset outPut = assetElement;



            return outPut;
        }

    }
}
