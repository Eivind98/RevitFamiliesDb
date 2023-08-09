using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RevitFamiliesDb.Objects
{
    public class DemElement
    {
        public int AssemblyInstanceId {  get; set; }
        public string Category { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<DemParameter> DemParameter { get; set; }
        public string UniqueId { get; set; }


        public DemElement() { }

        public DemElement(Element element)
        {
            AssemblyInstanceId = element.AssemblyInstanceId.IntegerValue;
            Category = element.Category.Name;
            Id = element.Id.IntegerValue;
            Name = element.Name;
            DemParameter = new List<DemParameter>();

            foreach (Parameter parameter in element.Parameters)
            {
                if (!parameter.IsReadOnly)
                {
                    DemParameter.Add(new DemParameter(parameter));
                }
            }

            UniqueId = element.UniqueId;
        }




    }
}
