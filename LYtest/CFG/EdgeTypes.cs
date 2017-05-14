using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using System.Text;
using System.Threading.Tasks;

namespace LYtest.CFG
{
    public enum EdgeType
    {
        Coming = 1,
        Retreating = 2,
        Cross = 3,
        Backward = 4
    }

    public class EdgeTypes : Dictionary<Edge<CFGNode>, EdgeType>
    {
        public override string ToString()
        {
            return string.Join("\n", this.Select(ed => $"[{ed.Key.Source.ToString()} -> {ed.Key.Target.ToString()}]: {ed.Value}"));
        }
    }
}
