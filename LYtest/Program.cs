using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramTree;
using LYtest.Visitors;

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
        }


    }
}
