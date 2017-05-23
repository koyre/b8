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
        List<Edge<CFGNode>> edges;

        public List<CFGNode> Nodes { get { return nodes; } }
        public List<Edge<CFGNode>> Edges { get { return edges; } }

        public NonLeafRegion(CFGNode header, List<CFGNode> nodes, List<Edge<CFGNode>> edges, string name="R") : base(header)
        {
            this.nodes = nodes;
            this.edges = edges;
            this.Name = name;
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
