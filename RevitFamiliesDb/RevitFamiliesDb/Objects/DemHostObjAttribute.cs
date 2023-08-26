using Autodesk.Revit.DB;

namespace RevitFamiliesDb.Objects
{
    public class DemHostObjAttribute : DemElementType
    {
        public DemCompoundStructure DemCompoundStructure { get; set; }

        public DemHostObjAttribute()
        {
            
        }

        public DemHostObjAttribute(HostObjAttributes host) : base(host)
        {
            DemCompoundStructure = CreateDemCompoundStructure(host);
            
        }

        public DemCompoundStructure CreateDemCompoundStructure(HostObjAttributes host)
        {
            if (host.GetCompoundStructure() is CompoundStructure compoundStructure)
            {
                return new DemCompoundStructure(compoundStructure, host.Document);
            }
            return null;
        }


    }
}
