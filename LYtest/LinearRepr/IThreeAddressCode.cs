
using LYtest.LinearRepr.Values;

namespace LYtest.LinearRepr
{
    public interface IThreeAddressCode
    {
        Operation Operation { get; set; }
        IValue LeftOperand { get; set; }
        IValue RightOperand { get; set; }
        StringValue Destination { get; set; }
        LabelValue Label { get; set; }
    }
}
