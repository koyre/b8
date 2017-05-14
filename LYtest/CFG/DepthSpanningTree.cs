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
        // Verteces numeration
        public Dictionary<CFGNode, int> Numbers { get; }

        // Tree
        public BidirectionalGraph<CFGNode, Edge<CFGNode>> Tree { get; }

        // visited verteces array
        private HashSet<CFGNode> visited = null;

        // Constructor from CFGraph
        public DepthSpanningTree(CFGraph cfg)
        {
            int numberOfVertices = cfg.NumberOfVertices() - 1;
            visited = new HashSet<CFGNode>();
            Tree = new BidirectionalGraph<CFGNode, Edge<CFGNode>>();
            Numbers = new Dictionary<CFGNode, int>();

            var root = cfg.GetRoot();
            BuildTree(root, ref numberOfVertices);
        }

        // Build tree
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


            if (!Tree.Vertices.Contains(node))
                Tree.AddVertex(node);
            foreach (var child in children)
            {
                if (!visited.Contains(child))
                {
                    if (!Tree.Vertices.Contains(child))
                        Tree.AddVertex(child);
                    Tree.AddEdge(new Edge<CFGNode>(node, child));

                    BuildTree(child, ref currentNumber);
                }

                Numbers[node] = currentNumber;
                currentNumber -= 1;
            }
        }

        // Finds back path from source to target, true if it is.
        public bool FindBackwardPath(CFGNode source, CFGNode target)
        {
            var result = false;

            var incomingEdges = Tree.InEdges(source);
            while (incomingEdges.Count() > 0)
            {
                var edge = incomingEdges.First();
                if (edge.Source.Equals(target))
                {
                    result = true;
                    break;
                }
                else
                {
                    incomingEdges = Tree.InEdges(edge.Source);
                }
            }

            return result;
        }

        public override string ToString()
        {
            var graphviz = new GraphvizAlgorithm<CFGNode, Edge<CFGNode>>(Tree);
            return graphviz.Generate();
        }
    }
}
