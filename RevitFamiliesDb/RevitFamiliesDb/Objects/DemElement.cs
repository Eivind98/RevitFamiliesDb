using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;

namespace RevitFamiliesDb.Objects
{
    public interface IDemElement
    {
        ElementId CreateThisMF(Document doc);
    }
    public class DemElement : IDemElement
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

            UniqueId = Guid.NewGuid().ToString();
            //UniqueId = element.UniqueId;
        }

        public virtual ElementId CreateThisMF(Document doc)
        {
            return ElementId.InvalidElementId;
        }
    }
}
