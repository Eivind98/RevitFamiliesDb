using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitFamiliesDb
{
    public class Helper
    {
        
        public static CompoundStructure GetCompound(Element element)
        {
            HostObjAttributes test = element as HostObjAttributes;

            return test.GetCompoundStructure();


            

        }


    }
}
