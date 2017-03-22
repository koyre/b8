using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramTree;

namespace LYtest.Visitors
{
    public interface IVisitor
    {
        void VisitIf(IfNode n);

        void VisitFor(ForNode n);

        void VisitAssign(AssignNode n);

        void VisitBlock(BlockNode n);

        void VisitProc(Procedure n);

        void VisitWhile(WhileNode n);

        void VisitConst(Const n);

        void VisitId(IdentNode n);

        void VisitBinOp(BinOp n);

        void VisitUnOp(UnOp n);
    }

}
