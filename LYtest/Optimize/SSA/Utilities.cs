using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.CFG;
using LYtest.BaseBlocks;
using LYtest.LinearRepr;
using LYtest.LinearRepr.Values;
using LYtest.ActiveVars;

namespace LYtest.Optimize.SSA
{
    static class Utilities
    {
        public static bool IsPhiAssignment(IThreeAddressCode line)
        {
            return IsPhiIdentificator(line.LeftOperand as IdentificatorValue) 
                    && line.Operation == Operation.Assign;
        }

        public static bool IsPhiFunction(IThreeAddressCode line)
        {
            return line.Operation == Operation.Phi;
        }

        public static bool IsPhiIdentificator(IdentificatorValue ident)
        {
            return ident != null
                && ident.Value.Count() >= 3
                && ident.Value.Substring(0, 3) == "phi";
        }

    }
}
