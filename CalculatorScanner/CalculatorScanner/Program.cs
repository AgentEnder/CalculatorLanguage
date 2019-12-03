using System;

namespace CalculatorScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Scanning now: ");
            Scanner s = new Scanner(@"..\..\..\sample.calc");
            var tokens = s.GetTokens();
            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
            Console.WriteLine("-----------------");
            Parser p = new Parser(tokens);
            var grammaticallyCorrect = p.Parse();
            Console.WriteLine("-----------------");
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
