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
        public readonly Dictionary<IBaseBlock, T> Out = new Dictionary<IBaseBlock, T>();
        public readonly Dictionary<IBaseBlock, T> In = new Dictionary<IBaseBlock, T>();

        private CFGraph graph;

        protected abstract T Top { get; }

        protected IterativeCommonAlg(CFGraph g)
        {
            graph = g;
        }

        public virtual void Run()
        {
            foreach (var b in graph.Blocks)
                Out[b] = Top;
            
            var nodes = CFGNodeSet.GetNodes(graph);
            nodes.Remove(graph.root);

            var cont = true;

            while (cont)
            {
                cont = false;
                foreach (var node in nodes)
                {
                    var b = node.Value;
                    In[b] = MeetOp(node.ParentsNodes);
                    var prevOut = Out[b];
                    var newOut = Out[b] = TransferFunc(b);
                    if (ContCond(prevOut, newOut))
                        cont = true;
                }
            }
        }

        protected abstract bool ContCond(T a, T b);
        protected abstract T TransferFunc(IBaseBlock b);
        protected abstract T MeetOp(List<CFGNode> nodes);
    }

}
