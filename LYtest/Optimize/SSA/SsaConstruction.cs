using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.CFG;

namespace LYtest.Optimize.SSA
{
    class SsaConstruction
    {
        public SsaConstruction()
        {
            /* Empty */
        }
        
        /// <summary>
        /// Inserts phi functions for all variables in graph, 
        /// relying on DF
        /// </summary>
        /// <param name="inputGraph"></param>
        /// <returns></returns>
        private CFGraph InsertPhiFunctions(CFGraph inputGraph)
        {
            CFGraph ssaGraph = inputGraph;
            // To do
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
            // To do
            return ssaGraph;
        }

        public CFGraph GetSsaForm(CFGraph inputGraph)
        {
            var phiGraph = InsertPhiFunctions(inputGraph);
            var ssaGraph = RenameVariables(phiGraph);
            return ssaGraph;
        }

    }
}
