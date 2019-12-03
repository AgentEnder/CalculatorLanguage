using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CalculatorScanner
{
    
    class Scanner
    {
        static string nonDivisionOperators = "*-+";
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
                queue.Enqueue(' '); //Prevent error when there is not whitespace at the EoF by adding a space.
                while (queue.Count > 0)
                {
                    char current; //Start of a new token
                    current = queue.Dequeue();
                    while (char.IsWhiteSpace(current) && queue.Count > 0) // Remove whitespace characters before tokens
                    {
                        current = queue.Dequeue();
                    }
                    //Check if comment, otherwise report division.
                    if (current == '/')
                    {
                        char next = queue.Peek();
                        if (next == '/')
                        {
                            while (queue.Dequeue() != '\n') { } //Do nothing,just getting rid of characters until end of line
                        }
                        else if (next == '*')
                        {
                            char prev = current;
                            current = queue.Dequeue();
                            while (prev != '*' || current != '/')
                            {
                                queue.Dequeue(); //Remove characters between /* and */
                            }
                        }
                        else
                        {
                            tokens.Add(current.ToString()); //Report Division Operator
                        }
                    }
                    //Check for valid numbers
                    else if (".0123456789".Contains(current))
                    {
                        string token = current.ToString();
                        char next = queue.Peek();
                        bool add = true;
                        while (!char.IsWhiteSpace(next)) //Tokens end on whitespace
                        {
                            bool acceptDecimalPoint = current != '.'; //numbers can have ONE decimal point
                            if ("0123456789".Contains(next) || (acceptDecimalPoint && next == '.'))
                            {
                                token += next; //Add next char to token
                                if (next == '.') //Only accept one character
                                {
                                    acceptDecimalPoint = false; //One decimal point
                                }
                                queue.Dequeue(); //Handled character
                            }
                            else if ("+-*/)".Contains(next)) //Valid next token, handle that at later time.
                            {
                                tokens.Add(token);
                                add = false;
                                break;
                            }
                            else
                            {
                                throw new Exception("Scanner encountered an error!");
                            }
                            next = queue.Peek(); //Look at next char
                        }
                        if (add)
                            tokens.Add(token);
                    }
                    //Check for valid symbols and operators (division already handled)
                    else if ((nonDivisionOperators + "():").Contains(current))
                    {
                        if (current == ':')
                        {
                            char next = queue.Peek();
                            if (next == '=')
                            {
                                queue.Dequeue();
                                tokens.Add(":=");
                                continue;
                            }
                            else
                            {
                                throw new Exception(": only valid as :=");
                            }
                        }
                        else if (current != ')') //Next can't be another operator
                        {
                            char next = queue.Peek(); //Check next character
                            if (nonDivisionOperators.Contains(next)) //Sequential operators not allowed
                            {
                                throw new Exception("Non separated operators");
                            }
                        }
                        tokens.Add(current.ToString()); //Capture a token
                                                        //Handle keywords and Id's
                    }
                    else if (char.IsLetter(current))
                    {
                        string token = current.ToString();
                        char next = queue.Peek(); //Look at next character
                        while (char.IsLetterOrDigit(next)) //Id or keyword continues/
                        {
                            token += queue.Dequeue(); //Add to token
                            next = queue.Peek(); //Look at next
                        }
                        tokens.Add(token); ///Add to list
                    }
                    else if (current == '$')
                    {
                        var next = queue.Peek();
                        if (next == '$')
                        {
                            tokens.Add("$$");
                            return tokens; //Reached end of program
                        }
                        else
                        {
                            throw new Exception("Single $ not allowed in program!");
                        }
                    }
                }
            }
            return tokens;
        }
    }
}
