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
            // Get all nodes of cfg
            List<CFGNode> allNodes = cfg.GetVertices().ToList();
            // Each node in cfg is leaf region
            foreach (var node in cfg.GetVertices())
            {
                regions.Add(new LeafRegion(node,NextName()));
            }
            var nc = cfg.getNaturalCyclesForBackwardEdges();
            // All edges in cfg
            List<Edge<CFGNode>> edges = cfg.EdgeTypes.Select(e => e.Key).ToList();
            // Nodes which are headers of natural cycles
            HashSet<CFGNode> cyclesHeaders = new HashSet<CFGNode>(nc.Select(c => c[0]));
            // Headers of cycles. These cycles are added to list of regions
            HashSet <CFGNode> addedCyclesHeaders = new HashSet<CFGNode>();
            while (nc.Count > 0)
            {
                // List of cycles we can add into regions (there are no nonadded cycles inside)
                List<List<CFGNode>> cyclesToAdd = nc.FindAll(c => c.Skip(1).All(node =>
                {
                    // If node is header of cycle then it should be added in list of regions
                    return cyclesHeaders.Contains(node) ? addedCyclesHeaders.Contains(node) : true ;
                }));
                foreach (var cycle in cyclesToAdd)
                {
                    var nodes = new HashSet<CFGNode>(cycle);
                    AddCycle(cycle, edges, nodes);
                    addedCyclesHeaders.Add(cycle[0]);
                }
                nc.RemoveAll(c => addedCyclesHeaders.Contains(c[0]));
            }
        }

        private void AddCycle(List<CFGNode> cycle, List<Edge<CFGNode>> edges, HashSet<CFGNode> nodes)
        {
            CFGNode header = cycle[0];
            List<CFGNode> bodyNodes = cycle.ToList();
            List<Edge<CFGNode>> allEdgesInCycle = edges.Where(e => { return nodes.Contains(e.Source); }).ToList();
            // Edges only for nodes into cycle
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
