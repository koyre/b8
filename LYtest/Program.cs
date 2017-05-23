using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramTree;
using LYtest.Visitors;
using LYtest.BaseBlocks;
using LYtest.CFG;
using LYtest.Optimize.AvailableExprAnalyzer;
using LYtest.Region;

namespace LYtest
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = File.ReadAllText("a.txt");
            //text = "n = 0;" +
            //        "if n {" +
            //        "custom_label1:" +
            //        "goto custom_label2;" +
            //        "}" +
            //        "else {" +
            //        "custom_label2:" +
            //        "goto custom_label1;" +
            //        "}";
            var root = Parser.ParseString(text);

            if (root == null)
            {
                Console.WriteLine("Error");
                return;
            }

            // Генерация и получение трёхзначного кода
            var linearCode = new LinearCodeVisitor();
            root.AcceptVisit(linearCode);
            var code = linearCode.code;

            
            // Get blocks and print it
            var blocks = LinearToBaseBlock.Build(code);
            foreach (var block in blocks)
            {
                Console.WriteLine(block.ToString());
            }

            // Get graph and made DepthSpanningTree
            var cfg = new CFGraph(blocks);
            Console.WriteLine(cfg.ToString());


            var exprsAnalizer = new AvailableExprAnalyzer(cfg);
            exprsAnalizer.analyze();

            var dst = new DepthSpanningTree(cfg);
            string dst_viz = dst.ToString();
            Console.WriteLine(dst_viz);

            Console.WriteLine("");

            Console.WriteLine(cfg.EdgeTypes.ToString());


            var f = cfg.allRetreatingEdgesAreBackwards();
            var r = cfg.getNaturalCyclesForBackwardEdges();
            var rs = new RegionSequence(cfg);
            Console.ReadLine();
        }

    }
}
