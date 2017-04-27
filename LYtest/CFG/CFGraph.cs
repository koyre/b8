using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYtest.CFG
{
    public class CFGraph
    {
        public CFGNode root;
        public int num_of_vertexes;

        public CFGraph(CFGNode root, int num_of_vertexes)
        {
            this.root = root;
            this.num_of_vertexes = num_of_vertexes;
        }
    }
}
