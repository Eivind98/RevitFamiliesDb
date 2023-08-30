using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitFamiliesDb.Objects
{
    public class DemParentSchema
    {
        public string Name { get; set; }
        public List<DemChildSchema> Properties { get; set; } = new List<DemChildSchema>();


        public DemParentSchema(AssetProperty parentSchemaAsset)
        {
            if (parentSchemaAsset != null)
            {
                Name = parentSchemaAsset.Name;
                if (parentSchemaAsset.NumberOfConnectedProperties != 0)
                {
                    Asset childSchemaAsset = (Asset)parentSchemaAsset.GetConnectedProperty(0);

                    if (childSchemaAsset != null)
                    {

                        List<string> propertiesString = new List<string>();
                        var childName = childSchemaAsset.Name;

                        Type theCurrentSchemaType = null;

                        switch (childName)
                        {
                            case "BumpMapSchema":
                                theCurrentSchemaType = typeof(BumpMap);
                                theCurrentSchemaType.GetProperties().ToList().ForEach(pro => propertiesString.Add(pro.Name));
                                break;
                            case "CheckerSchema":
                                theCurrentSchemaType = typeof(Checker);
                                theCurrentSchemaType.GetProperties().ToList().ForEach(pro => propertiesString.Add(pro.Name));
                                break;
                            case "GradientSchema":
                                theCurrentSchemaType = typeof(Gradient);
                                theCurrentSchemaType.GetProperties().ToList().ForEach(pro => propertiesString.Add(pro.Name));
                                break;
                            case "MarbleSchema":
                                theCurrentSchemaType = typeof(Marble);
                                theCurrentSchemaType.GetProperties().ToList().ForEach(pro => propertiesString.Add(pro.Name));
                                break;
                            case "NoiseSchema":
                                theCurrentSchemaType = typeof(Noise);
                                theCurrentSchemaType.GetProperties().ToList().ForEach(pro => propertiesString.Add(pro.Name));
                                break;
                            case "SpeckleSchema":
                                theCurrentSchemaType = typeof(Speckle);
                                theCurrentSchemaType.GetProperties().ToList().ForEach(pro => propertiesString.Add(pro.Name));
                                break;
                            case "TileSchema":
                                theCurrentSchemaType = typeof(Tile);
                                theCurrentSchemaType.GetProperties().ToList().ForEach(pro => propertiesString.Add(pro.Name));
                                break;
                            case "UnifiedBitmapSchema":
                                theCurrentSchemaType = typeof(UnifiedBitmap);
                                theCurrentSchemaType.GetProperties().ToList().ForEach(pro => propertiesString.Add(pro.Name));
                                break;
                            case "WaveSchema":
                                theCurrentSchemaType = typeof(Wave);
                                theCurrentSchemaType.GetProperties().ToList().ForEach(pro => propertiesString.Add(pro.Name));
                                break;
                            case "WoodSchema":
                                theCurrentSchemaType = typeof(Wood);
                                theCurrentSchemaType.GetProperties().ToList().ForEach(pro => propertiesString.Add(pro.Name));
                                break;
                        }

                        foreach (var propStr in propertiesString)
                        {
                            DemChildSchema objChildSchema = new DemChildSchema();
                            objChildSchema.Name = theCurrentSchemaType.GetProperty(propStr).GetValue(theCurrentSchemaType, null).ToString();

                            var schemaAssetChild = childSchemaAsset.FindByName(objChildSchema.Name);

                            if (schemaAssetChild != null)
                            {
                                switch (schemaAssetChild)
                                {
                                    case AssetPropertyBoolean bo when schemaAssetChild is AssetPropertyBoolean:
                                        objChildSchema.Value = bo.Value;
                                        objChildSchema.TheType = typeof(AssetPropertyBoolean);
                                        break;
                                    case AssetPropertyString str when schemaAssetChild is AssetPropertyString:
                                        objChildSchema.Value = str.Value;
                                        objChildSchema.TheType = typeof(AssetPropertyString);
                                        break;
                                    case AssetPropertyDouble doub when schemaAssetChild is AssetPropertyDouble:
                                        objChildSchema.Value = doub.Value;
                                        objChildSchema.TheType = typeof(AssetPropertyDouble);
                                        break;
                                    case AssetPropertyInteger integer when schemaAssetChild is AssetPropertyInteger:
                                        objChildSchema.Value = integer.Value;
                                        objChildSchema.TheType = typeof(AssetPropertyInteger);
                                        break;
                                    case AssetPropertyDistance distance when schemaAssetChild is AssetPropertyDistance:
                                        objChildSchema.Value = distance.Value;
                                        objChildSchema.TheType = typeof(AssetPropertyDistance);
                                        break;
                                    case AssetPropertyDoubleArray4d array when schemaAssetChild is AssetPropertyDoubleArray4d:
                                        objChildSchema.Value = array.GetValueAsDoubles();
                                        objChildSchema.TheType = typeof(AssetPropertyDoubleArray4d);
                                        break;
                                }

                                Properties.Add(objChildSchema);
                            }

                        }


                    }
                }
            }
        }





    }
}
