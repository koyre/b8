using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using LYtest.CFG;

namespace LYtest.Region
{
    public class LoopRegion : NonLeafRegion
    {
        BodyRegion bodyReg;
        public BodyRegion BodyReg { get { return bodyReg; } }

        public LoopRegion(CFGNode header, List<CFGNode> nodes, List<Edge<CFGNode>> edges, BodyRegion body, string name="loopR")
            : base (header, nodes, edges, name)
        {
            bodyReg = body;
        }
    }
}