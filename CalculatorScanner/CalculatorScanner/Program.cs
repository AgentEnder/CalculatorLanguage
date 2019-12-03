using System;
using System.Collections.Generic;

namespace CalculatorScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----------------------------\n");
            Console.WriteLine("Scanning now: \n");
            Console.WriteLine("----------------------------\n");
            Scanner s = new Scanner(@"..\..\..\sample.calc");
            var tokens = new List<string>();
            if (!s.TryGetTokens(out tokens))
            {
                return;
            }
            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
            Console.WriteLine("\n----------------------------\n");
            Console.WriteLine("Parsing Now: \n");
            Console.WriteLine("----------------------------\n");
            Parser p = new Parser(tokens);
            var grammaticallyCorrect = p.Parse();
            Console.WriteLine("----------------------------");
            if (grammaticallyCorrect)
            {
                Console.WriteLine("No grammar errors found!");
            }
            else
            {
                Console.WriteLine("Parser found errors");
            }
        }
    }
}
