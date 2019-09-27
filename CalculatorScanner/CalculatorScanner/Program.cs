using System;

namespace CalculatorScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            Scanner s = new Scanner(@"..\..\..\sample.calc");
            var tokens = s.GetTokens();
            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
        }
    }
}
