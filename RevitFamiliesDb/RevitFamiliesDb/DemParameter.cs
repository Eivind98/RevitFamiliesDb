using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitFamiliesDb
{
    public class DemParameter
    {
        public string Name { get; set; }
        public bool HasValue { get; set; }
        public int Id { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsShared { get; set; }
        public int StorageType { get; set; }
        public bool UserModifiable { get; set; }
        public double AsDouble { get; set; }
        public int AsElementId { get; set; }
        public int AsInteger { get; set; }
        public string AsString { get; set; }
        public string AsValueString { get; set; }
        public bool CanBeAssociatedWithGlobalParameters { get; set; }
        public string ClearValue { get; set; }
        public int GetAssociatedGlobalParameter { get; set; }


        public DemParameter()
        {

        }

        public DemParameter(Parameter parameter)
        {


            Name = parameter.Definition.Name;
            HasValue = parameter.HasValue;
            Id = parameter.Id.IntegerValue;
            IsReadOnly = parameter.IsReadOnly;
            IsShared = parameter.IsShared;
            StorageType = (int)parameter.StorageType;
            UserModifiable = parameter.UserModifiable;

            switch (StorageType)
            {
                case 0:

                    break;
                case 1:
                    AsInteger = parameter.AsInteger();
                    AsValueString = AsInteger.ToString();
                    break;
                case 2:
                    AsDouble = parameter.AsDouble();
                    AsValueString = AsDouble.ToString();
                    break;
                case 3:
                    AsString = parameter.AsString();
                    AsValueString = AsString;
                    break;
                case 4:
                    AsElementId = parameter.AsElementId().IntegerValue;
                    AsValueString = AsElementId.ToString();
                    break;
            }

            CanBeAssociatedWithGlobalParameters = parameter.CanBeAssociatedWithGlobalParameters();
            GetAssociatedGlobalParameter = parameter.GetAssociatedGlobalParameter().IntegerValue;

        }


        public Parameter CreateThoseMF(Element ele)
        {
            Parameter param = ele.LookupParameter(Name);

            switch (StorageType)
            {
                case 0:

                    break;
                case 1:
                    param.Set(AsInteger);
                    break;
                case 2:
                    param.Set(AsDouble);
                    break;
                case 3:
                    param.Set(AsString);
                    break;
                case 4:
                    param.Set(AsElementId);
                    break;

            }

            return null;
        }


    }
}
