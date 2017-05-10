using System;
using System.Collections.Generic;
using System.Linq;
using LYtest.BaseBlocks;
using LYtest.LinearRepr;
using LYtest.LinearRepr.Values;

namespace LYtest.ReachingDefs
{
    using Definition = Tuple<LabelValue, IdentificatorValue>;

    public class GenKillBuilder
    {

        public readonly Dictionary<IBaseBlock, List<Definition>> Gen = new Dictionary<IBaseBlock, List<Definition>>();

        public readonly Dictionary<IBaseBlock, List<Definition>> Kill = new Dictionary<IBaseBlock, List<Definition>>();

        public HashSet<LabelValue> GenLabels(IBaseBlock b)
        {
            return new HashSet<LabelValue>(Gen[b].Select(d => d.Item1));
        }

        public HashSet<LabelValue> KillLabels(IBaseBlock b)
        {
            return new HashSet<LabelValue>(Kill[b].Select(d => d.Item1));
        }

        public GenKillBuilder(List<IBaseBlock> blocks)
        {
            foreach (var block in blocks)
            {
                var gen = CalcGen(block).ToList();

                var vars = new HashSet<IdentificatorValue>(gen.Select(e => e.Item2));
                var kill = blocks.Where(b => b != block).SelectMany(CalcGen).Where(e => vars.Contains(e.Item2));

                Gen[block] = gen.ToList();
                Kill[block] = kill.ToList();
            }
        }


        static bool IsDefinition(IThreeAddressCode t)
        {
            return t.AsDefinition() != null;
        }

        static IEnumerable<Tuple<LabelValue, IdentificatorValue>> CalcGen(IBaseBlock block)
        {
            return block.Enumerate()
                .Where(IsDefinition)
                .Select(t => Tuple.Create(t.Label, t.AsDefinition()));
        }

    }
}
