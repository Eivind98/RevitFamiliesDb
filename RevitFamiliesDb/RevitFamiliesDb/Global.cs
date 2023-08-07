using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using System.Collections;

namespace RevitFamiliesDb
{
    public static class Global
    {
        public static UIDocument UIDoc { get; set; }
        public static Document Doc { get; set; }
        public static Application App { get; set; }
        public static List<FamilyTypeObject> AllDemFamilyTypeObject { get; set; }
        public static string TheJsonPath { get; set; } = "C:\\Users\\eev_9\\OneDrive\\02 - Projects\\Programming stuff\\Test.json";
        public static string TheDirPath { get; set; } = "C:\\Users\\eev_9\\OneDrive\\02 - Projects\\Programming stuff\\Pic\\";
        public static IList ElementsToProjectList { get; set; }



    }
}
