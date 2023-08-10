using Autodesk.Revit.DB;
using RevitFamiliesDb.Objects;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RevitFamiliesDb
{
    public class DemFloorType : DemHostObjAttribute
    {
        public bool IsFoundationSlab { get; set; }
        public int StructuralMaterialId { get; set; }
        //public string ThermalProperties { get; set; }

        public DemFloorType()
        {

        }

        public DemFloorType(FloorType floor) : base(floor)
        {
            IsFoundationSlab = floor.IsFoundationSlab;
            StructuralMaterialId = floor.StructuralMaterialId.IntegerValue;

        }

        public void CreateThisMF(Document doc)
        {

            FloorType randomFloor = new FilteredElementCollector(doc).OfClass(typeof(FloorType)).First(i => (i as ElementType).FamilyName == this.FamilyName) as FloorType;
            var floorEle = randomFloor.Duplicate(Guid.NewGuid().ToString()) as FloorType;

            if (DemCompoundStructure != null)
            {
                floorEle.SetCompoundStructure(DemCompoundStructure.Create());
            }

            foreach (DemParameter para in this.DemParameter)
            {
                para.CreateThoseMF(floorEle);

            }


        }

    }
}
