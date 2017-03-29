using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramTree;

namespace LYtest
{
    public static class Parser
    {
        public static Node ParseString(string input)
        {
            MyLanguageParser p = new MyLanguageParser();
            p.Parse(input);
            return p.root;
        }

    }
}
