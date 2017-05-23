using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using LYtest.CFG;

namespace LYtest.Region
{
    public class BodyRegion : NonLeafRegion
    {
        public BodyRegion(CFGNode header, List<CFGNode> nodes, List<Edge<CFGNode>> edges, string name="bodyR")
            : base(header, nodes, edges, name)
        {
        }
    }
}
