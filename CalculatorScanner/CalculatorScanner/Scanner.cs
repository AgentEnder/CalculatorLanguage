using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CalculatorScanner
{
    
    class Scanner
    {
        string filePath;
        public Scanner(string FilePath)
        {
            filePath = FilePath;
        }

        public List<string> GetTokens()
        {
            List<string> tokens = new List<string>();
            using (FileStream f = new FileStream(filePath, FileMode.Open))
            {
                StreamReader reader = new StreamReader(f);
                Queue<char> queue = new Queue<char>(reader.ReadToEnd());
                foreach (var item in queue)
                {
                    Console.WriteLine(item);
                }
            }
            return tokens;
        }

    }
}
