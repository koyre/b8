using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.BaseBlocks;
using LYtest.LinearRepr;
using LYtest.LinearRepr.Values;

namespace LYtest.ActiveVars
{
    using Definition = Tuple<LabelValue, IdentificatorValue>;

    class DefUseBuilder
    {
        public readonly Dictionary<IBaseBlock, List<Definition>> Use = new Dictionary<IBaseBlock, List<Definition>>();
        public readonly Dictionary<IBaseBlock, List<Definition>> Def = new Dictionary<IBaseBlock, List<Definition>>();

        public DefUseBuilder(List<IBaseBlock> blocks)
        {
            foreach (var block in blocks)
            {
                var defsB = new List<Definition>();
                var usesB = new List<Definition>();

                foreach (var t in block.Enumerate())
                {
                    var leftOperand = t.LeftOperand as IdentificatorValue;
                    var rightOperand = t.RightOperand as IdentificatorValue;

                    if (leftOperand != null && !defsB.Select(e => e.Item2).Contains(leftOperand))
                    {
                        usesB.Add(Tuple.Create(t.Label, leftOperand));
                    }
                    if (rightOperand != null && !defsB.Select(e => e.Item2).Contains(rightOperand))
                    {
                        usesB.Add(Tuple.Create(t.Label, rightOperand));
                    }

                    var def = t.AsDefinition();
                    if (def != null)
                    {
                        defsB.Add(Tuple.Create(t.Label, def));
                    }
                }

                Use[block] = usesB;
                Def[block] = defsB;
            }
        }

        public HashSet<LabelValue> DefLabels(IBaseBlock b)
        {
            return new HashSet<LabelValue>(Def[b].Select(d => d.Item1));
        }

        public HashSet<LabelValue> UseLabels(IBaseBlock b)
        {
            return new HashSet<LabelValue>(Use[b].Select(d => d.Item1));
        }
    }
}
