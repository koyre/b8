using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.CFG;
using LYtest.IterAlg;
using LYtest.LinearRepr.Values;

namespace LYtest.ActiveVars
{
    public sealed class ActiveVarsIterAlg : IterativeCommonAlg<HashSet<LabelValue>>
    {
        protected override HashSet<LabelValue> Top => new HashSet<LabelValue>();

        private readonly DefUseBuilder DefUse;

        public ActiveVarsIterAlg(CFGraph g) : base(g)
        {
            DefUse = new DefUseBuilder(g.Blocks);
            ReverseRun();
        }

        protected override bool ContCond(HashSet<LabelValue> a, HashSet<LabelValue> b)
        {
            return !a.SetEquals(b);
        }

        protected override HashSet<LabelValue> TransferFunc(CFGNode node)
        {
            var res = DefUse.UseLabels(node.Value);
            res.UnionWith(Out[node].Except(DefUse.DefLabels(node.Value)));
            return res;
        }

        protected override HashSet<LabelValue> MeetOp(List<CFGNode> nodes)
        {
            return new HashSet<LabelValue>(nodes.SelectMany(n => In[n]));
        }
    }
}
