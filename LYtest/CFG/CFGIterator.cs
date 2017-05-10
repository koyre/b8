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
            Visit(g.root);
        }
        
        private void Visit(CFGNode g)
        {
            if (g == null) return;

            Visit(g.directChild);

            if (Visited.Contains(g)) return;
            Visit(g.gotoNode);

            Visited.Add(g);
        }
    }
}
