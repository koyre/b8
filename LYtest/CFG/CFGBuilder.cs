using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LYtest.BaseBlocks;

namespace LYtest.CFG
{   
    class CFGBuilder
    {
        private List<IBaseBlock>_blocks;
        private LinkedList<CFGNode> _nodes = new LinkedList<CFGNode>();

        public CFGBuilder(List<IBaseBlock> blocks)
        {
            this._blocks = blocks;
        }

        public void Build()
        {
            var firstNode = new CFGNode();

            for (int i = 0; i < this._blocks.Count; i++)
            {
                
            }
        }

     }
}
