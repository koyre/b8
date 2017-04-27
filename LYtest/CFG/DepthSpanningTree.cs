using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.BaseBlocks;
using QuickGraph;
using QuickGraph.Graphviz;

namespace LYtest.CFG
{
    public class DepthSpanningTree
    {
        public HashSet<CFGNode> Visited { get; }
        public Dictionary<CFGNode, int> Numbers { get; }
        public BidirectionalGraph<CFGNode, Edge<CFGNode>> SpanningTree { get; }

        public DepthSpanningTree(CFGraph cfg)
        {
            int numberOfVertices = cfg.num_of_vertexes - 1;
            Visited = new HashSet<CFGNode>();
            SpanningTree = new BidirectionalGraph<CFGNode, Edge<CFGNode>>();
            Numbers = new Dictionary<CFGNode, int>();
            
            BuildTree(cfg.root, ref numberOfVertices);
        }

        private void BuildTree(CFGNode node, ref int currentNumber)
        {
            if (node == null)
                return;
            Visited.Add(node);
            CFGLookup lookup = new CFGLookup(node);

            if (!lookup.CanMoveDirect() && !lookup.CanMoveGoto())
            {
                Numbers[lookup.Current] = currentNumber;
                return;
            }

            var children = new List<CFGNode>();
            if (lookup.CanMoveDirect())
            {
                lookup.MoveDirect();
                children.Add(lookup.Current);
                lookup.MoveBack();
            }
            if (lookup.CanMoveGoto())
            {
                lookup.MoveGoto();
                children.Add(lookup.Current);
                lookup.MoveBack();
            }

            foreach (var child in children)
            {
                if (!Visited.Contains(child))
                {
                    if (!SpanningTree.Vertices.Contains(node))
                        SpanningTree.AddVertex(node);

                    if (!SpanningTree.Vertices.Contains(child))
                        SpanningTree.AddVertex(child);

                    SpanningTree.AddEdge(new Edge<CFGNode>(node, child));
                    BuildTree(child, ref currentNumber);
                }

                Numbers[node] = currentNumber;
                currentNumber -= 1;
            }
        }

        public override string ToString()
        {
            var graphviz = new GraphvizAlgorithm<CFGNode, Edge<CFGNode>>(SpanningTree);
            return graphviz.Generate();
        }
    }
}
