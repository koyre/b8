using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LYtest.BaseBlocks;

namespace LYtest.CFG
{
    class CFGNode
    {
        public CFGNode leftNode; // Will return left-hand side node 
        public CFGNode rightNode; // Will return right-hand side node

        public IBaseBlock nodeBlock; // Will return the block which was assigned for current node

        public CFGNode()
        {

        }

    }
}
