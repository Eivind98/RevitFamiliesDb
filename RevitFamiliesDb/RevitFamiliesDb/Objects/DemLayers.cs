using Autodesk.Revit.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public MaterialFunctionAssignment RevitFunction { get { return (MaterialFunctionAssignment)Function; } }
        public bool IsValidObject { get; set; }
        public bool LayerCapFlag { get; set; }
        public int LayerId { get; set; }
        public int MaterialId { get; set; }
        public string MaterialGuid { get; set; }
        public DemMaterial TheMaterial { get; set; }
        public string MaterialName
        {
            get
            {
                return TheMaterial is null ? "No Material Defined" : TheMaterial.Name;
            }
        }
        public double Width { get; set; }
        public double MetricWidth { get { return Math.Round(Width * 0.3048, 3); } }

        public DemLayers()
        {

        }

        public DemLayers(CompoundStructureLayer strucLayer, Document doc)
        {
            Function = (int)strucLayer.Function;



            if (Function == 200)
            {
                DeckEmbeddingType = (int)strucLayer.DeckEmbeddingType;
                DeckProfileId = strucLayer.DeckProfileId.IntegerValue;
            }



            IsValidObject = strucLayer.IsValidObject;
            LayerCapFlag = strucLayer.LayerCapFlag;
            LayerId = strucLayer.LayerId;
            MaterialId = strucLayer.MaterialId.IntegerValue;
            Width = strucLayer.Width;

            if (MaterialId != -1)
            {
                try
                {
                    Trace.Write("IS it this one?");
                    var test = doc.GetElement(strucLayer.MaterialId) as Material;
                    Trace.Write("OOOOOR is it this one?");
                    TheMaterial = new DemMaterial(test);
                }
                catch
                {
                    Trace.Write("Testing" + MaterialId);
                }



            }


        }

        public CompoundStructureLayer CreateLayer(Document doc)
        {
            CompoundStructureLayer Output = new CompoundStructureLayer();

            Output.Function = (MaterialFunctionAssignment)Function;

            if (Function == 200)
            {
                Output.DeckEmbeddingType = (StructDeckEmbeddingType)DeckEmbeddingType;
                Output.DeckProfileId = new ElementId(DeckProfileId);
            }

            Output.LayerCapFlag = LayerCapFlag;

            Output.MaterialId = TheMaterial.CreateThisMF(doc);
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
        public static IList<CompoundStructureLayer> CreateLayers(this List<DemLayers> demLayersList, Document doc)
        {
            IList<CompoundStructureLayer> layersList = new List<CompoundStructureLayer>();

            foreach (var demLayer in demLayersList)
            {
                var compoundStructureLayer = demLayer.CreateLayer(doc);
                layersList.Add(compoundStructureLayer);
            }

            return layersList;
        }
    }

}
