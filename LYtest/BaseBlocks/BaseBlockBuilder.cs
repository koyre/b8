using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.LinearRepr;
using LYtest.LinearRepr.Values;

namespace LYtest.BaseBlocks
{

    class LinearToBaseBlock
    {
        private class BaseBlockBuilder
        {
            private readonly List<IBaseBlock> _blocks = new List<IBaseBlock>();

            private IBaseBlock _currentBlock = new BaseBlock();

            public void Add(IThreeAddressCode elem)
            {
                _currentBlock.Append(elem);
            }

            public void Flush()
            {
                if (!_currentBlock.Enumerate().Any())
                    return;
                _blocks.Add(_currentBlock);
                _currentBlock = new BaseBlock();
            }

            public List<IBaseBlock> GetBlocks()
            {
                this.Flush();
                return _blocks;
            }
        }

        private static HashSet<LabelValue> FindControlPoints(List<LinearRepresentation> lst)
        {
            var controlPoints = new HashSet<LabelValue>();

            var forceAdd = true;

            foreach (var lin in lst)
            {
                if (forceAdd)
                    controlPoints.Add(lin.Label);
                forceAdd = false;

                var op = lin.Operation;

                if (op == Operation.Goto || op == Operation.CondGoto)
                {
                    forceAdd = true;
                    controlPoints.Add(lin.Destination as LabelValue);
                }
            }
            return controlPoints;
        }

        public List<IBaseBlock> Build(List<LinearRepresentation> lst)
        {
            var controlPoints = FindControlPoints(lst);
            var bbuilder = new BaseBlockBuilder();

            foreach (var lin in lst)
            {
                if (controlPoints.Contains(lin.Label))
                    bbuilder.Flush();
                bbuilder.Add(lin);
            }
            return bbuilder.GetBlocks();
        }

    }
}
