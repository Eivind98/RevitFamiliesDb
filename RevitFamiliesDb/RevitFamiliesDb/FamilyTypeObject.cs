using Autodesk.Revit.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace RevitFamiliesDb
{
    public class FamilyTypeObject
    {
        //public string AssemblyInstanceId { get; set; }
        //public string BoundingBox { get; set; }
        public string Name { get; set; }
        //public string CreatedPhaseId { get; set; }
        //public string DemolishedPhaseId { get; set; }
        //public string DesignOption { get; set; }

        //public Document Doc { get; set; }

        //public string Geometry { get; set; }
        //public string GroupId { get; set; }
        public int Id { get; set; }
        //public string IsModifiable { get; set; }
        //public string IsTransient { get; set; }
        public string Type { get; set; }
        public DemCompoundStructure ComStructureLayers { get; set; }


        public FamilyTypeObject(ElementId elementId, Document doc)
        {
            Id = elementId.IntegerValue;
            

            Element Ele = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .FirstOrDefault(x => x.Id == elementId);
            
            Name = Ele.Name;
            Type = Ele.GetType().ToString();

            try
            {
                ComStructureLayers = new DemCompoundStructure((Ele as HostObjAttributes).GetCompoundStructure());
            }
            catch
            {

            }
            
        }


        public Element CreateElement(Document doc)
        {

            //FilteredElementCollector collector = new FilteredElementCollector(doc);
            //ICollection<Element> floorTypes = collector.OfClass(typeof(FloorType)).ToElements();

            FloorType bullShitStuff = new FilteredElementCollector(doc).OfClass(typeof(FloorType)).First() as FloorType;


            //var element = new FilteredElementCollector(doc)
            //    .WhereElementIsElementType()
            //    .FirstOrDefault(x => x.Id == new ElementId(785)) as FloorType;

            FloorType ele = bullShitStuff.Duplicate(Guid.NewGuid().ToString()) as FloorType;

            ele.SetCompoundStructure(ComStructureLayers.Create());


            return null;
        }



        public static string PrintTypeObject(FamilyTypeObject obj)
        {
            //string output = obj.Id.ToString() + Environment.NewLine + obj.Name + Environment.NewLine + obj.Type.ToString() + Environment.NewLine ;

            string output = JsonConvert.SerializeObject(obj);

            return output;
        }


    }
}
