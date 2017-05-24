using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using LYtest.CFG;

namespace LYtest.Region
{
    public class NonLeafRegion : Region
    {
        List<CFGNode> nodes;
        public List<CFGNode> Nodes { get { return nodes; } }

        public NonLeafRegion(CFGNode header, List<CFGNode> nodes, List<Edge<CFGNode>> edges, string name="R") : base(header, edges, name)
        {
            this.nodes = nodes;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var line in nodes)
                sb.Append(line.ToString() + "\n");
            return sb.ToString();
        }
    }
}
