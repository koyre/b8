using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LYtest.BaseBlocks;
using LYtest.LinearRepr;
using LYtest.LinearRepr.Values;

namespace LYtest.CFG
{
    public static class ListBlocksToCFG
    {
        public static CFGraph Build(List<IBaseBlock> blocks)
        {
            CFGNode root = new CFGNode(blocks.First());
            var currentNode = root;

            foreach (var block in blocks.Skip(1))
            {
                var directChild = new CFGNode(block);
                currentNode.SetDirectChild(directChild);

                var lastOp = currentNode.Value.Enumerate().Last();
                if (lastOp.IsGoto())
                {
                    var gotoBlock = blocks.First(b => b.Enumerate().First().Label.Equals(lastOp.Destination));
                    var gotoChild = new CFGNode(gotoBlock);
                    currentNode.SetGotoChild(gotoChild);
                }
                currentNode = directChild;
            }

            var graph = new CFGraph(blocks);
            return graph;
        }
    }
}
