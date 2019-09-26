using System;

namespace CalculatorScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            Scanner s = new Scanner(@"..\..\..\sample.calc");
            s.GetTokens();
        }
    }
}
