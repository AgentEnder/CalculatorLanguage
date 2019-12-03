using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CalculatorScanner
{
    class Parser
    {
        public Queue<string> tokens;

        private List<string> reservedWords = new List<string>
        {
            "read",
            "write"
        };

        private List<string> addOps = new List<string>
        {
            "+",
            "-"
        };

        private List<string> multOps = new List<string>
        {
            "*",
            "/"
        };

        private bool callStackChanged = false;
        private Stack<string> callStack = new Stack<string>();

        private string lookahead;

        public Parser(List<string> _tokens)
        {
            tokens = new Queue<string>(_tokens);
        }

        public bool Parse()
        {
            lookahead = tokens.Dequeue();
            program();
            if (lookahead == "$$")
            {
                match("$$");
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool match(string token)
        {
            if (callStackChanged)
            {
                var array = callStack.ToArray()[0..(callStack.Count > 4 ? 4 : callStack.Count)];
                Array.Reverse(array);
                Console.WriteLine($"Executing: {string.Join(" ",  array)}");
                callStackChanged = false;
            }
            if (lookahead == token)
            {
                Console.WriteLine($"\t Token: {lookahead} consumed");
                lookahead = tokens.Count > 0 ? tokens.Dequeue() : null;
                return true;
            }
            else
            {
                Console.WriteLine($"Expected {token}, recieved {lookahead}.");
                return false;
            }
        }

        private bool match_id(string token)
        {
            if (!reservedWords.Contains(token) && Regex.Match(token, "(?:[A-Z]|[a-z]|-|_)*").Success)
            {
                return match(token);
            }
            else
            {
                Console.WriteLine($"Invalid ID: {token}");
                return false;
            }
        }

        private void program()
        {
            callStackChanged = true;
            callStack.Push("program()");
            stmt_list();
        }

        private void stmt_list()
        {
            callStackChanged = true;
            callStack.Push("stmt_list()");
            if (tokens.Count == 0)
            {
                return;
            }
            stmt();
            stmt_list();
            callStack.Pop();
        }

        private void stmt()
        {
            callStackChanged = true; 
            callStack.Push("stmt()");
            if (lookahead == "read")
            {
                match("read");
                match_id(lookahead);
            }
            else if (lookahead == "write")
            {
                match("write");
                expr();
            }
            else
            {
                match_id(lookahead);
                match(":=");
                expr();
            }
            callStack.Pop();
        }

        private void expr()
        {
            callStackChanged = true; 
            callStack.Push("expr()");
            term();
            term_tail();
            callStack.Pop();
        }

        private void term_tail()
        {
            callStackChanged = true; 
            callStack.Push("term_tail()");
            if (addOps.Contains(lookahead))
            {
                add_op();
                term();
                term_tail();
            }
            else
            {
                return; //epsilon
            }
            callStack.Pop();
        }

        private void term()
        {
            callStackChanged = true; 
            callStack.Push("term()");
            factor();
            fact_tail();
            callStack.Pop();
        }

        private void fact_tail()
        {
            callStackChanged = true; 
            callStack.Push("fact_tail()");
            if (multOps.Contains(lookahead))
            {
                mult_op();
                factor();
                fact_tail();
            }
            callStack.Pop();
        }

        private void factor()
        {
            callStackChanged = true; 
            callStack.Push("factor()");
            if (lookahead == "(")
            {
                match("(");
                expr();
                match(")");
            }
            else if (double.TryParse(lookahead, out _)) // The token is a number
            {
                match(lookahead);
            }
            else
            {
                match_id(lookahead);
            }
            callStack.Pop();
        }

        private void mult_op()
        {
            callStackChanged = true; 
            callStack.Push("mult_op()");
            match(lookahead);
            callStack.Pop();
        }
        
        private void add_op()
        {
            callStackChanged = true; 
            callStack.Push("add_op()");
            match(lookahead);
            callStack.Pop();
        }

    }
}
