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
    public interface IBaseBlock
    {
        bool InsertAfter(IThreeAddressCode after, IThreeAddressCode newElem);
        void Append(IThreeAddressCode newElem);
        void AppendFirst(IThreeAddressCode newElem);
        bool Remove(IThreeAddressCode elem);
        IEnumerable<IThreeAddressCode> Enumerate();
        string ToString();
    }
    
    public class BaseBlock : IBaseBlock
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
            _elems.AddLast(newElem);
        }

        public void AppendFirst(IThreeAddressCode newElem)
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

        public override string ToString()
        {
            string str = "";
            foreach (var line in _elems)
            {
                str += line.ToString();
                str += '\n';
            }
            return str;
        }

        public override bool Equals(object obj)
        {
            var second = obj as BaseBlock;
            if (!(this._elems.Count == second._elems.Count))
            {
                return false;
            }

            var this_f = _elems.First;
            var sec_f = second._elems.First;
            while (this_f != null)
            {
                if (this_f.Value.Label != sec_f.Value.Label)
                {
                    return false;
                }
                this_f = this_f.Next;
                sec_f = sec_f.Next;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
