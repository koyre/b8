using LYtest.LinearRepr.Values.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYtest.LinearRepr.Values
{
    public abstract class StringValue: BaseValue<String>
    {
        public StringValue(String s): base(s)
        {
            Value = s;
        }

        public String Value { get; set; }

    }
}
