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
        Cross = 3
    }

    public class EdgeTypes : Dictionary<Edge<CFGNode>, EdgeType>
    {
        public override string ToString()
        {
            return string.Join("\n\n", this.Select(ed => $"[{ed.Key.ToString()} -> {ed.Value.ToString()}]: {ed.Value}"));
        }
    }
}
