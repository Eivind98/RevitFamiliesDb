﻿using Autodesk.Revit.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitFamiliesDb
{
    public class DemCompoundStructure
    {

        public double CutOffHeight { get; set; }
        public int EndCap { get; set; }
        public bool HasStructuralDeck { get; set; }
        public bool IsEmpty { get; set; }
        public bool IsValidObject { get; set; }
        public bool IsVerticalCompound { get; set; }
        public int LayersCount { get; set; }
        public double MinimumSampleHeight { get; set; }
        public int OpeningWrapping { get; set; }
        public double SampleHeight { get; set; }
        public int StructuralMaterialIndex { get; set; }
        public int VariableLayerIndex { get; set; }
        public bool CanSplitAndMergeRegionsBeUsed { get; set; }
        public int GetFirstCoreLayerIndex { get; set; }
        public int GetLastCoreLayerIndex { get; set; }
        public List<DemLayers> GetLayers { get; set; }
        public IList<int> GetRegionIds { get; set; }
        public IList<int> GetSegmentIds { get; set; }
        public double GetWidth { get; set; }
        public double GetWidthMetric { get { return Math.Round(GetWidth * 0.3048, 3); } }
        public bool IsVerticallyHomogeneous { get; set; }


        public DemCompoundStructure()
        {

        }

        public DemCompoundStructure(CompoundStructure comStructure, Document doc)
        {

            EndCap = (int)comStructure.EndCap;
            HasStructuralDeck = comStructure.HasStructuralDeck;
            IsEmpty = comStructure.IsEmpty;
            IsValidObject = comStructure.IsValidObject;
            IsVerticalCompound = comStructure.IsVerticallyCompound;
            LayersCount = comStructure.LayerCount;
            MinimumSampleHeight = comStructure.MinimumSampleHeight;
            OpeningWrapping = (int)comStructure.OpeningWrapping;
            StructuralMaterialIndex = comStructure.StructuralMaterialIndex;
            VariableLayerIndex = comStructure.VariableLayerIndex;
            CanSplitAndMergeRegionsBeUsed = comStructure.CanSplitAndMergeRegionsBeUsed();
            GetFirstCoreLayerIndex = comStructure.GetFirstCoreLayerIndex();
            GetLastCoreLayerIndex = comStructure.GetLastCoreLayerIndex();
            GetLayers = comStructure.GetLayers().Select(layer => new DemLayers(layer, doc)).ToList(); ;
            GetWidth = comStructure.GetWidth();
            IsVerticallyHomogeneous = comStructure.IsVerticallyHomogeneous();
            

            if (IsVerticalCompound)
            {
                CutOffHeight = comStructure.CutoffHeight;
                SampleHeight = comStructure.SampleHeight;
                GetRegionIds = comStructure.GetRegionIds();
                GetSegmentIds = comStructure.GetSegmentIds();
            }

        }

        public CompoundStructure Create(Document doc)
        {

            IList<CompoundStructureLayer> layers = GetLayers.CreateLayers(doc);

            CompoundStructure output = CompoundStructure.CreateSimpleCompoundStructure(layers);
            output.EndCap = (EndCapCondition)EndCap;
            output.OpeningWrapping = (OpeningWrappingCondition)OpeningWrapping;
            output.StructuralMaterialIndex = StructuralMaterialIndex;
            output.VariableLayerIndex = VariableLayerIndex;
            output.SetNumberOfShellLayers(ShellLayerType.Exterior, GetFirstCoreLayerIndex);
            output.SetNumberOfShellLayers(ShellLayerType.Interior, GetLayers.Count - (GetLastCoreLayerIndex + 1));



            return output;
        }

        public string Print()
        {
            return JsonConvert.SerializeObject(this);
        }


    }
}
