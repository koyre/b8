using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.CFG;
using QuickGraph;

namespace LYtest.Region
{
    public class LeafRegion : Region
    {
        // LeafRegion has just one block (= header)
        public LeafRegion(CFGNode header, List<Edge<CFGNode>> edges, string name = "leafR")
            : base(header, edges, name)
        {
        }

    }
}
