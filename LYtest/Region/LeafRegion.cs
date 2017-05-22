using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.BaseBlocks;

namespace LYtest.Region
{
    class LeafRegion : Region
    {
        // LeafRegion has just one block (= header)
        public LeafRegion(BaseBlock header) : base(header, new List<BaseBlock> { header })
        {
            
        }

    }
}
