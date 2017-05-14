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
        public Dictionary<CFGNode, int> Numbers { get; }
        public BidirectionalGraph<CFGNode, Edge<CFGNode>> SpanningTree { get; }

        private HashSet<CFGNode> visited = null;

        public DepthSpanningTree(CFGraph cfg)
        {
            int numberOfVertices = cfg.NumberOfVertices() - 1;
            visited = new HashSet<CFGNode>();
            SpanningTree = new BidirectionalGraph<CFGNode, Edge<CFGNode>>();
            Numbers = new Dictionary<CFGNode, int>();

            var root = cfg.GetRoot();
            BuildTree(root, ref numberOfVertices);
        }

        private void BuildTree(CFGNode node, ref int currentNumber)
        {
            if (node == null)
                return;
            visited.Add(node);
            if (node.directChild == null && node.gotoNode == null)
            {
                Numbers[node] = currentNumber;
                return;
            }

            var children = new List<CFGNode>();
            if (node.directChild != null)
            {
                children.Add(node.directChild);
            }
            if (node.gotoNode != null)
            {
                children.Add(node.gotoNode);
            }


            if (!SpanningTree.Vertices.Contains(node))
                SpanningTree.AddVertex(node);
            foreach (var child in children)
            {
                if (!SpanningTree.Vertices.Contains(child))
                    SpanningTree.AddVertex(child);
                SpanningTree.AddEdge(new Edge<CFGNode>(node, child));

                if (!visited.Contains(child))
                {
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
