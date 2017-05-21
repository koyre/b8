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
using LYtest.Visitors;
using ProgramTree;

namespace UnitTestProject1
{
    [TestClass]
    public class DemoTest
    {
        [TestMethod]
        public void ReachingDefsIterTest()
        {
            var root = Parser.ParseString(Samples.SampleProgramText.sample2);
            var code = ProgramTreeToLinear.Build(root);
            var blocks = LYtest.BaseBlocks.LinearToBaseBlock.Build(code);
            var cfg = ListBlocksToCFG.Build(blocks);

            var defs = new ReachingDefsIterAlg(cfg);

            foreach (var block in cfg.graph.Vertices)
            {
                Console.Write(block);

                Console.Write("ReachingDefs: ");
                foreach (var labelValue in defs.Out[block])
                {
                    Console.Write(labelValue);
                    Console.Write(", ");
                }
                Console.WriteLine();
                Console.WriteLine("-----------------------");
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
            var root = Parser.ParseString(Samples.SampleProgramText.domSampleTrivial);
            var code = ProgramTreeToLinear.Build(root);
            var blocks = LYtest.BaseBlocks.LinearToBaseBlock.Build(code);
            var cfg = ListBlocksToCFG.Build(blocks);
            
            Console.WriteLine(string.Join("\n", code));

            for (int i = 0; i < blocks.Count(); i++)
            {
                Console.WriteLine(String.Format("\nBlock #{0}:", i));
                Console.WriteLine(string.Join("\n", blocks[i].Enumerate()));
            }

            var dt = new LYtest.DominatorTree.DominatorTree(cfg);
            var node = dt.GetRoot();

            Console.WriteLine("\nDominator tree:");
            Console.WriteLine(dt.ToString());
        }
    }
}
