using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitFamiliesDb
{
    public class FamilyTypeObject
    {
        public string typeName {  get; set; }
        public Type type { get; set; }
        public CompoundStructure layerStructure { get; set; }


        public FamilyTypeObject(ElementId elementId, Document doc)
        {
            var element = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .FirstOrDefault(x => x.Id == elementId);
            
            switch (element.GetType().ToString())
            {
                case "Autodesk.Revit.DB.FloorType":
                    FloorType ele = element as FloorType;
                    typeName = ele.Name;
                    type = typeof(FloorType);
                    layerStructure = ele.GetCompoundStructure();
                    break;

            }



        }


    }
}
