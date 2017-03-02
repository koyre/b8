using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramTree;


namespace LYtest
{
    class Program
    {
        static void Main(string[] args)
        {

            string FileName = "a.txt";


            string inp = File.ReadAllText(FileName);

            MyLanguageScanner s = new MyLanguageScanner();
            s.SetSource(inp, 0);
            MyLanguageParser p = new MyLanguageParser(s);

            var r = p.Parse();
           

            Console.WriteLine(r);
        }


    }
}
