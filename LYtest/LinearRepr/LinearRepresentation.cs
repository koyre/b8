using LYtest.LinearRepr.Values;

namespace LYtest.LinearRepr
{
    class LinearRepresentation: ThreeAddressCode
    {
        public LinearRepresentation(LabelValue label,
                                    Operation operation,
                                    StringValue destination = null,
                                    IValue leftOperand = null,
                                    IValue rightOperand = null)
        {
            Operation = operation;
            Destination = destination;
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
            Label = label;
        }
        public LinearRepresentation(Operation operation,
                                    StringValue destination = null,
                                    IValue leftOperand = null,
                                    IValue rightOperand = null)
            : this(null, operation, destination, leftOperand, rightOperand)
        { }
    }
}
