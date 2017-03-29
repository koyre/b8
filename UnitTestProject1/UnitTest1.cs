using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LYtest;
using ProgramTree;

namespace UnitTestProject1
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void ParserTestAssign()
        {
            var root = Parser.ParseString("newVar=0;" +
                                          "k = n + 2 * k;" +
                                          "for m = (5+6)..a*b { c = a+b*3; }");
            var broot = root as BlockNode;

            Assert.IsNotNull(broot);

            var chlds = broot.Childs.ToList();
            Assert.AreEqual(chlds.Count, 3);

            var nAssign = chlds[0] as AssignNode;
            Assert.AreEqual(nAssign?.Id.Name, "newVar");

            var kAssign = (chlds[1] as AssignNode)?.Expr as BinOp;

            Assert.AreEqual(kAssign.Op, Operator.Plus);

            Assert.AreEqual((kAssign.Lhs as IdentNode)?.Name, "n");
            Assert.AreEqual((kAssign.Rhs as BinOp)?.Op, Operator.Mult);
        }

        [TestMethod]
        public void ParserTestErr()
        {
            var root = Parser.ParseString("n=0\n k = ");
            Assert.IsNull(root);
        }
    }
}
