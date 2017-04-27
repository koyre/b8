using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LYtest.CFG
{
    public class ImmutableLookup<T>
    {
        private readonly T _current;
        private readonly ImmutableLookup<T> _prev;

        protected ImmutableLookup(T val)
        {
            _current = val;
        }

        protected ImmutableLookup(ImmutableLookup<T> prev, T val)
        {
            _current = val;
            _prev = prev;
        }

        public virtual ImmutableLookup<T> Move(T val)
        {
            return new ImmutableLookup<T>(this, val);
        }
    }

    public class CFGLookup
    {
        private readonly CFGNode _root;
        private CFGNode _current;
        private readonly Stack<CFGNode> _path = new Stack<CFGNode>();

        public CFGNode Current => _current;
        public int PathLen => _path.Count;

        public CFGLookup(CFGNode root)
        {
            _root = root;
            _current = root;
        }

        private CFGNode Move(CFGNode next)
        {
            if (next == null)
                return null;

            _path.Push(_current);
            return _current = next;
        }

        public void Reset()
        {
            _current = _root;
            _path.Clear();
        }

        public bool MoveBack()
        {
            if (_path.Count == 0)
                return false;
            _current = _path.Pop();
            return true;
        }

        public bool MoveGoto()
        {
            return this.Move(_current.gotoNode) != null;
        }

        public bool MoveDirect()
        {
            return this.Move(_current.directChild) != null;
        }

        public bool CanMoveGoto()
        {
            return _current.gotoNode != null;
        }

        public bool CanMoveDirect()
        {
            return _current.directChild != null;
        }
    }
}
