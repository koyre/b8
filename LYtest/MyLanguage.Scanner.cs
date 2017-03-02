using System;
using System.Collections.Generic;
using System.Text;

namespace LYtest
{
    internal partial class MyLanguageScanner
    {

        void GetNumber()
        {
        }

		public override void yyerror(string format, params object[] args)
		{
			base.yyerror(format, args);
			Console.WriteLine(format, args);
			Console.WriteLine();
		}
    }
}
