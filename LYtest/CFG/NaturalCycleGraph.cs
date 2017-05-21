using System.Collections.Generic;
using System.Linq;

namespace LYtest.CFG
{
    public class NaturalCycleGraph
    {
        private CFGraph cfGraph;

        public NaturalCycleGraph(CFGraph cfg)
        {
            this.cfGraph = cfg;
        }
        
        private List<CFGNode> visitedNodes = new List<CFGNode>();

        public List<int> findBetween(int from, int to)
        {
            return findBetween(cfGraph.GetVertices().ToList()[from], 
                               cfGraph.GetVertices().ToList()[to]).ToList()
                .Select(node => cfGraph.GetVertices().ToList().IndexOf(node)).ToList();
        }

        public List<CFGNode> findBetween(CFGNode from, CFGNode to)
        {
            visitedNodes.Clear();
            visitedNodes.Add(to);

            backDFS(from);

            return visitedNodes;
        }


        private void backDFS(CFGNode currentNode)
        {
            if (!visitedNodes.Contains(currentNode))
                visitedNodes.Add(currentNode);

            getNodes(currentNode).ToList()
                .Where(node => !visitedNodes.Contains(node)).ToList()
                .ForEach(node => backDFS(node));
        }

        private IEnumerable<CFGNode> getNodes(CFGNode node)
        {
            return cfGraph.graph.Edges
                .Where(edge => edge.Target == node)
                .Select(edge => edge.Source);
        }
    }
}
