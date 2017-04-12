using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYtest.Helpers
{
    interface IUniqueIdGen<T>
    {
        T GetNext();
    }

    class UniqueIntGen : IUniqueIdGen<int>
    {
        private int _id = 0;

        public int GetNext()
        {
            return _id++;
        }
    }
    
}
