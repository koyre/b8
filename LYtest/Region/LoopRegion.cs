using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.BaseBlocks;

namespace LYtest.Region
{
    class LoopRegion : Region
    {
        List<Region> regions;
        BodyRegion bodyReg;

        public List<Region> Regions { get { return regions; } }
        public BodyRegion BodyReg { get { return bodyReg; } }

        public LoopRegion(BaseBlock header, List<BaseBlock> blocks, List<Region> regions) : base (header, blocks)
        {
            this.regions = regions;
        }
    }
}
}
