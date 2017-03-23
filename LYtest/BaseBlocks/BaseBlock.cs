using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.LinearRepr;
using LYtest.LinearRepr.Values;

namespace LYtest.BaseBlocks
{
    interface IBaseBlock
    {
        IThreeAddressCode InsertAfter(LabelValue after, IThreeAddressCode newElem);
        IThreeAddressCode Remove(LabelValue elem);
        IEnumerable<IThreeAddressCode> Enumerate();
    }

    class BaseBlock : IBaseBlock
    {
        public IThreeAddressCode InsertAfter(LabelValue after, IThreeAddressCode newElem)
        {
            throw new NotImplementedException();
        }

        public IThreeAddressCode Remove(LabelValue elem)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IThreeAddressCode> Enumerate()
        {
            throw new NotImplementedException();
        }
    }
}
