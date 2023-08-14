using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
