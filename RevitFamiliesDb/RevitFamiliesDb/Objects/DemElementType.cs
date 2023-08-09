using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RevitFamiliesDb.Objects
{
    public class DemElementType : DemElement
    {
        public string FamilyName {  get; set; }


        public DemElementType()
        {


        }

        public DemElementType(ElementType element) : base(element)
        {
            FamilyName = element.FamilyName;

        }

        

    }
}
