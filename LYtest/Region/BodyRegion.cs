using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.BaseBlocks;

namespace LYtest.Region
{
    class BodyRegion : Region
    {
        List<Region> regions;
        public List<Region> Regions { get { return regions; } }

        public BodyRegion(BaseBlock header, List<BaseBlock> blocks, List<Region> regions) : base(header, blocks)
        {
            this.regions = regions;
        }
    }
}
