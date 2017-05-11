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
        public BidirectionalGraph<CFGNode, Edge<CFGNode>> graph =
            new BidirectionalGraph<CFGNode, Edge<CFGNode>>();

        public EdgeTypes EdgeTypes { get; }

        public readonly List<IBaseBlock> Blocks;

        public CFGraph(List<IBaseBlock> blocks)
        {
            this.Blocks = blocks;
            List<CFGNode> cfg_nodes = new List<CFGNode>(blocks.Count);
            for (int i = 0; i < blocks.Count; i++)
            {
                cfg_nodes.Add(new CFGNode(blocks[i]));
            }

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

            var base_cfg_nodes = cfg_nodes as IList<CFGNode> ?? cfg_nodes.ToList();
            graph.AddVertexRange(base_cfg_nodes);

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
    }
}
