using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.CFG;

namespace LYtest.Region
{
    public class LeafRegion : Region
    {
        // LeafRegion has just one block (= header)
        public LeafRegion(CFGNode header) : base(header)
        {
        }

        public LeafRegion(CFGNode header, string name="leafR") : base(header, name)
        {
        }

    }
}
