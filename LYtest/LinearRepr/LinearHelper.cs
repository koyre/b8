using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using LYtest.LinearRepr.Values;

namespace LYtest.LinearRepr
{
    public static class LinearHelper
    {

        public static bool IsGoto(this IThreeAddressCode t)
        {
            return t.Operation == Operation.CondGoto || t.Operation == Operation.Goto;
        }

        public static LabelValue GotoDest(this IThreeAddressCode t)
        {
            if (!t.IsGoto())
                throw new Exception("Calling GotoDest on non-goto operator!");
            return t.Destination as LabelValue;
        }

        public static bool IsBinOp(this IThreeAddressCode t)
        {
            switch (t.Operation)
            {
                case Operation.NoOperation:
                case Operation.Assign:
                case Operation.Goto:
                case Operation.CondGoto:
                case Operation.Print:
                case Operation.Println:
                    return false;
                case Operation.Plus:
                case Operation.Minus:
                case Operation.Mult:
                case Operation.Div:
                case Operation.Less:
                case Operation.LessOrEquals:
                case Operation.Great:
                case Operation.GreatOrEquals:
                case Operation.And:
                case Operation.Or:
                case Operation.NotEqual:
                case Operation.Equals:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool IsBinOp(Operation op)
        {
            switch (op)
            {
                case Operation.NoOperation:
                case Operation.Assign:
                case Operation.Goto:
                case Operation.CondGoto:
                case Operation.Print:
                case Operation.Println:
                    return false;
                case Operation.Plus:
                case Operation.Minus:
                case Operation.Mult:
                case Operation.Div:
                case Operation.Less:
                case Operation.LessOrEquals:
                case Operation.Great:
                case Operation.GreatOrEquals:
                case Operation.And:
                case Operation.Or:
                case Operation.NotEqual:
                case Operation.Equals:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool isExpression(this IThreeAddressCode t)
        {
            return (t.Operation != Operation.CondGoto && t.Operation != Operation.Goto && t.Operation != Operation.NoOperation);
        }

        public static IdentificatorValue AsDefinition(this IThreeAddressCode t)
        {
            if (t.IsBinOp() || t.Operation == Operation.Assign)
            {
                var res = t.Destination as IdentificatorValue;
                if (res == null)
                    throw new Exception("Asserion error");
                return res;
            }
            return null;
        }

        public static Func<int, int, int> ExecBinOp(this IThreeAddressCode t)
        {
            var ops = new Dictionary<Operation, Func<int, int, int>>
            {
                [Operation.Minus] = (x,y) => x-y,
                [Operation.Mult] = (x,y) => x*y,
                [Operation.Div] = (x,y) => x/y,
                [Operation.Plus] = (x, y) => x + y,
                [Operation.Less] = (x,y) => Convert.ToInt32(x<y),
                [Operation.LessOrEquals] = (x,y) => Convert.ToInt32(x <=y),
                [Operation.Great] = (x,y) => Convert.ToInt32(x > y),
                [Operation.GreatOrEquals] = (x,y) => Convert.ToInt32(x >= y),
                [Operation.And] = (x,y) => x == 0 ? 0 : y,
                [Operation.Or] = (x,y) => x == 0 ? y : x,
                [Operation.NotEqual] = (x,y) => Convert.ToInt32(x !=y),
                [Operation.Equals] = (x,y) => Convert.ToInt32(x ==y),

            };
            return ops[t.Operation];
        }

    }
}
