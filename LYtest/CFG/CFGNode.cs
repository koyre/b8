using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LYtest.BaseBlocks;

namespace LYtest.CFG
{
    public class CFGNode
    {
        public readonly List<CFGNode> ParentsNodes = new List<CFGNode>();
        public CFGNode directChild; // Will return direct child node (nut null)
        public CFGNode gotoNode; // Will return node following goto operator (may be null)

        public IBaseBlock Value { get; }


        public CFGNode(IBaseBlock val)
        {
            Value = val;
        }

        private void AcceptChild(CFGNode child)
        {
            if (child.ParentsNodes.Contains(this))
                return;
            child.ParentsNodes.Add(this);
        }

        public void SetDirectChild(CFGNode child)
        {
            directChild = child;
            AcceptChild(child);
        }
        public void SetGotoChild(CFGNode child)
        {
            gotoNode = child;
            AcceptChild(child);
        }

    }
}
