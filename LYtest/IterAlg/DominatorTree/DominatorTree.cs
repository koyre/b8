using LYtest.CFG;
using LYtest.IterAlg.DominatorTree;
using QuickGraph;
using QuickGraph.Graphviz;
using System.Collections.Generic;
using System.Linq;

namespace LYtest.DominatorTree
{
    public class DominatorTree
    {
        // Graph presentation using QuickGraph because of its simplicity
        public BidirectionalGraph<DominatorTreeNode, Edge<DominatorTreeNode>> graph =
            new BidirectionalGraph<DominatorTreeNode, Edge<DominatorTreeNode>>();
        
        // Constructor from list of blocks
        public DominatorTree(CFGraph cfg)
        {
            var doms = new DominatorsIterAlg(cfg).Dom;

            // Make IList and add as vertexes
            var vertices = doms.Keys.Select(x => new DominatorTreeNode(x)).ToList();
            graph.AddVertexRange(vertices);
            
            foreach (var node in vertices)
            {
                var dominatedBy = doms[node.CFGNode].ToList();
                dominatedBy.Reverse();
                var cfgClosestDominator = dominatedBy.Skip(1).FirstOrDefault();
                if (cfgClosestDominator != null)
                {
                    var domClosestDominator = vertices.FirstOrDefault(x => x.CFGNode == cfgClosestDominator);

                    node.ParentNode = domClosestDominator;
                    domClosestDominator.AddChild(node);

                    graph.AddEdge(new Edge<DominatorTreeNode>(domClosestDominator, node));
                }
            }
        }

        public DominatorTreeNode GetRoot()
        {
            return (NumberOfVertices() > 0) ? graph.Vertices.ElementAt(0) : null;
        }

        public int NumberOfVertices()
        {
            return graph.Vertices.Count();
        }

        public IEnumerable<DominatorTreeNode> GetVertices()
        {
            return graph.Vertices;
        }

        // Classificate all edges on three types
        public override string ToString()
        {
            var graphviz = new GraphvizAlgorithm<DominatorTreeNode, Edge<DominatorTreeNode>>(graph);
            return graphviz.Generate();
        }
    }
}
