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
    class SsaVarsRenaming
    {
        private CFGraph cfGraph;

        private Dictionary<IdentificatorValue, Stack<int>> variableStacks;
        private Dictionary<IdentificatorValue, int> counters;

        public SsaVarsRenaming(CFGraph cfg)
        {
            this.cfGraph = cfg;
            variableStacks = new Dictionary<IdentificatorValue, Stack<int>>();
            counters = new Dictionary<IdentificatorValue, int>();
            var allVariables = GetAllVariables(cfGraph);
            foreach (var v in allVariables)
            {
                var stack = new Stack<int>();
                stack.Push(0);
                variableStacks.Add(v, stack);
                
                counters.Add(v, 0);
            }
        }

        private List<CFGNode> visitedNodes = new List<CFGNode>();

        public CFGraph Launch()
        {
            Rename(cfGraph.GetRoot());
            return cfGraph;
        }

        private void Rename(CFGNode currentNode)
        {
            //Если уже посетили этот блок, выходим
            if (visitedNodes.Contains(currentNode))
                return;

            foreach (var str in currentNode.Value.Enumerate())
            {
                //Если встретили фи-функцию, переименовываем ее левую часть
                if (Utilities.IsPhiAssignment(str))
                {
                    IdentificatorValue curVar = str.Destination as IdentificatorValue;
                    GenName(curVar);
                    int varCounter = variableStacks[curVar].Peek();
                    str.Destination = new IdentificatorValue(str.Destination.Value + varCounter.ToString());
                }
                //Переименовываем use переменные в правой части присваиваний
                if (!Utilities.IsPhiIdentificator(str.LeftOperand as IdentificatorValue) && str.Operation != Operation.Phi)
                { 
                    if (str.RightOperand is IdentificatorValue)
                    {
                        IdentificatorValue curVar = str.RightOperand as IdentificatorValue;
                        int varCounter = variableStacks[curVar].Peek();
                        str.RightOperand = new IdentificatorValue(str.RightOperand.Value + varCounter.ToString());
                    }
                    if (str.LeftOperand is IdentificatorValue)
                    {
                        IdentificatorValue curVar = str.LeftOperand as IdentificatorValue;
                        int varCounter = variableStacks[curVar].Peek();
                        str.LeftOperand = new IdentificatorValue(str.LeftOperand.Value + varCounter.ToString());
                    }
                    if (str.Destination is IdentificatorValue)
                    {
                        IdentificatorValue curVar = str.Destination as IdentificatorValue;
                        GenName(curVar);
                        int varCounter = variableStacks[curVar].Peek();
                        str.Destination = new IdentificatorValue(str.Destination.Value + varCounter.ToString());
                    }
                }
            }
            var children = new List<CFGNode>() { currentNode.directChild, currentNode.gotoNode };
            foreach (var child in children)
            {
                if (child == null)
                    continue;
                foreach (var s in child.Value.Enumerate())
                {
                    //Если встретили фи-функцию, переименовываем ее левую часть
                    if (Utilities.IsPhiAssignment(s))
                    {
                        //Ищем строку соответствующей фи-функции и переименовываем переменную в ее левой части
                        foreach (var line in child.Value.Enumerate()
                            .Select(str => str).Where(str => str.Operation == Operation.Phi && str.Destination == s.LeftOperand))
                        {
                            if (line.RightOperand == currentNode.Value.Enumerate().Last().Label)
                            {
                                IdentificatorValue curVar = line.LeftOperand as IdentificatorValue;
                                int varCounter = variableStacks[curVar].Peek();
                                line.LeftOperand = new IdentificatorValue(line.LeftOperand.Value + varCounter.ToString());
                            }
                        }
                    }
                }
            }

            if (!visitedNodes.Contains(currentNode))
                visitedNodes.Add(currentNode);

            foreach (var child in children)
            {
                if (child != null)
                    Rename(child);
            }

            //Очищаем стек от переменных данного блока
            foreach (var str in currentNode.Value.Enumerate())
            {
                if (str.LeftOperand is IdentificatorValue)
                {
                    IdentificatorValue curVar = str.LeftOperand as IdentificatorValue;
                    if (variableStacks.Keys.Contains(curVar))
                    variableStacks[curVar].Pop();
                }
            }

        }

        private void GenName(IdentificatorValue v)
        {
            if (variableStacks[v] == null)
                variableStacks[v] = new Stack<int>();
            var i = counters[v];
            variableStacks[v].Push(i);
            counters[v] = i + 1;
        }

        

        private HashSet<IdentificatorValue> GetAllVariables(CFGraph inputGraph)
        {
            HashSet<IdentificatorValue> variables = new HashSet<IdentificatorValue>();
            foreach (var block in inputGraph.Blocks)
            {
                foreach (var line in block.Enumerate())
                {
                    //if (LinearHelper.AsDefinition(line) != null && !Utilities.IsPhiIdentificator(line.LeftOperand.Value as IdentificatorValue))
                    //    variables.Add(line.Destination as IdentificatorValue);
                    if (LinearHelper.AsDefinition(line) != null && !Utilities.IsPhiIdentificator(line.LeftOperand.Value as IdentificatorValue))
                    {
                        if (line.LeftOperand is IdentificatorValue)
                            variables.Add(line.LeftOperand as IdentificatorValue);
                        if (line.RightOperand is IdentificatorValue)
                            variables.Add(line.RightOperand as IdentificatorValue);
                        if (line.Destination is IdentificatorValue)
                            variables.Add(line.Destination as IdentificatorValue);
                    }
                }
            }
            return variables;
        }

    }
}
