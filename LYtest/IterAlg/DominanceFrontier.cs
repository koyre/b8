using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.BaseBlocks;
using LYtest.CFG;

namespace LYtest
{
    public class DominanceFrontier
    {
        public Dictionary<IBaseBlock, HashSet<IBaseBlock>> DF = new Dictionary<IBaseBlock, HashSet<IBaseBlock>>();
        public Dictionary<IBaseBlock, HashSet<IBaseBlock>> IDF = new Dictionary<IBaseBlock, HashSet<IBaseBlock>>();
        private List<IBaseBlock> blocks;

        public DominanceFrontier(List<IBaseBlock> blocks)
        {
            this.blocks = blocks;
            for (int i = 0; i < blocks.Count; i++)
            {
                DF[blocks[i]] = new HashSet<IBaseBlock>();
            }

            MakeDF();
        }

        private void MakeDF()
        {
            var cfg = ListBlocksToCFG.Build(blocks);
            var start = cfg.GetRoot();

            Queue<CFGNode> queue = new Queue<CFGNode>();
            queue.Enqueue(start);
            var visited = new HashSet<CFGNode>();

            while (queue.Count > 0)
            {
                var cur_node = queue.Dequeue();
                if (visited.Contains(cur_node))
                {
                    continue;
                }
                DF[cur_node.Value].Add(cur_node.Value);
                visited.Add(cur_node);

                var childs = new List<CFGNode>(new CFGNode[] { cur_node.directChild, cur_node.gotoNode });
                foreach (var cur_child in childs)
                {
                    if(cur_child == null)
                    {
                        continue;
                    }
                    queue.Enqueue(cur_child);

                    if (cur_child.directChild != null)
                    {
                        DF[cur_node.Value].Add(cur_child.directChild.Value);
                    }

                    if(cur_child.gotoNode != null)
                    {
                        DF[cur_node.Value].Add(cur_child.gotoNode.Value);
                    }
                }
            }
        }

        //
        public HashSet<IBaseBlock> ComputeIDF(List<IBaseBlock> blocks)
        {
            HashSet<IBaseBlock> IDF = new HashSet<IBaseBlock>();
            HashSet<IBaseBlock> prevIDF = new HashSet<IBaseBlock>();
            HashSet<IBaseBlock> DF_S = new HashSet<IBaseBlock>();
            HashSet<IBaseBlock> S = new HashSet<IBaseBlock>();
            bool check = true;

            foreach (var block in blocks)
            {
                S.Add(block);
                DF_S.UnionWith(DF[block]);
            }

            IDF = new HashSet<IBaseBlock>(DF_S);
            prevIDF = new HashSet<IBaseBlock>(IDF);


            while (check)
            {
                DF_S = new HashSet<IBaseBlock>();
                var tempS = new HashSet<IBaseBlock>(S);
                tempS.UnionWith(IDF);
                foreach (var nameBlock in tempS)
                    DF_S.UnionWith(DF[nameBlock]);

                IDF = new HashSet<IBaseBlock>(DF_S);

                if (IDF.SetEquals(prevIDF))
                    check = false;

                prevIDF = new HashSet<IBaseBlock>(IDF);
            }

            return IDF;
        }

        public HashSet<IBaseBlock> ComputeIDF(IBaseBlock block)
        {
            List<IBaseBlock> t = new List<IBaseBlock>();
            t.Add(block);
            return ComputeIDF(t);
        }

        public Dictionary<IBaseBlock, HashSet<IBaseBlock>> ComputeIDF_ForEachBlock(List<IBaseBlock> blocks)
        {
            Dictionary<IBaseBlock, HashSet<IBaseBlock>> IDF = new Dictionary<IBaseBlock, HashSet<IBaseBlock>>();
            foreach (var block in blocks)
            {
                IDF[block] = ComputeIDF(block);
            }
            return IDF;
        }

        public override string ToString()
        {
            string result = "";

            foreach (var block in DF)
            {

                result += "DF(" + block.Key + ")= {" + string.Join(", ", block.Value) + "}";
                result += "\n";
            }

            result += "\n";

            return result;
        }
    }
}
