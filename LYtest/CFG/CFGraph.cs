using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.BaseBlocks;

namespace LYtest.CFG
{
    public class CFGraph
    {
        public CFGNode root;
        public int num_of_vertexes => Blocks.Count;
        public readonly List<IBaseBlock> Blocks;

        public CFGraph(CFGNode root, List<IBaseBlock> blocks)
        {
            this.root = root;
            Blocks = blocks;
        }
    }
}
