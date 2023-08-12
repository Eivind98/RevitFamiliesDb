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
    public class DemColor : DemElement
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }


        public DemColor(Color color)
        {
            Red = color.Red;
            Green = color.Green;
            Blue = color.Blue;
        }

        public Color ConvertToRevitColor()
        {
            return new Color(Red, Green, Blue);
        }

    }
}
