using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.BaseBlocks;
using QuickGraph;

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

        // Constructor from list of blocks
        public CFGraph(List<IBaseBlock> blocks)
        {
            this.Blocks = blocks;
            
            // First step - construct
            List<CFGNode> cfg_nodes = new List<CFGNode>(blocks.Count);
            for (int i = 0; i < blocks.Count; i++)
            {
                cfg_nodes.Add(new CFGNode(blocks[i]));
            }

            // Second step - make connections
            for (int i = 0; i < cfg_nodes.Count; i++)
            {
                if (i != cfg_nodes.Count - 1)
                {
                    cfg_nodes[i].SetDirectChild(cfg_nodes[i + 1]);
                }

                var lastOp = cfg_nodes[i].Value.Enumerate().Last();
                if (lastOp.Operation == LinearRepr.Values.Operation.Goto)
                {
                    var gotoBlock = blocks.First(b => b.Enumerate().First().Label.Equals(lastOp.Destination));
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
    }
}
