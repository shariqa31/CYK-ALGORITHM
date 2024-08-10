using System;
using System.Collections.Generic;

class CYKParser
{
    static void Main()
    {
        // Read CFG from the user
        Console.WriteLine("Enter the CFG in the format S->AB|BC,A->BA|a,B->CC|b,C->AB|a (without spaces):");
        string[] rulesInput = Console.ReadLine().Split(',');

        Dictionary<char, List<string>> cfg = new Dictionary<char, List<string>>();

        foreach (string rule in rulesInput)
        {
            string[] parts = rule.Split("->");
            char lhs = parts[0][0];
            string[] rhs = parts[1].Split('|');

            if (!cfg.ContainsKey(lhs))
            {
                cfg[lhs] = new List<string>();
            }

            cfg[lhs].AddRange(rhs);
        }

        // Read the input string
        Console.WriteLine("Enter the input string:");
        string input = Console.ReadLine();
        int n = input.Length;

        // Initialize the table
        HashSet<char>[,] table = new HashSet<char>[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                table[i, j] = new HashSet<char>();
            }
        }

        // Base case: Fill the table for substrings of length 1
        for (int i = 0; i < n; i++)
        {
            foreach (var rule in cfg)
            {
                foreach (string production in rule.Value)
                {
                    if (production.Length == 1 && production[0] == input[i])
                    {
                        table[i, 0].Add(rule.Key);
                    }
                }
            }
        }

        // Recursive case: Fill the table for substrings of length 2 to n
        for (int l = 2; l <= n; l++)
        {
            for (int i = 0; i <= n - l; i++)
            {
                for (int j = 1; j < l; j++)
                {
                    HashSet<char> leftSet = table[i, j - 1];
                    HashSet<char> rightSet = table[i + j, l - j - 1];

                    foreach (char left in leftSet)
                    {
                        foreach (char right in rightSet)
                        {
                            foreach (var rule in cfg)
                            {
                                foreach (string production in rule.Value)
                                {
                                    if (production.Length == 2 && production[0] == left && production[1] == right)
                                    {
                                        table[i, l - 1].Add(rule.Key);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Print the table


        // Check if the input string belongs to the CFG
        if (table[0, n - 1].Contains('S'))
        {
            for (int l = 0; l < n; l++)
            {
                for (int i = 0; i < n - l; i++)
                {
                    Console.Write("{");
                    foreach (char c in table[i, l])
                    {
                        Console.Write(c);
                    }
                    Console.Write("}");
                    if (i < n - l - 1) Console.Write(", ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("The input string belongs to the CFG.");
        }
        else
        {
            Console.WriteLine("The input string does not belong to the CFG.");
        }
    }
}
