using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.BaseBlocks;
using LYtest.CFG;
using LYtest.LinearRepr.Values;

namespace LYtest.IterAlg
{
    public abstract class IterativeCommonAlg<T>
    {
        public readonly Dictionary<CFGNode, T> Out = new Dictionary<CFGNode, T>();
        public readonly Dictionary<CFGNode, T> In = new Dictionary<CFGNode, T>();

        protected CFGraph graph;

        protected abstract T Top { get; }

        protected IterativeCommonAlg(CFGraph g)
        {
            graph = g;
        }

        public virtual void Run()
        {
            foreach (var b in graph.GetVertices())
                Out[b] = Top;

            var nodes = new HashSet<CFGNode>(graph.GetVertices());
            nodes.Remove(graph.GetRoot());

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

        public virtual void ReverseRun()
        {
            foreach (var b in graph.GetVertices().Reverse())
                In[b] = Top;

            var nodes = new HashSet<CFGNode>(graph.GetVertices().Reverse());

            var cont = true;

            while (cont)
            {
                cont = false;
                foreach (var node in nodes)
                {
                    var childNodes = new List<CFGNode>();
                    if (node.directChild != null)
                    {
                        childNodes.Add(node.directChild);
                    }
                    Out[node] = MeetOp(childNodes);
                    var prevIn = In[node];
                    var newIn = In[node] = TransferFunc(node);

                    if (ContCond(prevIn, newIn))
                        cont = true;
                }
            }
        }

        protected abstract bool ContCond(T a, T b);
        protected abstract T TransferFunc(CFGNode node);
        protected abstract T MeetOp(List<CFGNode> nodes);
    }

}
