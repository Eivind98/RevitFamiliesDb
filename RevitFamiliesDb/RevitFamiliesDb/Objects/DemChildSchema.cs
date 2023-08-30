using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitFamiliesDb.Objects
{
    public class DemChildSchema
    {
        public string Name { get; set; }
        public Type TheType { get; set; }
        public object Value { get; set; }



        public DemChildSchema()
        {

        }



    }
}
