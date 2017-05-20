using LYtest.CFG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYtest.IterAlg.DominatorTree
{
    public class DominatorsIterAlg : IterativeCommonAlg<HashSet<CFGNode>>
    {
        protected override HashSet<CFGNode> Top => new HashSet<CFGNode>();
        public Dictionary<CFGNode, HashSet<CFGNode>> Dom;

        public DominatorsIterAlg(CFGraph g) : base(g)
        {
            Run();
            Dom = Out;
        }

        protected override bool ContCond(HashSet<CFGNode> a, HashSet<CFGNode> b)
        {
            return !a.SetEquals(b);
        }

        protected override HashSet<CFGNode> TransferFunc(CFGNode node)
        {
            var res = In[node];
            res.Add(node);
            return res;
        }

        protected override HashSet<CFGNode> MeetOp(List<CFGNode> nodes)
        {
            var node = nodes.FirstOrDefault();
            var res = new HashSet<CFGNode>();
            if (node != null)
            {
                res.UnionWith(Out[node]);
                foreach (var set in nodes.Skip(1).Select(x => Out[x]))
                {
                    res.IntersectWith(set);
                }
            }
            return res;
        }

        public override void Run()
        {
            var nodes = new List<CFGNode>(graph.GetVertices());
            Out[graph.GetRoot()] = new HashSet<CFGNode>();
            Out[graph.GetRoot()].Add(graph.GetRoot());

            nodes.Remove(graph.GetRoot());
            foreach (var b in nodes)
            {
                Out[b] = new HashSet<CFGNode>(graph.GetVertices());
                In[b] = new HashSet<CFGNode>();
            }

            var cont = true;
            while (cont)
            {
                cont = false;
                foreach (var node in nodes)
                {
                    In[node] = MeetOp(node.ParentsNodes);
                    var prevOut = Out[node];
                    var newOut = Out[node] = TransferFunc(node);
                    if (ContCond(prevOut, newOut))
                        cont = true;
                }
            }
        }
    }
}
