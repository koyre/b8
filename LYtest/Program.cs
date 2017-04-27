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
using QuickGraph;

namespace LYtest
{
    class Program
    {
        static void Main(string[] args)
        {
            string inp = File.ReadAllText("a.txt");
            var root = Parser.ParseString(inp);

            // Генерация и получение трёхзначного кода
            var linearCode = new LinearCodeVisitor();
            root?.AcceptVisit(linearCode);
            var code = linearCode.code;

            
            // Get blocks and print it
            var blocks = LinearToBaseBlock.Build(code);
            foreach (var block in blocks)
            {
                Console.WriteLine(block.ToString());
            }

            // Get graph and made DepthSpanningTree
            var cfg = ListBlocksToCFG.Build(blocks);
            var dst = new DepthSpanningTree(cfg);
            string dst_viz = dst.ToString();
            Console.WriteLine(dst_viz);


            Console.ReadLine();
        }


    }
}
