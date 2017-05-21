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
    public class CFGraph
    {
        // Graph presentation using QuickGraph because of its simplicity
        public BidirectionalGraph<CFGNode, Edge<CFGNode>> graph =
            new BidirectionalGraph<CFGNode, Edge<CFGNode>>();

        // Dictionary with {Edge : Type}
        public EdgeTypes EdgeTypes { get; }

        // Raw blocks
        public readonly List<IBaseBlock> Blocks;

        private DominatorTree.DominatorTree dominatorTree = null;

        // Constructor from list of blocks
        public CFGraph(List<IBaseBlock> blocks)
        {
            Blocks = blocks;
            EdgeTypes = new EdgeTypes();

            // First step - construct
            List<CFGNode> cfg_nodes = new List<CFGNode>(blocks.Count);
            for (int i = 0; i < blocks.Count; i++)
            {
                cfg_nodes.Add(new CFGNode(blocks[i]));
            }

            // Second step - make connections
            for (int i = 0; i < cfg_nodes.Count; i++)
            {
                var lastOp = cfg_nodes[i].Value.Enumerate().Last();
                if (i != cfg_nodes.Count - 1 &&
                    lastOp.Operation != LinearRepr.Values.Operation.Goto)
                {
                    cfg_nodes[i].SetDirectChild(cfg_nodes[i + 1]);
                }

                if (lastOp.Operation == LinearRepr.Values.Operation.Goto ||
                    lastOp.Operation == LinearRepr.Values.Operation.CondGoto)
                {
                    var gotoBlock = blocks.FirstOrDefault(b => b.Enumerate().First().Label.Equals(lastOp.Destination));
                    CFGNode goto_node = cfg_nodes.Find(el => el.Value == gotoBlock);
                    cfg_nodes[i].SetGotoChild(goto_node);
                }
            }

            // Make IList and add as vertexes
            var base_cfg_nodes = cfg_nodes as IList<CFGNode> ?? cfg_nodes.ToList();
            graph.AddVertexRange(base_cfg_nodes);


            // Add edges now
            foreach (var node in base_cfg_nodes)
            {
                if (node.directChild != null)
                {
                    graph.AddEdge(new Edge<CFGNode>(node, node.directChild));
                }
                
                if (node.gotoNode != null)
                {
                    graph.AddEdge(new Edge<CFGNode>(node, node.gotoNode));
                }
            }

            ClassificateEdges();

            buildDominatorTree();
        }

        public CFGNode GetRoot()
        {
            return (NumberOfVertices() > 0) ? graph.Vertices.ElementAt(0) : null;
        }

        public int NumberOfVertices()
        {
            return graph.Vertices.Count();
        }

        public IEnumerable<CFGNode> GetVertices()
        {
            return graph.Vertices;
        }

        // Classificate all edges on three types
        private void ClassificateEdges()
        {
            var depthTree = new DepthSpanningTree(this);
            foreach (var edge in graph.Edges)
            {
                // If this edges is on DST
                if (depthTree.Tree.Edges.Any(e => e.Target.Equals(edge.Target) && e.Source.Equals(edge.Source)))
                {
                    EdgeTypes.Add(edge, EdgeType.Coming);
                }
                // If there is back path in DST
                else if (depthTree.FindBackwardPath(edge.Source, edge.Target))
                {
                    EdgeTypes.Add(edge, EdgeType.Retreating);
                }
                else
                {
                    EdgeTypes.Add(edge, EdgeType.Cross);
                }
            }
        }


        public bool allRetreatingEdgesAreBackwards()
        {
            return EdgeTypes.Where(edgeType => edgeType.Value == EdgeType.Retreating)
                .Select(edgeType => edgeType.Key).ToList()
                .All(edge => isDominate(edge.Target, edge.Source));
        }

        private bool isDominate(CFGNode from, CFGNode to)
        {
            return dominatorTree.isDominate(from, to);
        }

        private int getNodeIndex(CFGNode node)
        {
            return graph.Vertices.ToList().IndexOf(node);
        }

        private void buildDominatorTree()
        {
            dominatorTree = new DominatorTree.DominatorTree(this);
        }

        public override string ToString()
        {
            var graphviz = new GraphvizAlgorithm<CFGNode, Edge<CFGNode>>(graph);
            return graphviz.Generate();
        }
    }
}
