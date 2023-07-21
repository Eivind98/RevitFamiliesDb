using Autodesk.Revit.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RevitFamiliesDb
{
    public class DemLayers
    {
        
        public int DeckEmbeddingType { get; set; }
        public int DeckProfileId { get; set; }
        public int Function { get; set; }
        public bool IsValidObject { get; set; }
        public bool LayerCapFlag { get; set; }
        public int LayerId { get; set; }
        public int MaterialId { get; set; }
        public double Width { get; set; }

        public DemLayers(CompoundStructureLayer strucLayer)
        {
            
            if (Function == 200)
            {
                DeckEmbeddingType = (int)strucLayer.DeckEmbeddingType;
                DeckProfileId = strucLayer.DeckProfileId.IntegerValue;
            }
            
            Function = (int)strucLayer.Function;
            IsValidObject = strucLayer.IsValidObject;
            LayerCapFlag = strucLayer.LayerCapFlag;
            LayerId = strucLayer.LayerId;
            MaterialId = strucLayer.MaterialId.IntegerValue;
            Width = strucLayer.Width;
            
        }

        public CompoundStructureLayer CreateLayer()
        {
            CompoundStructureLayer Output = new CompoundStructureLayer();

            if (Function == 200)
            {
                Output.DeckEmbeddingType = (StructDeckEmbeddingType)DeckEmbeddingType;
                Output.DeckProfileId = new ElementId(DeckProfileId);

            }
            
            Output.Function = (MaterialFunctionAssignment)Function;
            Output.LayerCapFlag = LayerCapFlag;
            Output.MaterialId = new ElementId(MaterialId);
            Output.Width = Width;

            return Output;
        }
        
        public string PrintThisShit()
        {
            return JsonConvert.SerializeObject(this);

        }

    }

    public static class DemLayersExtensions
    {
        // Extension method to create a list of CompoundStructureLayers from a list of DemLayers objects
        public static IList<CompoundStructureLayer> CreateLayers(this List<DemLayers> demLayersList)
        {
            IList<CompoundStructureLayer> layersList = new List<CompoundStructureLayer>();

            foreach (var demLayer in demLayersList)
            {
                var compoundStructureLayer = demLayer.CreateLayer();
                layersList.Add(compoundStructureLayer);
            }

            return layersList;
        }
    }

}
