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
    public class SsaCopyPropagation
    {
        private CFGraph graph;
        private HashSet<IThreeAddressCode> worklist;

        public CFGraph OptimizedSsaGraph
        {
            get => graph;
        }

        public SsaCopyPropagation(CFGraph ssaGraph)
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

                //Если данное выражение - фи функция и в ней присваиваются одинаковые переменные
                if (Utilities.IsPhiAssignment(s))
                {
                    List<IThreeAddressCode> phis = new List<IThreeAddressCode>();
                    phis = worklist.Select(x => x)
                        .Where(x => Utilities.IsPhiFunction(x) && x.Destination == s.LeftOperand)
                        .ToList();
                    bool AllPhiValuesAreEqualVariables = true;
                    var firstPhiValue = phis.First().LeftOperand;
                    foreach (var phi in phis)
                    {
                        if (!phi.LeftOperand.Equals(firstPhiValue) || !(phi.LeftOperand is IdentificatorValue))
                            AllPhiValuesAreEqualVariables = false;
                    }
                    //Если ее фи-функция состоит из одинаковых констант, то заменяем ее на эту константу
                    if (AllPhiValuesAreEqualVariables)
                    {
                        ReplaceVar(graph, s.Destination as IdentificatorValue, firstPhiValue as IdentificatorValue);
                        ChangePhiFuncToVariable(graph, s.LeftOperand as IdentificatorValue, firstPhiValue as IdentificatorValue);
                    }
                }

                //Если встретили присваивание переменной
                if (s.Operation == Operation.Assign && s.LeftOperand is IdentificatorValue && !Utilities.IsPhiAssignment(s))
                    ReplaceVar(graph, s.Destination as IdentificatorValue, s.LeftOperand as IdentificatorValue);

                worklist.Remove(s);
            }
            return graph;
        }

        private void ChangePhiFuncToVariable(CFGraph graph, IValue phiName, IValue newVariableName)
        {
            foreach (var block in graph.Blocks)
            {
                foreach (var instr in block.Enumerate())
                {
                    if (instr.Operation == Operation.Assign && instr.LeftOperand == phiName)
                    {
                        instr.LeftOperand = newVariableName;
                    }
                }
                var phiInstrs = block.Enumerate().Select(x => x).Where(instr => instr.Operation == Operation.Phi && instr.Destination == phiName);
                for (int i = phiInstrs.Count() - 1; i >= 0; --i)
                    block.Remove(phiInstrs.ElementAt(i));
            }
        }

        private void ReplaceVar(CFGraph graph, IdentificatorValue oldVar, IdentificatorValue newVar)
        {
            foreach (var block in graph.graph.Vertices)
            {
                foreach (var instr in block.Value.Enumerate())
                {
                    if (LinearHelper.AsDefinition(instr) == null)
                        continue;
                    bool matched = false;
                    if (instr.LeftOperand.Equals(oldVar))
                    {
                        instr.LeftOperand = newVar;
                        matched = true;
                    }
                    if (instr.RightOperand != null && instr.RightOperand.Equals(oldVar))
                    {
                        instr.RightOperand = newVar;
                        matched = true;
                    }
                    if (matched)
                        worklist.Add(instr);
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

    }
}
