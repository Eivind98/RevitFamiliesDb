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
    public class DemCeilingType : DemHostObjAttribute
    {
        public string ThermalProperties { get; set; }
        //public string ThermalProperties { get; set; }

        public DemCeilingType()
        {

        }

        public DemCeilingType(CeilingType ceiling) : base(ceiling)
        {
            ThermalProperties = ceiling.ThermalProperties?.ToString();

        }

        public void CreateThisMF(Document doc)
        {

            CeilingType randomCeiling = new FilteredElementCollector(doc).OfClass(typeof(CeilingType)).First(i => (i as ElementType).FamilyName == this.FamilyName) as CeilingType;
            var CeilingEle = randomCeiling.Duplicate(Guid.NewGuid().ToString()) as CeilingType;

            if (DemCompoundStructure != null)
            {
                CeilingEle.SetCompoundStructure(DemCompoundStructure.Create(doc));
            }

            foreach (DemParameter para in this.DemParameter)
            {
                para.CreateThoseMF(CeilingEle);

            }
        }



    }
}
