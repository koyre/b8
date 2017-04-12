using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.Visitors;
using ProgramTree;

namespace LYtest.LinearRepr
{
    public static class ProgramTreeToLinear
    {
        public static List<LinearRepresentation> Build(Node root)
        {
            var linearCode = new LinearCodeVisitor();
            root.AcceptVisit(linearCode);
            return linearCode.code;
        }
    }
}
