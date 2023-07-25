using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitFamiliesDb
{
    public static class Global
    {
        public static Document Doc { get; set; }
        public static Application App { get; set; }
        public static List<FamilyTypeObject> AllDemFamilyTypeObject { get; set; }


    }
}
