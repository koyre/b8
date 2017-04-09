using System;
using System.Collections.Generic;
using System.ComponentModel;
using LYtest.Helpers;
using LYtest.LinearRepr.Values;

namespace LYtest.LinearRepr
{
    public class LinearRepresentation: ThreeAddressCode
    {
        private static readonly IUniqueIdGen<int> gen = new UniqueIntGen();

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
        {
            var label = new LabelValue($"%ulabel{gen.GetNext()}");
            Operation = operation;
            Destination = destination;
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
            Label = label;
        }

        public override string ToString()
        {
            string body;

            var signs = new Dictionary<Operation, string>
            {
                [Operation.Minus] = "-",
                [Operation.Mult] = "*",
                [Operation.Div] = "/",
                [Operation.Less] = "<",
                [Operation.LessOrEquals] = "<=",
                [Operation.Great] = ">",
                [Operation.GreatOrEquals] = ">=",
                [Operation.And] = "&&",
                [Operation.Or] = "||",
                [Operation.NotEqual] = "!=",
                [Operation.Plus] = "+",
                [Operation.Equals] = "==",

            };

            if (signs.ContainsKey(Operation))
            {
                body = $"{Destination} := {LeftOperand} {signs[Operation]} {RightOperand}";
            }
            else
            {
                switch (this.Operation)
                {
                    case Operation.NoOperation:
                        body = "NOP";
                        break;
                    case Operation.Assign:
                        body = $"{Destination} := {LeftOperand}";
                        break;
                    case Operation.Goto:
                        body = $"GOTO {Destination}";
                        break;
                    case Operation.CondGoto:
                        body = $"IF {LeftOperand} THEN GOTO {Destination}";
                        break;
                    case Operation.Print:
                        body = $"print {LeftOperand}";
                        break;
                    case Operation.Println:
                        body = $"printLn {LeftOperand}";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return $"{this.Label.Value}: {body}";
        }
    }
}
