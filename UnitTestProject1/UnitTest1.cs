using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LYtest;
using LYtest.LinearRepr;
using LYtest.LinearRepr.Values;
using LYtest.Visitors;
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

        [TestMethod]
        public void LinearCodeTest()
        {
            var root = Parser.ParseString("n=0;" +
                                          "k = n + 5;" +
                                          "if n >= 5 {" +
                                          "x = 0;" +
                                          "} else {" +
                                          "x = 8;" +
                                          "}" +
                                          "n = x + 2 * k;" +
                                          "for i = 0..n {" +
                                          "t = t + i;" +
                                          "k = k + i*3;" +
                                          "}" +
                                          //"while (3 <= n and n < 8) or flag {" +
                                          "n = n /2;" +
                                          "if n == 0 {" +
                                          "flag = 1;" +
                                          //"}" +
                                          "}");

            var linearCode = new LinearCodeVisitor();
            root.AcceptVisit(linearCode);
            var code = linearCode.code;

            var labels = code.Select(c => c.Label);

            // All elems have label
            Assert.IsFalse(labels.Any(c => c == null));

            // All labels different
            Assert.AreEqual(labels.Distinct().Count(), code.Count);
            
            var blocks = LYtest.BaseBlocks.LinearToBaseBlock.Build(code);

            // All elems in blocks
            Assert.AreEqual(blocks.Select(b => b.Enumerate().Count()).Sum(), code.Count);

            // There is no empty blocks
            Assert.IsTrue(blocks.All(b => b.Enumerate().Any()));

            var isGotoOp =
                new Func<IThreeAddressCode, bool>(
                    c => c.Operation == Operation.CondGoto || c.Operation == Operation.Goto);

            foreach (var baseBlock in blocks)
            {
                var notLast = baseBlock.Enumerate().Reverse().Skip(1);
                Assert.IsFalse(notLast.Any(isGotoOp));
            }

            foreach (var baseBlock in blocks)
            {
                foreach (var threeAddressCode in baseBlock.Enumerate())
                    Console.WriteLine(threeAddressCode);

                Console.WriteLine();
            }

            var gotoLables = code.Where(isGotoOp).Select(c => c.Destination.Value);
            var firstLabels = blocks.Select(b => b.Enumerate().First().Label.Value);
            foreach (var d in gotoLables)
            {
                Assert.IsTrue(firstLabels.Contains(d));
            }
        }
    }
}
