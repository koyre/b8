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
    public class SsaConstruction
    {

        CFGraph ssaForm;

        public SsaConstruction(CFGraph inputGraph)
        {
            variables = GetAllVariables(inputGraph);
            var phiGraph = InsertPhiFunctions(inputGraph);
            ssaForm = RenameVariables(phiGraph);
        }

        public CFGraph SsaForm => ssaForm;
        private int phiCounter = 0;

        private HashSet<IdentificatorValue> variables;

        /// <summary>
        /// Inserts phi functions for all variables in every block,
        /// which contains 2 predesessors
        /// </summary>
        /// <param name="inputGraph"></param>
        /// <returns></returns>
        private CFGraph InsertPhiFunctions(CFGraph inputGraph)
        {
            CFGraph ssaGraph = inputGraph;

            //HashSet<BaseBlock> blocksWithPhi = new HashSet<BaseBlock>();
            
            foreach (var variable in variables)
            {
                foreach (var node in inputGraph.graph.Vertices)
                { 
                    if (node.ParentsNodes.Count >= 2)
                    {
                        IValue phiLabel = new IdentificatorValue("phi" + phiCounter);
                        var newAssign = new LinearRepresentation(Operation.Assign, variable
                            , phiLabel, null);
                        node.Value.AppendFirst(newAssign);

                        foreach (var parentNode in node.ParentsNodes)
                        {
                            var phiFunc = new LinearRepresentation(Operation.Phi, phiLabel as StringValue
                                , variable, parentNode.Value.Enumerate().Last().Label);
                            node.Value.InsertAfter(newAssign, phiFunc);
                        }
                        phiCounter++;
                    }
                }
            }

            return ssaGraph;
        }

        /// <summary>
        /// Renaming variables in order to make single assignment 
        /// of every variable
        /// </summary>
        /// <param name="inputGraph"></param>
        /// <returns></returns>
        private CFGraph RenameVariables(CFGraph inputGraph)
        {
            CFGraph ssaGraph = inputGraph;

            SsaVarsRenaming renaming = new SsaVarsRenaming(ssaGraph);

            return renaming.Launch();
        }

        private HashSet<IdentificatorValue> GetAllVariables(CFGraph inputGraph)
        {
            HashSet<IdentificatorValue> variables = new HashSet<IdentificatorValue>();
            foreach (var block in inputGraph.Blocks)
            {
                foreach (var line in block.Enumerate())
                {
                    if (LinearHelper.AsDefinition(line) != null && !isPhi(line.LeftOperand.Value as IdentificatorValue))
                        variables.Add(line.Destination as IdentificatorValue);
                }
            }
            return variables;
        }

        private bool isPhi(IdentificatorValue ident)
        {
            return ident != null
                && ident.Value.Count() >= 3
                && ident.Value.Substring(0, 3) == "phi";
        }

    }
}
