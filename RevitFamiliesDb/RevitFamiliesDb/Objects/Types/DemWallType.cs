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
    public class DemWallType : DemHostObjAttribute
    {
        public int Function { get; set; }
        public string FunctionName { get { return ((WallFunction)Function).ToString(); } }
        public int Kind { get; set; }
        public string KindName { get { return ((WallKind)Kind).ToString(); } }
        public string ThermalProperties { get; set; }
        

        public DemWallType()
        {

        }

        public DemWallType(WallType wall) : base(wall)
        {
            Function = (int)wall?.Function;
            Kind = (int)wall?.Kind;
            ThermalProperties = wall.ThermalProperties?.ToString();

        }

        public void CreateThisMF(Document doc)
        {

            WallType randomWall = new FilteredElementCollector(doc).OfClass(typeof(WallType)).First(i => (i as ElementType).FamilyName == this.FamilyName) as WallType;
            var WallEle = randomWall.Duplicate(Guid.NewGuid().ToString()) as WallType;

            if (DemCompoundStructure != null)
            {
                WallEle.SetCompoundStructure(DemCompoundStructure.Create(doc));
            }

            foreach (DemParameter para in this.DemParameter)
            {
                para.CreateThoseMF(WallEle);

            }

        }
    }
}
