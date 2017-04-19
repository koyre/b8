using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    static class Samples
    {
        public static class SampleProgramText
        {
            public static readonly string sample1 = "n=0;" +
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
                                     "while (3 <= n and n < 8) or flag {" +
                                     "n = n /2;" +
                                     "if n == 0 {" +
                                     "flag = 1;" +
                                     "}" +
                                     "}";
        }
    }
}
