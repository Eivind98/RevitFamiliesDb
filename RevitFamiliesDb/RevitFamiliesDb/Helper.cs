using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using RevitFamiliesDb.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;

namespace RevitFamiliesDb
{
    public class MyEventHandler : IExternalEventHandler
    {
        public static ExternalEvent HandlerEvent = null;
        public static MyEventHandler Handler = null;
        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;
            try
            {
                //notifying me of raised event
                Trace.WriteLine("Raised");
                try
                {

                    IList listOfIList = Global.ElementsToProjectList;

                    if (listOfIList.Count > 0)
                    {
                        using (var tx = new Transaction(doc))
                        {
                            tx.Start("Creating");

                            try
                            {
                                Helper.CreateDemElementsInRevit(Global.ElementsToProjectList.Cast<DemElement>().ToList(), doc);
                            }
                            catch(Exception ex)
                            {
                                Trace.WriteLine("Trying to create them items");
                                Trace.WriteLine(ex.Message);
                            }

                            tx.Commit();
                        }
                    }
                }
                catch (Exception e)
                {
                    //catch whatever exception
                    throw e;
                }
            }
            catch (InvalidOperationException)
            {

                throw;
            }
        }

        public string GetName()
        {
            return "My Event Handler";
        }
        public void GetData(MechanicalEquipment mechEq)
        {

        }
    }


    public class Helper
    {
        public static void CreateDemElementsInRevit(List<DemElement> demElements, Document doc)
        {
            Dictionary<string, Action<DemElement, Document>> categoryToCreateMethod = new Dictionary<string, Action<DemElement, Document>>
            {
                { "Ceilings", (yo, Doc) => ((DemCeilingType)yo).CreateThisMF(Doc) },
                { "Floors", (yo, Doc) => ((DemFloorType)yo).CreateThisMF(Doc) },
                { "Roofs", (yo, Doc) => ((DemRoofType)yo).CreateThisMF(Doc) },
                { "Walls", (yo, Doc) => ((DemWallType)yo).CreateThisMF(Doc) }
            };

            foreach (DemElement yo in demElements)
            {
                if (categoryToCreateMethod.TryGetValue(yo.Category, out var createMethod))
                {
                    createMethod(yo, doc);
                }
            }



            //foreach (DemElement yo in demElements)
            //{
            //    switch (yo.Category)
            //    {
            //        case "Ceilings":
            //            ((DemCeilingType)yo).CreateThisMF(doc);
            //            break;
            //        case "Floors":
            //            ((DemFloorType)yo).CreateThisMF(doc);
            //            break;
            //        case "Roofs":
            //            ((DemRoofType)yo).CreateThisMF(doc);
            //            break;
            //        case "Walls":
            //            ((DemWallType)yo).CreateThisMF(doc);
            //            break;
            //    }
            //};
        }

        public static DemElement GetDemElement(Element ele)
        {
            DemElement outPut = null;

            switch (ele.Category.Name)
            {
                case "Ceilings":
                    outPut = new DemCeilingType(ele as CeilingType);
                    break;
                case "Floors":
                    outPut = new DemFloorType(ele as FloorType);
                    break;
                case "Roofs":
                    outPut = new DemRoofType(ele as RoofType);
                    break;
                case "Walls":
                    outPut = new DemWallType(ele as WallType);
                    break;
            }

            return outPut;
        }

        public static List<DemElement> CreateFromSelection(UIDocument uidoc)
        {
            List<DemElement> demElements = new List<DemElement>();

            var doc = uidoc.Document;
            var sel = uidoc.Selection;

            foreach (ElementId eleId in sel.GetElementIds())
            {
                Element element = doc.GetElement(eleId);
                DemElement demElement = GetDemElement(element);

                if (demElement != null)
                {
                    demElements.Add(demElement);
                }
            }

            return demElements;

        }

        public static List<DemElement> LoadDemElementsFromFile()
        {
            List<DemElement> output = new List<DemElement>();

            try
            {
                output.AddRange(JsonConvert.DeserializeObject<List<DemCeilingType>>(File.ReadAllText(Global.TheCeilingPath)));
            } 
            catch { }

            try
            {
                output.AddRange(JsonConvert.DeserializeObject<List<DemFloorType>>(File.ReadAllText(Global.TheFloorPath)));
            }
            catch { }

            try
            {
                output.AddRange(JsonConvert.DeserializeObject<List<DemRoofType>>(File.ReadAllText(Global.TheRoofPath)));
            }
            catch { }

            try
            {
                output.AddRange(JsonConvert.DeserializeObject<List<DemWallType>>(File.ReadAllText(Global.TheWallPath)));
            }
            catch { }

            return output;
        }

        public static void SaveDemElementsToFile(List<DemElement> demElements)
        {
            List<string> imagePaths = new List<string>();

            List<DemCeilingType> demCeilingTypes = new List<DemCeilingType>();
            List<DemFloorType> demFloorTypes = new List<DemFloorType>();
            List<DemRoofType> demRoofTypes = new List<DemRoofType>();
            List<DemWallType> demWallTypes = new List<DemWallType>();

            foreach (DemElement ele in demElements)
            {
                switch (ele.Category)
                {
                    case "Ceilings":
                        DemCeilingType ceiling = ele as DemCeilingType;
                        demCeilingTypes.Add(ceiling);
                        imagePaths.Add(ceiling.ImagePath);
                        break;
                    case "Floors":
                        DemFloorType floor = ele as DemFloorType;
                        demFloorTypes.Add(floor);
                        imagePaths.Add(floor.ImagePath);
                        break;
                    case "Roofs":
                        DemRoofType roof = ele as DemRoofType;
                        demRoofTypes.Add(roof);
                        imagePaths.Add(roof.ImagePath);
                        break;
                    case "Walls":
                        DemWallType wall = ele as DemWallType;
                        demWallTypes.Add(wall);
                        imagePaths.Add(wall.ImagePath);
                        break;
                }
            }

            string[] files = Directory.GetFiles(Global.TheDirPath);

            foreach (string image in files)
            {
                if (!imagePaths.Contains(image))
                {
                    File.Delete(image);
                }
            }

            File.WriteAllText(Global.TheCeilingPath, JsonConvert.SerializeObject(demCeilingTypes));
            File.WriteAllText(Global.TheFloorPath, JsonConvert.SerializeObject(demFloorTypes));
            File.WriteAllText(Global.TheRoofPath, JsonConvert.SerializeObject(demRoofTypes));
            File.WriteAllText(Global.TheWallPath, JsonConvert.SerializeObject(demWallTypes));
        }


        public static void SaveDemElementsToFile(DemElement demElement)
        {

            Dictionary<string, string> categoryPaths = new Dictionary<string, string>
            {
                { "Ceilings", Global.TheCeilingPath },
                { "Floors", Global.TheFloorPath },
                { "Roofs", Global.TheRoofPath },
                { "Walls", Global.TheWallPath }
            };

            if (categoryPaths.TryGetValue(demElement.Category, out string filePath))
            {
                File.WriteAllText(filePath, JsonConvert.SerializeObject(demElement));
            }

            //switch (demElement.Category)
            //{
            //    case "Ceilings":
            //        File.WriteAllText(Global.TheCeilingPath, JsonConvert.SerializeObject(demElement as DemCeilingType));
            //        break;
            //    case "Floors":
            //        File.WriteAllText(Global.TheFloorPath, JsonConvert.SerializeObject(demElement as DemFloorType));
            //        break;
            //    case "Roofs":
            //        File.WriteAllText(Global.TheRoofPath, JsonConvert.SerializeObject(demElement as DemRoofType));
            //        break;
            //    case "Walls":
            //        File.WriteAllText(Global.TheWallPath, JsonConvert.SerializeObject(demElement as DemWallType));
            //        break;
            //}
        }



        public static CompoundStructure GetCompound(Element element)
        {
            HostObjAttributes test = element as HostObjAttributes;

            return test.GetCompoundStructure();




        }


    }
}
