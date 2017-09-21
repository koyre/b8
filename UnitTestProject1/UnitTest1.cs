using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LYtest;
using LYtest.BaseBlocks;
using LYtest.CFG;
using LYtest.Interpretator;
using LYtest.LinearRepr;
using LYtest.LinearRepr.Values;
using LYtest.ReachingDefs;
using LYtest.ActiveVars;
using LYtest.Visitors;
using ProgramTree;
using LYtest.Optimize.AvailableExprAnalyzer;
using LYtest.Region;

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
            var root = Parser.ParseString(Samples.SampleProgramText.sample1);
            var code = ProgramTreeToLinear.Build(root);
            var blocks = LYtest.BaseBlocks.LinearToBaseBlock.Build(code);

            var labels = code.Select(c => c.Label);

            // All elems have label
            Assert.IsFalse(labels.Any(c => c == null));

            // All labels different
            Assert.AreEqual(labels.Distinct().Count(), code.Count);
            

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


        [TestMethod]
        public void SimpleInterpret()
        {
            var root = Parser.ParseString("n = 666; print(n);");
            var code = ProgramTreeToLinear.Build(root);
            var res = LinearInterpretator.Run(code);
            Assert.AreEqual(res[0], 666);
            Assert.AreEqual(res.Count, 1);
        }

        [TestMethod]
        public void SimpleInterpret2()
        {
            var root = Parser.ParseString("n = 1;" +
                                          "k=n;" +
                                          "z=k+1;" +
                                          "print(2);" +
                                          "print(z);" +
                                          "");
            var code = ProgramTreeToLinear.Build(root);
            var res = LinearInterpretator.Run(code);
            Assert.AreEqual(res.Count, 2);
            Assert.AreEqual(res[0], 2);
            Assert.AreEqual(res[1], 2);

        }

        [TestMethod]
        public void SimpleInterpret3()
        {
            var root = Parser.ParseString("n = 1;" +
                                          "k=n;" +
                                          "z=k+1;" +
                                          "print(z);" +
                                          "t = 2*3;" +
                                          "if (z > 0) {" +
                                          "for x = 2..(z+2) {" +
                                          "t = t + 1; }" +
                                          "}" +
                                          "print (t);");
            var code = ProgramTreeToLinear.Build(root);
            var res = LinearInterpretator.Run(code);
            
            Assert.AreEqual(res.Count, 2);
            Assert.AreEqual(res[0], 2);
            Assert.AreEqual(res[1], 9);

        }

        [TestMethod]
        public void CFGTest()
        {
            var tree = Parser.ParseString(Samples.SampleProgramText.sample1);
            var code = ProgramTreeToLinear.Build(tree);
            var blocks = LYtest.BaseBlocks.LinearToBaseBlock.Build(code);
            
            var cfg = ListBlocksToCFG.Build(blocks);
            Assert.AreEqual(cfg.GetRoot().ParentsNodes.Count, 0);

            Assert.AreEqual(cfg.Blocks.Count, 15);

            Assert.IsNotNull(cfg.GetRoot().directChild);
            Assert.IsNotNull(cfg.GetRoot().gotoNode);

            cfg.Blocks.ForEach(Console.WriteLine);
        }

        [TestMethod]
        public void LabelGotoTest()
        {
            var root = Parser.ParseString("label1 :" +
                                          "n = 1;" +
                                          "goto label1;");
            var code = ProgramTreeToLinear.Build(root);

            foreach (var threeAddressCode in code)
                Console.WriteLine(threeAddressCode);
            Assert.AreEqual(code.Count, 3);
        }


        [TestMethod]
        public void CFGNodeSetTest()
        {
            var root = Parser.ParseString(Samples.SampleProgramText.sample1);
            var code = ProgramTreeToLinear.Build(root);
            var blocks = LYtest.BaseBlocks.LinearToBaseBlock.Build(code);
            var cfg = ListBlocksToCFG.Build(blocks);
            
            var nodes = cfg.graph.Vertices.Select(n => n.Value);
            Assert.AreEqual(nodes.Count(), blocks.Count());

            foreach (var b in blocks)
            {
                Assert.IsTrue(nodes.Contains(b));
            }
        }

        [TestMethod]
        public void ActiveVarsIterTest()
        {
            var root = Parser.ParseString(Samples.SampleProgramText.sample3);
            var code = ProgramTreeToLinear.Build(root);
            var blocks = LYtest.BaseBlocks.LinearToBaseBlock.Build(code);
            var cfg = ListBlocksToCFG.Build(blocks);

            var DefUse = new ActiveVarsIterAlg(cfg);

            foreach (var block in cfg.graph.Vertices)
            {
                Console.Write(block);
                foreach (var labelValue in DefUse.In[block])
                {
                    Console.Write(labelValue);
                    Console.Write(", ");
                }
                Console.WriteLine();
                Console.WriteLine("-----------------------");
            }
        }

        [TestMethod]
        public void AvailableExpressionsTest()
        {
            Console.Write("----------- Available Expressions Analyzer ---------- \n");
            var root = Parser.ParseString(Samples.SampleProgramText.AvailableExprsSample);
            var code = ProgramTreeToLinear.Build(root);
            var blocks = LinearToBaseBlock.Build(code);
            var cfg = ListBlocksToCFG.Build(blocks);


            var exprsAnalizer = new AvailableExprAnalyzer(cfg);
            exprsAnalizer.analyze();

            Assert.IsTrue(exprsAnalizer.OutBlocks[blocks[0]].Count == 1);
            Assert.IsTrue(exprsAnalizer.InBlocks[blocks[2]].Count == 1);
            Assert.IsTrue(exprsAnalizer.OutBlocks[blocks[2]].Count == 0);
        }


        [TestMethod]
        public void DominatorTreeTest()
        {
            var root = Parser.ParseString(Samples.SampleProgramText.domSampleTrivial);
            var code = ProgramTreeToLinear.Build(root);
            var blocks = LYtest.BaseBlocks.LinearToBaseBlock.Build(code);
            var cfg = ListBlocksToCFG.Build(blocks);

            var dt = new LYtest.DominatorTree.DominatorTree(cfg);
            var node = dt.GetRoot();
            Console.WriteLine(dt.ToString());
            Console.WriteLine(cfg.ToString());
            Assert.AreEqual(dt.NumberOfVertices(), 4);
        }

        [TestMethod]
        public void DF_IDF_Test()
        {
            var root = Parser.ParseString(Samples.SampleProgramText.domSample);
            var code = ProgramTreeToLinear.Build(root);
            var blocks = LYtest.BaseBlocks.LinearToBaseBlock.Build(code);
            var cfg = ListBlocksToCFG.Build(blocks);
            Console.WriteLine(cfg.ToString());

            var df = new DominanceFrontier(blocks);
            var idf = df.ComputeIDF_ForEachBlock(blocks);
            Console.WriteLine(df.ToString());
            
            foreach (var pair in idf)
            {
                Console.WriteLine("FOR BLOCK:");
                Console.WriteLine(pair.Key.ToString());
                Console.WriteLine("IDF WAS:");
                foreach (var val in pair.Value)
                {
                    Console.WriteLine(val.ToString());
                }
                Console.WriteLine();
            }


            Console.WriteLine("FINAL IDF:");
            var idf2 = df.ComputeIDF(blocks);
            foreach (var value in idf2)
            {
                Console.WriteLine(value.ToString());
            }
        }

        [TestMethod]
        public void RegionSequenceTest()
        {
            var root = Parser.ParseString(Samples.SampleProgramText.regionSqeuenceSample);
            var code = ProgramTreeToLinear.Build(root);
            var blocks = LinearToBaseBlock.Build(code);
            var cfg = ListBlocksToCFG.Build(blocks);

            var regSeq = new RegionSequence(cfg);
            var natCycles = cfg.getNaturalCyclesForBackwardEdges();
            Assert.AreEqual(regSeq.Regions.Count, 24);
            Assert.AreEqual(regSeq.Regions.Last().Header, cfg.GetVertices().ToList()[0]);
            Assert.IsTrue(regSeq.Regions.Exists(r => natCycles[0][0] == r.Header));
        }

    }
}
