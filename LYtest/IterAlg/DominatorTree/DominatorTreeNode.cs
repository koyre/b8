using LYtest.BaseBlocks;
using LYtest.CFG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYtest.DominatorTree
{
    public class DominatorTreeNode
    {
        public DominatorTreeNode ParentNode { get; set; }
        public List<DominatorTreeNode> ChildrenNodes { get; set; }
        public CFGNode CFGNode { get; set; }
        
        public DominatorTreeNode(CFGNode cfgNode)
        {
            CFGNode = cfgNode;
            ChildrenNodes = new List<DominatorTreeNode>();
        }

        public DominatorTreeNode(CFGNode cfgNode, DominatorTreeNode parent, IEnumerable<DominatorTreeNode> children)
        {
            CFGNode = cfgNode;
            ParentNode = parent;
            ChildrenNodes = new List<DominatorTreeNode>(children) ?? new List<DominatorTreeNode>();
        }

        public void AddChild(DominatorTreeNode child)
        {
            if (!ChildrenNodes.Contains(child))
                ChildrenNodes.Add(child);
        }
    }
}