using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.LinearRepr.Values;

namespace LYtest.Optimize.ConstantPropagation
{

    public enum VariableValueType
    {
        UNDEF,
        NAC,
        CONSTANT
    };

    public struct VariableValue
    {
        public VariableValueType type;
        public int value;
    }

    public class VariableConstantMap
    {

        public Dictionary<IdentificatorValue, VariableValue> variableTable { get; }

        public VariableConstantMap()
        {
            variableTable = new Dictionary<IdentificatorValue, VariableValue>();
        }

        public VariableConstantMap(Dictionary<IdentificatorValue, VariableValue> newDictionary)
        {
            variableTable = newDictionary;
        }

    }
}
