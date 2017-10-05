using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.LinearRepr.Values;

namespace LYtest.SymbolicAnalysis
{
    public enum VariableValueType
    {
        UNDEF,
        NAA,
        AFFINE
    };

    public struct VariableValue
    {
        public VariableValueType type;
        public AffineExpr value;
    }

    public struct AffineExpr
    {
        public List<int> constants;
        public List<IdentificatorValue> variables;
        public int value;
    }

    public class SymbolicMap
    {
        public Dictionary<IdentificatorValue, VariableValue> variableTable { get; }

        public SymbolicMap()
        {
            variableTable = new Dictionary<IdentificatorValue, VariableValue>();
        }

        public SymbolicMap(Dictionary<IdentificatorValue, VariableValue> newDictionary)
        {
            variableTable = newDictionary;
        }        
    }    
}
