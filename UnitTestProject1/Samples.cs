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

            public static readonly string sample2 = "i= m -1;" +
                                                    "j = n;" +
                                                    "a = u1;" +

                                                    "while 1 {" +
                                                    " i = i +1;" +
                                                    "j = j -1;" +

                                                    "if 5 { a = u2; }" +
                                                    "i = u3;" +
                                                    "}";

            public static readonly string sample3 = "i = m - 1;" +
                                                    "j = n;" +
                                                    "n = i;" +

                                                    "while 1 {" +
                                                    "  j = i + 1;" +
                                                    "  i = j - 1;" +

                                                    "if 5 { m = b; }" +
                                                    "j = u3;" +
                                                    "}";

            public static readonly string domSampleTrivial = "i = 1;" +

                                                             "if 5 { m = b; }" +
                                                             "j = u3;";

            public static readonly string domSample = "a = 1;" +
                                                      "if 5 {" +
                                                      "  b = 1;" +
                                                      "if 5 {" +
                                                      "  b = 1;" +
                                                      "} else {" +
                                                      "  c = 2;" +
                                                      "}" +
                                                      "} else {" +
                                                      "  c = 2;" +
                                                      "}" +
                                                      "d = a;";
        }
    }
}
