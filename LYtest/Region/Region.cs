using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.CFG;
using QuickGraph;

namespace LYtest.Region
{
    public class Region
    {
        List<Edge<CFGNode>> edges;
        public List<Edge<CFGNode>> Edges { get { return edges; } }
        public CFGNode Header { get; }
        public string Name { get; set; }

        public Region(CFGNode header, List<Edge<CFGNode>> edges, string name= "R")
        {
            Header = header;
            this.edges = edges;
            Name = name;
        }        

        public override string ToString()
        {
            return Header.ToString();
        }
    }
}
