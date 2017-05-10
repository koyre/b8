using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.BaseBlocks;
using LYtest.CFG;
using LYtest.LinearRepr.Values;

namespace LYtest.ReachingDefs
{
    public class ReachingDefsIterAlg
    {

        public Dictionary<IBaseBlock, HashSet<LabelValue>> Out = new Dictionary<IBaseBlock, HashSet<LabelValue>>();
        public Dictionary<IBaseBlock, HashSet<LabelValue>> In = new Dictionary<IBaseBlock, HashSet<LabelValue>>();

        public ReachingDefsIterAlg(CFGraph graph)
        {
            var genKill = new GenKillBuilder(graph.Blocks);
            foreach (var b in graph.Blocks)
            {
                Out[b] = new HashSet<LabelValue>();
            }

            var nodes = CFGNodeSet.GetNodes(graph);
            nodes.Remove(graph.root);
            var outModified = true;

            while (outModified)
            {
                outModified = false;

                foreach (var node in nodes)
                {
                    var b = node.Value;
                    In[b] = new HashSet<LabelValue>(node.ParentsNodes.SelectMany(n => Out[n.Value]));

                    var oldOut = Out[b];
                    var outt = genKill.GenLabels(b);
                    outt.UnionWith(In[b].Except(genKill.KillLabels(b)));
                    Out[b] = outt;
                    if (!outt.SetEquals(oldOut))
                    {
                        outModified = true;
                    }
                }
            }
        }
    }
}
