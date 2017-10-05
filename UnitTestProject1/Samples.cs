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
        

        public static readonly string veryStrangeCode = "n = 0;" +
                                                        "if n {" +
                                                        "custom_label1:" +
                                                        "goto custom_label2;" +
                                                        "}" +
                                                        "custom_label2:" +
                                                        "goto custom_label1;";

        public static readonly string AvailableExprsSample = "a = b + c;" +
                                                "b = a - d;" +
                                                "c = b + c;" +
                                                "n = 0;" +

                                                "if n {" +
                                                "  d = a - d;" +
                                                "  a = 5;" +
                                                "  b = 9;" +
                                                "  for m = (5+6)..a*b {" +
                                                "  c = a+b*3;" +

                                                " }" +
                                                "}";
            public static readonly string regionSqeuenceSample = "while c1" +
                                                    "{" +
                                                    "d = a - d;" +
                                                    "for m = (5+6)..a* b" +
                                                    "{" +
                                                    "c = a+b*3;" +
                                                    "for i = 1..4 {" +
                                                    "t = 10;" +
                                                    "}" +
                                                    "while cond {" +
                                                    "w = 0;" +
                                                    "}" +
                                                    "}" +
                                                    "}";
            public static readonly string constantPropagationSample =
                                                    "a = 3;" +
                                                    "b = 4;" +
                                                    "c = a;" +
                                                    "d = b + 3;" +
                                                    "e = a + b;";
            public static readonly string symbolicAnalysisSample =
                                                    "y = 3;" +
                                                    "z = 5;" +
                                                    "u = 9;" +
                                                    "v = 6*u+1 + y;" +
                                                    "y = 5 * z;" +
                                                    "x = 4 + 3*y + z;";
            public static readonly string symbolicAnalysisSample2 =
                                                    "y = 9;" +
                                                    "z = y+5;" +
                                                    "x = 14 + 11*y + z;";
        }
    }
}
