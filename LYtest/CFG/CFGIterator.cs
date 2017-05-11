using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.BaseBlocks;

namespace LYtest.CFG
{
    public static class CFGNodeSet
    {
        public static HashSet<CFGNode> GetNodes(CFGraph g)
        {
            var it = new CFGIterator(g);
            return it.Visited;
        }
    }

    public class CFGIterator
    {
        public HashSet<CFGNode> Visited = new HashSet<CFGNode>();
        public CFGIterator(CFGraph g)
        {
            Visit(g.GetRoot());
        }
        
        private void Visit(CFGNode bl)
        {
            if (bl == null) return;

            Visit(bl.directChild);

            if (Visited.Contains(bl)) return;
            Visit(bl.gotoNode);

            Visited.Add(bl);
        }
    }
}
