using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LYtest;
using LYtest.CFG;
using LYtest.Interpretator;
using LYtest.LinearRepr;
using LYtest.LinearRepr.Values;
using LYtest.ReachingDefs;
using LYtest.ActiveVars;
using LYtest.BaseBlocks;
using LYtest.Visitors;
using ProgramTree;
using LYtest.Optimize.AvailableExprAnalyzer;
using QuickGraph.Graphviz;

namespace UnitTestProject1
{
    [TestClass]
    public class DemoTest
    {
        [TestMethod]
        public void CFGraphTest()
        {
            var root = Parser.ParseString(Samples.SampleProgramText.sample2);
            var linearCode = new LinearCodeVisitor();
            root.AcceptVisit(linearCode);

            var code = linearCode.code;
            var blocks = LinearToBaseBlock.Build(code);

            var cfg = new CFGraph(blocks);
            Console.WriteLine(cfg.ToString());
        }

        [TestMethod]
        public void ReachingDefsIterTest()
        {
            var root = Parser.ParseString(Samples.SampleProgramText.sample2);
            var code = ProgramTreeToLinear.Build(root);
            var blocks = LYtest.BaseBlocks.LinearToBaseBlock.Build(code);
            var cfg = ListBlocksToCFG.Build(blocks);

            var defs = new ReachingDefsIterAlg(cfg);
            cfg.InfoFunc = node =>
            {
                var outs = string.Join(", ", defs.Out[node]);
                var ins = node.ParentsNodes.Count > 0 ? string.Join(", ", defs.In[node]) : "";
                return $" In: {ins} \n\t\t\tOut: {outs}";
            };
            Console.WriteLine(cfg);
        }

        [TestMethod]
        public void DSTTest()
        {
            var root = Parser.ParseString(Samples.SampleProgramText.sample2);
            var linearCode = new LinearCodeVisitor();
            root.AcceptVisit(linearCode);

            var code = linearCode.code;
            var blocks = LinearToBaseBlock.Build(code);
            var cfg = new CFGraph(blocks);
            var dst = new DepthSpanningTree(cfg);

            cfg.ShowCompact = true;
            Console.WriteLine(cfg.ToString());
            Console.WriteLine(dst.ToString());
        }

        [TestMethod]
        public void EdgesTypesTest()
        {
            var root = Parser.ParseString(Samples.SampleProgramText.sample2);
            var linearCode = new LinearCodeVisitor();
            root.AcceptVisit(linearCode);

            var code = linearCode.code;
            var blocks = LinearToBaseBlock.Build(code);

            var cfg = new CFGraph(blocks);
            Console.WriteLine(cfg.ToString());
            Console.WriteLine(cfg.EdgeTypes.ToString());
        }

        [TestMethod]
        public void allRetreatingEdgesAreBackwardsTest()
        {
            var root = Parser.ParseString(Samples.SampleProgramText.sample1);
            var code = ProgramTreeToLinear.Build(root);
            var blocks = LinearToBaseBlock.Build(code);
            var cfg = ListBlocksToCFG.Build(blocks);
            cfg.ShowCompact = true;
            Console.WriteLine(cfg);
            Console.WriteLine("allRetreatingEdgesAreBackwards: " + cfg.allRetreatingEdgesAreBackwards());
            Assert.AreEqual(cfg.allRetreatingEdgesAreBackwards(), true);

            root = Parser.ParseString(Samples.SampleProgramText.veryStrangeCode);
            code = ProgramTreeToLinear.Build(root);
            blocks = LinearToBaseBlock.Build(code);
            cfg = ListBlocksToCFG.Build(blocks);
            cfg.ShowCompact = true;
            Console.WriteLine(cfg);
            Console.WriteLine("allRetreatingEdgesAreBackwards: " + cfg.allRetreatingEdgesAreBackwards());
            Assert.AreEqual(cfg.allRetreatingEdgesAreBackwards(), false);

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
            var blocks = LYtest.BaseBlocks.LinearToBaseBlock.Build(code);
            var cfg = ListBlocksToCFG.Build(blocks);


            var exprsAnalizer = new AvailableExprAnalyzer(cfg);
            exprsAnalizer.analyze();


            foreach (var block in blocks)
            {
                Console.Write("Block: " + block);

                Console.Write("\n");

                Console.Write("IN: \t");
                foreach (var expr in exprsAnalizer.InBlocks[block])
                {
                    Console.Write(expr);
                    Console.Write(", ");
                }
                Console.Write("\n");

                Console.Write("Out: \t");
                foreach (var expr in exprsAnalizer.OutBlocks[block])
                {
                    Console.Write(expr);
                    Console.Write(", ");
                }
                Console.Write("\n");

                Console.WriteLine("-----------------------");
            }
        }


        [TestMethod]
        public void GenKillTest()
        {
            var root = Parser.ParseString(Samples.SampleProgramText.sample2);
            var code = ProgramTreeToLinear.Build(root);
            var blocks = LYtest.BaseBlocks.LinearToBaseBlock.Build(code);
            
            var genKill = new GenKillBuilder(blocks);

            foreach (var b in blocks)
            {
                Console.WriteLine(b);
                Console.Write("Gen: ");
                Console.WriteLine(string.Join(",", genKill.Gen[b]));
                Console.Write("Kill: ");
                Console.WriteLine(string.Join(",", genKill.Kill[b]));
                Console.WriteLine("-------------------------");
            }
            Assert.AreEqual(genKill.Gen[blocks[0]].Count, 3);
            Assert.AreEqual(genKill.Kill[blocks[0]].Count, 4);

            Assert.AreEqual(genKill.Gen[blocks[6]].Count, 1);
            Assert.AreEqual(genKill.Kill[blocks[6]].Count, 2);
        }

        [TestMethod]
        public void DominatorTreeDemo()
        {
            var root = Parser.ParseString(Samples.SampleProgramText.sample2);
            var code = ProgramTreeToLinear.Build(root);
            var blocks = LYtest.BaseBlocks.LinearToBaseBlock.Build(code);
            var cfg = ListBlocksToCFG.Build(blocks);
 
            var dt = new LYtest.DominatorTree.DominatorTree(cfg);
            var node = dt.GetRoot();
            cfg.ShowCompact = true;
            Console.WriteLine(cfg);

            Console.WriteLine("\nDominator tree:");
            Console.WriteLine(dt.ToString());
        }

        [TestMethod]
        public void naturalCycleTest()
        {
            var root = Parser.ParseString(Samples.SampleProgramText.sample1);
            var code = ProgramTreeToLinear.Build(root);
            var blocks = LinearToBaseBlock.Build(code);
            var cfg = ListBlocksToCFG.Build(blocks);
            cfg.ShowCompact = true;
            Console.WriteLine(cfg);
            var ncg = new NaturalCycleGraph(cfg);
            var res = ncg.findBetween(6, 4);
            res.Sort();
            var expected = new List<int>() { 4, 6 };
            CollectionAssert.AreEqual(res, expected);

            var res1 = ncg.findBetween(13, 8);

            res1.Sort();
            Console.Write("Cycle btw 13, 8: " + string.Join(", ", res1));
            var expected1 = new List<int>() { 8, 10, 11, 12, 13 };
            CollectionAssert.AreEqual(res1, expected1);

        }
    }
}
