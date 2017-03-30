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
        bool InsertAfter(IThreeAddressCode after, IThreeAddressCode newElem);
        void Append(IThreeAddressCode newElem);
        bool Remove(IThreeAddressCode elem);
        IEnumerable<IThreeAddressCode> Enumerate();
    }
    
    class BaseBlock : IBaseBlock
    {
        private readonly LinkedList<IThreeAddressCode> _elems = new LinkedList<IThreeAddressCode>();

        public bool InsertAfter(IThreeAddressCode after, IThreeAddressCode newElem)
        {
            var node = _elems.Find(after);
            if (node == null)
                return false;

            _elems.AddAfter(node, newElem);
            return true;
        }

        public void Append(IThreeAddressCode newElem)
        {
            _elems.AddFirst(newElem);
        }

        public bool Remove(IThreeAddressCode elem)
        {
            return _elems.Remove(elem);
        }

        public IEnumerable<IThreeAddressCode> Enumerate()
        {
            return _elems;
        }
    }
}
