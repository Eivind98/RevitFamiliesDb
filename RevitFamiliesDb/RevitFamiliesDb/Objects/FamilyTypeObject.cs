using Autodesk.Revit.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
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
        public string DemGuid { get; set; }
        public string Path { get; set; }
        public BitmapImage Image { get; set; }
        public DemCompoundStructure ComStructureLayers { get; set; }
        public List<DemParameter> Parameters { get; set; }

        public FamilyTypeObject()
        {
            
            DemGuid = Guid.NewGuid().ToString();

        }

        static BitmapSource ConvertBitmapToBitmapSource(Bitmap bmp)
        {
            return System.Windows.Interop.Imaging
              .CreateBitmapSourceFromHBitmap(
                bmp.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }

        public FamilyTypeObject(ElementId elementId, Document doc)
        {
            
            DemGuid = Guid.NewGuid().ToString();
            Path = Global.TheDirPath + DemGuid + ".jpg";
            Id = elementId.IntegerValue;
            



            Element Ele = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .FirstOrDefault(x => x.Id == elementId);
            
            Name = Ele.Name;
            Type = Ele.GetType().ToString();

            System.Drawing.Size imgSize = new System.Drawing.Size(200, 200);

            Bitmap bitmap = ((ElementType)Ele).GetPreviewImage(imgSize);
            
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(ConvertBitmapToBitmapSource(bitmap)));

            encoder.QualityLevel = 25;

            string filename = Path;

            FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write);

            encoder.Save(file);
            
            file.Close();

            try
            {
                ComStructureLayers = new DemCompoundStructure((Ele as HostObjAttributes).GetCompoundStructure());
            }
            catch
            {

            }

            Parameters = new List<DemParameter>();

            foreach (Parameter parameter in Ele.Parameters)
            {
                if (!parameter.IsReadOnly)
                {
                    Parameters.Add(new DemParameter(parameter));
                }

            }

        }


        public Element CreateElement(Document doc)
        {

            FloorType bullShitStuff = new FilteredElementCollector(doc).OfClass(typeof(FloorType)).First() as FloorType;


            FloorType ele = bullShitStuff.Duplicate(Guid.NewGuid().ToString()) as FloorType;

            ele.SetCompoundStructure(ComStructureLayers.Create());

            foreach (DemParameter para in Parameters)
            {
                para.CreateThoseMF(ele);


            }

            return null;
        }

        public static List<string> ToListStrings(List<FamilyTypeObject> ls)
        {
            if (ls == null || ls.Count == 0)
            {
                return null;
            }

            List<string> list = new List<string>();

            foreach(FamilyTypeObject obj in ls)
            {
                list.Add(obj.Name);

            }



            return list;
        }


        public static string PrintTypeObject(FamilyTypeObject obj)
        {
            //string output = obj.Id.ToString() + Environment.NewLine + obj.Name + Environment.NewLine + obj.Type.ToString() + Environment.NewLine ;

            return JsonConvert.SerializeObject(obj);
        }

        public static string PrintTypeObject(List<FamilyTypeObject> obj)
        {


            return JsonConvert.SerializeObject(obj);
        }


    }
}
