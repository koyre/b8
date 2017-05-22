using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.BaseBlocks;

namespace LYtest.Region
{
    class Region
    {
        // Each region has at least 1 block inside
        List<BaseBlock> blocks;

        public BaseBlock Header { get; }
        public List<BaseBlock> Blocks { get { return blocks; } }

        public Region(BaseBlock header, List<BaseBlock> blocks)
        {
            Header = header;
            this.blocks = blocks;
        }
    }
}
