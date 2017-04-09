
using LYtest.LinearRepr.Values;

namespace LYtest.LinearRepr
{
    public class ThreeAddressCode: IThreeAddressCode
    {
        public Operation Operation { get; set; }
        public IValue LeftOperand { get; set; }
        public IValue RightOperand { get; set; }
        public StringValue Destination { get; set; }
        public LabelValue Label { get; set; }
    }
}
