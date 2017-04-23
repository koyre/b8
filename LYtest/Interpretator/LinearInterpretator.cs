using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.LinearRepr;
using LYtest.LinearRepr.Values;

namespace LYtest.Interpretator
{
    class InterpretEnv
    {
        private Dictionary<LabelValue, int> _codeAdress;
        private List<IThreeAddressCode> _code;
        private Dictionary<String, int> variables;
        private int ip;
        private List<int> printBuf;

        public InterpretEnv(List<IThreeAddressCode> code)
        {
            Load(code);
        }

        public void Load(List<IThreeAddressCode> code)
        {
            _code = code;
            _codeAdress = new Dictionary<LabelValue, int>();
            for (var i = 0; i < _code.Count; i++)
                _codeAdress.Add(_code[i].Label, i);
            variables = new Dictionary<String, int>();
            printBuf = new List<int>();
            ip = 0;
        }

        public bool Next()
        {
            var cur = _code[ip];
            if (cur.IsGoto())
            {
                if (cur.Operation == Operation.Goto || variables[(cur.LeftOperand as StringValue).Value] != 0)
                {
                    ip = _codeAdress[cur.GotoDest()];
                    return true;
                }
            }

            if (cur.IsBinOp())
            {
                var f = cur.ExecBinOp();

                var lhs = GetVarOrConst(cur.LeftOperand);
                var rhs = GetVarOrConst(cur.RightOperand);
                variables[cur.Destination.Value] = f(lhs, rhs);
            }

            if (cur.Operation == Operation.Print || cur.Operation == Operation.Println)
            {
                var name = cur.LeftOperand as StringValue;
                printBuf.Add(variables[name.Value]);
            }
            if (

                cur.Operation == Operation.Assign)
            {
                variables[cur.Destination.Value] = GetVarOrConst(cur.LeftOperand);
            }
            ip++;
            return (ip < _code.Count);
        }

        private int GetVarOrConst(IValue val)
        {
            var variab = val as StringValue;
            if (variab != null)
                return variables[variab.Value];
            return (val as NumericValue).Value;
        }
        public List<int> Run()
        {
            while (Next()) ;
            return printBuf;
        }
    }

    public static class LinearInterpretator
    {
        public static List<int> Run(IEnumerable<IThreeAddressCode> code)
        {
            var interp = new InterpretEnv(code.ToList());
            return interp.Run();
        }
    }
}
