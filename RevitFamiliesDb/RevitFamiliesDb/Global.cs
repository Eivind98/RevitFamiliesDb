using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;

namespace RevitFamiliesDb
{
    public static class Global
    {
        public static UIDocument UIDoc { get; set; }
        public static Document Doc { get; set; }
        public static Application App { get; set; }
        public static List<FamilyTypeObject> AllDemFamilyTypeObject { get; set; }
        public static string ThePath { get; set; } = "C:\\Users\\eev_9\\OneDrive\\02 - Projects\\Programming stuff\\Test.json";


    }
}
