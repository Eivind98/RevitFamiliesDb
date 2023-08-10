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
    public class DemRoofType : DemHostObjAttribute
    {
        public string ThermalProperties { get; set; }

        public DemRoofType()
        {

        }

        public DemRoofType(RoofType roof) : base(roof)
        {
            ThermalProperties = roof.ThermalProperties.ToString();

        }

        public void CreateThisMF(Document doc)
        {

            RoofType randomRoof = new FilteredElementCollector(doc).OfClass(typeof(RoofType)).First(i => (i as ElementType).FamilyName == this.FamilyName) as RoofType;
            var roofEle = randomRoof.Duplicate(Guid.NewGuid().ToString()) as RoofType;

            if (DemCompoundStructure != null)
            {
                roofEle.SetCompoundStructure(DemCompoundStructure.Create());
            }

            foreach (DemParameter para in this.DemParameter)
            {
                para.CreateThoseMF(roofEle);

            }


        }

    }
}
