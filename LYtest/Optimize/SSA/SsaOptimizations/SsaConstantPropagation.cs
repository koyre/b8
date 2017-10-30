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

namespace LYtest.Optimize.SSA.SsaOptimizations
{
    public class SsaConstantPropagation
    {
        private CFGraph graph;
        private HashSet<IThreeAddressCode> worklist;
        private List<CFGNode> visitedNodes = new List<CFGNode>();

        public CFGraph OptimizedSsaGraph
        {
            get => graph;
        }

        public SsaConstantPropagation(CFGraph ssaGraph)
        {
            graph = ssaGraph;
            worklist = new HashSet<IThreeAddressCode>();
            GetStatements(graph.GetRoot());
        }

        public CFGraph Launch()
        {
            while (worklist.Count != 0)
            {
                var s = worklist.First();

                //Свертка контант
                if (LinearHelper.IsBinOp(s) && s.LeftOperand is NumericValue && s.RightOperand is NumericValue)
                {
                    NumericValue newConstant = CalculateConstant(s.Operation, s.LeftOperand as NumericValue, s.RightOperand as NumericValue);
                    s.Operation = Operation.Assign;
                    s.LeftOperand = newConstant;
                }

                //Если данное выражение - фи функция
                if (Utilities.IsPhiAssignment(s))
                {
                    List<IThreeAddressCode> phis = new List<IThreeAddressCode>();
                    phis = worklist.Select(x => x)
                        .Where(x => Utilities.IsPhiFunction(x) && x.Destination == s.LeftOperand)
                        .ToList();
                    bool AllPhiValuesAreConstants = true;
                    var firstPhiValue = phis.First().LeftOperand;
                    foreach (var phi in phis)
                    {
                        if (phi.LeftOperand != firstPhiValue || !(phi.LeftOperand is NumericValue))
                            AllPhiValuesAreConstants = false;
                    }
                    //Если ее фи-функция состоит из одинаковых констант, то заменяем ее на эту константу
                    if (AllPhiValuesAreConstants)
                    {
                        ChangePhiFuncToConstant(graph, s.LeftOperand, s.Destination, firstPhiValue);
                    }
                }

                

                //Если присваиваем константу
                else if (s.Operation == Operation.Assign && s.LeftOperand is NumericValue)
                {
                    var variable = s.Destination;
                    var constant = s.LeftOperand;
                    foreach (var block in graph.graph.Vertices)
                    {
                        foreach (var instr in block.Value.Enumerate())
                        {
                            if (s.Destination.Equals(instr.LeftOperand))
                            {
                                instr.LeftOperand = constant;
                                worklist.Add(instr);
                            }
                            if (variable.Equals(instr.RightOperand))
                            {
                                instr.RightOperand = constant;
                                worklist.Add(instr);
                            }
                        }
                    }
                    RemoveInstructionFromGraph(graph, s);
                }
                
                worklist.Remove(s);
            }
            return graph;
        }

        private void ChangePhiFuncToConstant(CFGraph graph, IValue phiName, IValue variableName, IValue constantValue)
        {
            foreach (var block in graph.Blocks)
            {
                foreach (var instr in block.Enumerate())
                {
                    if (instr.Operation == Operation.Assign && instr.LeftOperand == phiName)
                    {
                        instr.LeftOperand = constantValue;
                    }
                }
                var phiInstrs = block.Enumerate().Select(x => x).Where(instr => instr.Operation == Operation.Phi && instr.Destination == phiName);
                for (int i = phiInstrs.Count() - 1; i >= 0; --i)
                    block.Remove(phiInstrs.ElementAt(i));
            }
        }

        private void ReplaceInstructionInGraph(CFGraph graph, IThreeAddressCode oldInstr, IThreeAddressCode newInstr)
        {
            foreach (var block in graph.Blocks)
            {
                foreach (var instr in block.Enumerate())
                {
                    if (instr == oldInstr)
                    {
                        block.InsertAfter(oldInstr, newInstr);
                        block.Remove(oldInstr);
                    }
                }
            }
        }

        private void RemoveInstructionFromGraph(CFGraph graph, IThreeAddressCode instrToDelete)
        {
            foreach (var block in graph.Blocks)
            {
                block.Remove(instrToDelete);
            }
        }

        private void GetStatements(CFGNode currentNode)
        {
            foreach (var vert in graph.graph.Vertices)
            {
                foreach (var s in vert.Value.Enumerate())
                {
                    worklist.Add(s);
                }
            }
        }

        private NumericValue CalculateConstant(Operation op, NumericValue x, NumericValue y)
        {
            int _x = x.Value;
            int _y = y.Value;
            int res = 0;
            switch (op)
            {
                case Operation.Plus:
                    res = _x + _y;
                    break;
                case Operation.Minus:
                    res = _x - _y;
                    break;
                case Operation.Mult:
                    res = _x * _y;
                    break;
                case Operation.Div:
                    res = _x / _y;
                    break;
                default:
                    return new NumericValue(0);
            }
            return new NumericValue(res);
        }

    }
}
