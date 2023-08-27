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
    public class DemAsset
    {
        public string ValueType { get; set; }
        public string Name { get; set; }
        private string _value { get; set; } = null;
        public object TheValue
        {
            get => _value;
            set
            {
                switch (value)
                {
                    case string str when value is string:
                        ValueAsString = str;
                        _value = str;
                        break;
                    case bool bo when value is bool:

                        break;
                    case int inte when value is int:

                        break;
                    case double doub when value is double:

                        break;
                    case float fl when value is float:
                        Trace.Write("Is this even used??");
                        break;
                    case DoubleArray doubleArray when value is DoubleArray:


                        break;
                    default:

                        break;

                }
            }
        }
        
        public string ValueAsString { get; set; }
        public bool ValueAsBool { get; set; }
        public double ValueAsDouble { get; set; }
        public Array ValueAsArray { get; set; }



        public DemAsset()
        {

        }

        public DemAsset(Asset ap)
        {
            Type type = ap.GetType();
            ValueType = type.Name;

            //Type tup = Type.GetType(ValueType);

            try
            {
                // using .net reflection to get the value
                var prop = type.GetProperty("Value");
                if (prop != null && prop.GetIndexParameters().Length == 0)
                {
                    TheValue = prop.GetValue(ap);
                }
                else
                {
                    TheValue = "<No Value Property>";
                }
            }
            catch (Exception ex)
            {
                TheValue = ex.GetType().Name + "-" + ex.Message;
            }

            if (TheValue is DoubleArray)
            {
                var doubles = TheValue as DoubleArray;
                TheValue = doubles.Cast<double>().Aggregate("", (s, d) => s + Math.Round(d, 5) + ",");
            }

            switch (TheValue)
            {
                case string str when TheValue is string:

                    break;
                case bool bo when TheValue is bool:

                    break;
                case int inte when TheValue is int:

                    break;
                case double doub when TheValue is double:

                    break;
                case float fl when TheValue is float:
                    Trace.Write("Is this even used??");
                    break;
                case DoubleArray doubleArray when TheValue is DoubleArray:


                    break;
                default:

                    break;

            }


        }

        public Asset CreateThisMF(Asset assetElement)
        {
            Asset outPut = assetElement;



            return outPut;
        }

    }
}
