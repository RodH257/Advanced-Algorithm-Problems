using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeworkProblems
{
    /// <summary>
    /// Team 8 - Rod Howarth
    /// n6294685
    /// </summary>
    class BigStringSearch
    {
        private const int NOT_FOUND = -1;
        static void Main(string[] args)
        {
            int numLines = int.Parse(Console.ReadLine());
            for (int i = 0; i < numLines; i++)
            {
                string pattern = Console.ReadLine();
                string text = Console.ReadLine();

                int location = FindTextQuickly(pattern, text);

                if (location == NOT_FOUND)
                    Console.WriteLine(i + 1 + " NOT FOUND");
                else
                    Console.WriteLine(i + 1 + " " + location);
            }
        }

        private static int[] ShiftTable(string pattern)
        {
            int[] table = new int[256];
            for (int i = 0; i < table.Length; i++)
            {
                table[i] = pattern.Length - 1;
            }

            for (int j = 0; j <= pattern.Length - 2; j++)
            {
                table[(int)pattern[j]] = pattern.Length - 1 - j;
            }
            return table;
        }


        private static int FindTextQuickly(string pattern, string text)
        {
            int[] shiftTable = ShiftTable(pattern);

            int i = pattern.Length - 1;
            while (i < text.Length)
            {
                int k = 0;

                while (k < pattern.Length && pattern[pattern.Length - 1 - k] == text[i - k])
                {
                    k++;
                }
                if (k == pattern.Length)
                {
                    return i - pattern.Length + 1;
                }
                else
                {
                    i = i + shiftTable[(int)text[i]];
                }

            }
            return NOT_FOUND;
        }

        private static void SimpleStringSearch()
        {
            int numLines = int.Parse(Console.ReadLine());

            for (int i = 0; i < numLines; i++)
            {
                string pattern = Console.ReadLine();
                string text = Console.ReadLine();

                int location = FindText(pattern, text);

                if (location == NOT_FOUND)
                {
                    Console.WriteLine(i + 1 + " NOT FOUND");
                }
                else
                {
                    Console.WriteLine(i + 1 + " " + location);
                }

            }
        }

        private static int FindText(string pattern, string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                for (int j = 0; j < pattern.Length; j++)
                {
                    //if it doesnt match move i
                    if (i + j == text.Length || text[i + j] != pattern[j])
                    {
                        break;
                    }
                    if (j == pattern.Length - 1)
                    {
                        return i;
                    }
                }
            }
            return NOT_FOUND;
        }
    }
}
