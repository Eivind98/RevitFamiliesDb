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
using System.Windows.Media.Imaging;

namespace RevitFamiliesDb.Objects
{
    public class DemAssetProperty
    {
        public Type ValueType { get; set; }
        public string Name { get; set; }
        public List<DemAssetProperty> DemConnectedProperties { get; set; }
        public object TheValue { get; set; } = null;
        public string GetValue
        {
            get
            {
                switch (TheValue)
                {
                    case string str when TheValue is string:
                        return str;
                    case bool bo when TheValue is bool:
                        return bo.ToString();
                    case int inte when TheValue is int:
                        return inte.ToString();
                    case double doub when TheValue is double:
                        return doub.ToString();
                    case DoubleArray doubleArray when TheValue is DoubleArray:
                        return doubleArray.Cast<double>().Aggregate("", (s, d) => s + Math.Round(d, 5) + ",");
                    default:
                        return "<No Value>";
                }
            }
        }



        public DemAssetProperty()
        {

        }

        public DemAssetProperty(Asset ap)
        {
            Type type = ap.GetType();
            ValueType = type;
            Name = ap.Name;
            

            var prop = type.GetProperty("Value");
            if (prop != null && prop.GetIndexParameters().Length == 0)
            {
                TheValue = prop.GetValue(ap);
            }
            else
            {
                TheValue = null;
            }
        }

        public Asset CreateThisMF(Asset assetElement)
        {
            Asset outPut = assetElement;



            return outPut;
        }

    }
}
