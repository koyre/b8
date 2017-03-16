using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYtest.auxil
{
    public enum OperationType
    {
        Assign,
        Plus,
        Minus,
        Mult,
        Div
    }

    public interface IInstructionTerm
    {
    }

    public interface IValue : IInstructionTerm
    {

    }

    public class Number: IValue
    {

        public int n;

        public Number(int n) { this.n = n; }
    }

    public class Identificator: IValue
    {
        public String id;
        public Identificator(String id) { this.id = id; }
    }


    public class LinearNode
    {
        public OperationType opType;
        public IInstructionTerm destination;
        public IValue left;
        public IValue right;

        public LinearNode(OperationType opType,
                          IInstructionTerm destination,
                          IValue left,
                          IValue right)
        {
            this.opType = opType;
            this.destination = destination;
            this.left = left;
            this.right = right;
        }
    }

    public class LinearCode //: Visitors.IVsisitor
    {

    }

}
