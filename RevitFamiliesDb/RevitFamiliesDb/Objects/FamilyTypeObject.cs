using Autodesk.Revit.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

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
        public string ThePath { get; set; }
        public string DemFamilyName { get; set; }

        public DemCompoundStructure ComStructureLayers { get; set; }
        public List<DemParameter> Parameters { get; set; }

        public BitmapImage Image
        {
            get
            {
                if (File.Exists(ThePath))
                {
                    try
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.UriSource = new Uri(ThePath);
                        bitmapImage.EndInit();
                        return bitmapImage;
                    }
                    catch (Exception)
                    {

                    }
                }

                return null;
            }
        }

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
            ThePath = Global.TheDirPath + DemGuid + ".jpg";
            Id = elementId.IntegerValue;

            Element Ele = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .FirstOrDefault(x => x.Id == elementId);

            Name = Ele.Name;
            Type = Ele.Category.Name;
            DemFamilyName = ((ElementType)Ele).FamilyName;


            System.Drawing.Size imgSize = new System.Drawing.Size(200, 200);

            Bitmap bitmap = ((ElementType)Ele).GetPreviewImage(imgSize);

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(ConvertBitmapToBitmapSource(bitmap)));

            encoder.QualityLevel = 25;

            string filename = ThePath;

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

            //Dictionary<string, Type> elementTypeMap = new Dictionary<string, Type>
            //{
            //    { "Ceilings", typeof(CeilingType) },
            //    { "Floors", typeof(FloorType) },
            //    { "Roofs", typeof(RoofType) },
            //    { "Walls", typeof(WallType) }
            //};


            //if (elementTypeMap.TryGetValue(this.Type, out Type elementType))
            //{
            //    ElementType randomElement = new FilteredElementCollector(doc)
            //        .OfClass(elementType)
            //        .First(i => (i as ElementType).FamilyName == DemFamilyName) as ElementType;

            //    if (randomElement != null)
            //    {
            //        var newElement = randomElement.Duplicate(Guid.NewGuid().ToString()) as ElementType;

            //        if (ComStructureLayers != null)
            //        {
            //            dynamic newElementType = Convert.ChangeType(newElement, elementType);
            //            newElementType.SetCompoundStructure(ComStructureLayers.Create());
            //        }

            //        foreach (DemParameter para in Parameters)
            //        {
            //            para.CreateThoseMF(newElement);
            //        }
            //    }
            //}

            //Type elementType = null;
            //switch (this.Type)
            //{
            //    case "Ceilings":
            //        elementType = typeof(CeilingType);
            //        break;
            //    case "Floors":
            //        elementType = typeof(FloorType);
            //        break;
            //    case "Roofs":
            //        elementType = typeof(RoofType);
            //        break;
            //    case "Walls":
            //        elementType = typeof(WallType);
            //        break;
            //}

            //if (elementType != null)
            //{
            //    ElementType randomElement = new FilteredElementCollector(doc).OfClass(elementType).First(i => (i as ElementType).FamilyName == DemFamilyName) as ElementType;
            //    var elementEle = randomElement.Duplicate(Guid.NewGuid().ToString()) as ElementType;

            //    if (ComStructureLayers != null)
            //    {
            //        elementEle.SetCompoundStructure(ComStructureLayers.Create());
            //    }

            //    foreach (DemParameter para in Parameters)
            //    {
            //        para.CreateThoseMF(elementEle);
            //    }
            //}




            switch (this.Type)
            {
                case "Ceilings":
                    CeilingType randomCeiling = new FilteredElementCollector(doc).OfClass(typeof(CeilingType)).First(i => (i as ElementType).FamilyName == DemFamilyName) as CeilingType;
                    var ceilingEle = randomCeiling.Duplicate(Guid.NewGuid().ToString()) as CeilingType;

                    if (ComStructureLayers != null)
                    {
                        ceilingEle.SetCompoundStructure(ComStructureLayers.Create());
                    }

                    foreach (DemParameter para in Parameters)
                    {
                        para.CreateThoseMF(ceilingEle);

                    }
                    break;
                case "Floors":
                    FloorType randomFloor = new FilteredElementCollector(doc).OfClass(typeof(FloorType)).First(i => (i as ElementType).FamilyName == DemFamilyName) as FloorType;
                    var floorEle = randomFloor.Duplicate(Guid.NewGuid().ToString()) as FloorType;

                    if (ComStructureLayers != null)
                    {
                        floorEle.SetCompoundStructure(ComStructureLayers.Create());
                    }

                    foreach (DemParameter para in Parameters)
                    {
                        para.CreateThoseMF(floorEle);

                    }
                    break;
                case "Roofs":
                    RoofType randomRoof = new FilteredElementCollector(doc).OfClass(typeof(RoofType)).First(i => (i as ElementType).FamilyName == DemFamilyName) as RoofType;
                    var roofEle = randomRoof.Duplicate(Guid.NewGuid().ToString()) as RoofType;
                    if (ComStructureLayers != null)
                    {
                        roofEle.SetCompoundStructure(ComStructureLayers.Create());
                    }
                    foreach (DemParameter para in Parameters)
                    {
                        para.CreateThoseMF(roofEle);

                    }
                    break;
                case "Walls":
                    WallType randomWall = new FilteredElementCollector(doc).OfClass(typeof(WallType)).First(i => (i as ElementType).FamilyName == DemFamilyName) as WallType;
                    var wallEle = randomWall.Duplicate(Guid.NewGuid().ToString()) as WallType;
                    if (ComStructureLayers != null)
                    {
                        wallEle.SetCompoundStructure(ComStructureLayers.Create());
                    }
                    foreach (DemParameter para in Parameters)
                    {
                        para.CreateThoseMF(wallEle);

                    }
                    break;

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

            foreach (FamilyTypeObject obj in ls)
            {
                list.Add(obj.Name);

            }

            return list;
        }

        public static string PrintTypeObject(FamilyTypeObject obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static string PrintTypeObject(List<FamilyTypeObject> obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
