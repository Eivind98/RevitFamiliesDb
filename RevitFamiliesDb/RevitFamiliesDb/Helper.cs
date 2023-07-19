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


            switch (element.GetType().ToString())
            {
                case "Autodesk.Revit.DB.FloorType":
                    FloorType floor = element as FloorType;
                    return floor.GetCompoundStructure();
                case "Autodesk.Revit.DB.WallType":
                    WallType wall = element as WallType;
                    return wall.GetCompoundStructure();
                case "Autodesk.Revit.DB.CeilingType":
                    CeilingType ceiling = element as CeilingType;
                    return ceiling.GetCompoundStructure();
                case "Autodesk.Revit.DB.RoofType":
                    RoofType roof = element as RoofType;
                    return roof.GetCompoundStructure();
                default:
                    return null;
            }

        }


    }
}
