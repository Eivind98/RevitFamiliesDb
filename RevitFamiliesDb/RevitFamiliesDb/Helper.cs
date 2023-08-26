using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using RevitFamiliesDb.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
                                Helper.CreateDemElementsInRevit(listOfIList.Cast<DemElement>().ToList(), doc);
                            }
                            catch (Exception ex)
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
            foreach (DemElement yo in demElements)
            {
                yo.CreateThisMF(doc);
            }
        }

        public static DemElement GetDemElement(ElementId eleId, Document doc)
        {
            return GetDemElement(doc.GetElement(eleId));
        }

        public static DemElement GetDemElement(Element ele)
        {
            DemElement outPut = null;
            ElementId TypeId = ele.GetTypeId();

            if (TypeId != ElementId.InvalidElementId)
            {
                ele = ele.Document.GetElement(TypeId);
            }

            switch (ele)
            {
                case CeilingType ceiling when ele is CeilingType:
                    outPut = new DemCeilingType(ceiling);
                    break;
                case FloorType floor when ele is FloorType:
                    outPut = new DemFloorType(floor);
                    break;
                case RoofType roof when ele is RoofType:
                    outPut = new DemRoofType(roof);
                    break;
                case WallType wall when ele is WallType:
                    outPut = new DemWallType(wall);
                    break;
                case Material material when ele is Material:
                    outPut = new DemMaterial(material);
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
                    //List<DemMaterial> tust = GetDemMaterialFromAndForElement(demElement, doc);
                    //if(tust.Count() != 0)
                    //{
                    //    demElements.AddRange(tust);
                    //}
                }
            }




            return demElements;
        }


        public static List<T> LoadJsonWithErrorHandling<T>(string path)
        {
            try
            {
                var content = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<List<T>>(content);
            }
            catch (JsonReaderException)
            {

            }
            catch (JsonSerializationException)
            {

            }

            return new List<T>();
        }

        public static List<DemElement> LoadDemElementsFromFile()
        {
            List<DemElement> output = new List<DemElement>();

            output.AddRange(LoadJsonWithErrorHandling<DemCeilingType>(Global.TheCeilingPath));
            output.AddRange(LoadJsonWithErrorHandling<DemFloorType>(Global.TheFloorPath));
            output.AddRange(LoadJsonWithErrorHandling<DemRoofType>(Global.TheRoofPath));
            output.AddRange(LoadJsonWithErrorHandling<DemWallType>(Global.TheWallPath));

            return output;
        }

        public static void SaveDemElementsToFile(List<DemElement> demElements)
        {
            List<string> imagePaths = new List<string>();

            List<DemCeilingType> demCeilingTypes = new List<DemCeilingType>();
            List<DemFloorType> demFloorTypes = new List<DemFloorType>();
            List<DemRoofType> demRoofTypes = new List<DemRoofType>();
            List<DemWallType> demWallTypes = new List<DemWallType>();
            List<DemMaterial> demMaterial = new List<DemMaterial>();

            foreach (DemElement ele in demElements)
            {
                switch (ele)
                {
                    case DemCeilingType ceiling when ele is DemCeilingType:
                        demCeilingTypes.Add(ceiling);
                        imagePaths.Add(ceiling.ImagePath);
                        break;
                    case DemFloorType floor when ele is DemFloorType:
                        demFloorTypes.Add(floor);
                        imagePaths.Add(floor.ImagePath);
                        break;
                    case DemRoofType roof when ele is DemRoofType:
                        demRoofTypes.Add(roof);
                        imagePaths.Add(roof.ImagePath);
                        break;
                    case DemWallType wall when ele is DemWallType:
                        demWallTypes.Add(wall);
                        imagePaths.Add(wall.ImagePath);
                        break;
                    case DemMaterial material when ele is DemMaterial:
                        demMaterial.Add(material);
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
            File.WriteAllText(Global.TheMaterialPath, JsonConvert.SerializeObject(demMaterial));
        }

        public static List<DemMaterial> GetDemMaterialFromAndForElement(DemElement ele, Document doc)
        {
            List<DemMaterial> lst = new List<DemMaterial>();

            if (ele is DemHostObjAttribute)
            {
                if (((DemHostObjAttribute)ele).DemCompoundStructure != null)
                {
                    foreach (DemLayers l in (ele as DemHostObjAttribute).DemCompoundStructure.GetLayers)
                    {
                        var tist = l.MaterialId;
                        if (tist != -1)
                        {
                            DemMaterial mat = new DemMaterial(doc.GetElement(new ElementId(tist)) as Material);
                            lst.Add(mat);
                            l.MaterialGuid = mat.UniqueId;
                        }
                    }
                }

            }
            return lst;
        }

        void ChangeRenderingTexturePath(Document doc)
        {
            // As there is only one material in the sample 
            // project, we can use FilteredElementCollector 
            // and grab the first result

            Material mat = new FilteredElementCollector(doc)
              .OfClass(typeof(Material))
              .FirstElement() as Material;

            // Fixed path for new texture
            // Texture included in sample files

            string texturePath = Path.Combine(
              Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
              "new_texture.png");

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Changing material texture path");

                using (AppearanceAssetEditScope editScope = new AppearanceAssetEditScope(doc))
                {
                    Asset editableAsset = editScope.Start(mat.AppearanceAssetId);

                    // Getting the correct AssetProperty
                    AssetProperty assetProperty = editableAsset.FindByName("generic_diffuse");

                    Asset connectedAsset = assetProperty.GetConnectedProperty(0) as Asset;

                    var testingAsset = assetProperty.GetAllConnectedProperties();

                    foreach (var prop in testingAsset)
                    {
                        Trace.WriteLine(prop.Name);
                        Trace.WriteLine(prop.Type);
                        Trace.WriteLine(prop.IsReadOnly);
                    }

                    // Getting the right connected Asset
                    if (connectedAsset.Name == "UnifiedBitmapSchema")
                    {
                        AssetPropertyString path = connectedAsset.FindByName(UnifiedBitmap.UnifiedbitmapBitmap) as AssetPropertyString;

                        if (path.IsValidValue(texturePath)) path.Value = texturePath;
                    }
                    editScope.Commit(true);
                }
                TaskDialog.Show("Material texture path",
                  "Material texture path changed to:\n"
                  + texturePath);

                t.Commit();
                t.Dispose();
            }
        }

    }
}
