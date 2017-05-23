using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.Region;
using QuickGraph;
using LYtest.CFG;

namespace LYtest.Region
{
    public class RegionSequence
    {
        List<Region> regions;
        public List<Region> Regions { get { return regions; } }
        // For numeration of regions
        string name = "R";
        int k = 0;

        public RegionSequence(CFGraph cfg)
        {
            regions = new List<Region>();
            List<CFGNode> allNodes = cfg.GetVertices().ToList();
            foreach (var node in cfg.GetVertices())
            {
                regions.Add(new LeafRegion(node,NextName()));
             }
            var nc = cfg.getNaturalCyclesForBackwardEdges();
            List<Edge<CFGNode>> edges = cfg.EdgeTypes.Select(e => e.Key).ToList();
            HashSet<CFGNode> cyclesHeaders = new HashSet<CFGNode>(nc.Select(c => c[0]));

            HashSet <CFGNode> addedCyclesHeaders = new HashSet<CFGNode>();

            // Find most inner cycles and create corresponding regions
            foreach (var cycle in nc)
            {
                // Nodes of current cycle
                var nodes = new HashSet<CFGNode>(cycle);
                // Find all inner cycles in current (includes itself)
                List<List<CFGNode>> innerCycles = nc.FindAll(c =>
                {
                    return (new HashSet<CFGNode>(c)).IsSubsetOf(nodes);
                });
                // Remove current node
                innerCycles.Remove(cycle);
                // If there are no inner cycles then add BodyRegion and LoopRegion
                if (innerCycles.Count == 0)
                {
                    AddRightCycle(cycle, edges, nodes);
                    addedCyclesHeaders.Add(cycle[0]);
                }
            }
            nc.RemoveAll(c => addedCyclesHeaders.Contains(c[0]));
            while (nc.Count > 0)
            {
                List<List<CFGNode>> cyclesToAdd = nc.FindAll(c => c.Skip(1).All(node =>
                {
                    bool b = false;
                    if (cyclesHeaders.Contains(node))
                        b = addedCyclesHeaders.Contains(node);
                    else
                        b = true;
                    return b;
                }));
                foreach (var cycle in cyclesToAdd)
                {
                    var nodes = new HashSet<CFGNode>(cycle);
                    AddRightCycle(cycle, edges, nodes);
                    addedCyclesHeaders.Add(cycle[0]);
                }
                nc.RemoveAll(c => addedCyclesHeaders.Contains(c[0]));
            }
        }

        private void AddRightCycle(List<CFGNode> cycle, List<Edge<CFGNode>> edges, HashSet<CFGNode> nodes)
        {
            // Region is header and other nodes
            CFGNode header = cycle[0];
            List<CFGNode> bodyNodes = cycle.Skip(1).ToList();
            // Edges only for nodes into cycle
            List<Edge<CFGNode>> allEdgesInCycle = edges.Where(e => { return nodes.Contains(e.Source); }).ToList();
            List<Edge<CFGNode>> bodyEdgesInCycle = allEdgesInCycle.Where(e => { return e.Target != header; }).ToList();
            BodyRegion br = new BodyRegion(header, bodyNodes, bodyEdgesInCycle, NextName());
            LoopRegion lr = new LoopRegion(header, bodyNodes, allEdgesInCycle, br, NextName());
            regions.Add(br);
            regions.Add(lr);
        }

        private string NextName()
        {
            string n = name + k.ToString();
            ++k;
            return n;
        }

        public string PrintRegions()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var reg in regions)
            {
                sb.Append(reg.Name + "\n");
                if (reg is NonLeafRegion)
                {
                    sb.Append(reg.Header + "\n");
                    sb.Append(reg.ToString());
                }
                else
                {
                    sb.Append(reg.Header+"\n");
                }
            }
            return sb.ToString();
        }
    }
}
