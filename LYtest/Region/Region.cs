using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.CFG;

namespace LYtest.Region
{
    public class Region
    {
        public CFGNode Header { get; }
        public string Name { get; set; }

        public Region(CFGNode header)
        {
            Header = header;
        }

        public Region(CFGNode header, string name="R")
        {
            Header = header;
            Name = name;
        }

        public override string ToString()
        {
            return Header.ToString();
        }
    }
}
