
using System.Collections.Generic;

namespace LYtest.LinearRepr.Values.Base
{
    public abstract class BaseValue<T>: IValue
    {
        protected BaseValue(T value)
        {
            Value = value;
        }

        public T Value { get; }

        object IValue.Value => Value;

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals(object obj)
        {
            var baseValue = obj as BaseValue<T>;
            return baseValue != null && Value.Equals(baseValue.Value);
        }

        protected bool Equals(BaseValue<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Value);
        }
    }
}
