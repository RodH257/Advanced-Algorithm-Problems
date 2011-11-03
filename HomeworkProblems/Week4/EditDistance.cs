using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeworkProblems
{
    /// <summary>
    /// Rod Howarth - n6294685
    /// Team14
    /// </summary>
    public class EditDistance
    {
        private const int NOT_INITIALIZED = -1;
        private static void Main(string[] args)
        {
            int numTestCases = int.Parse(Console.ReadLine());

            for (int i = 0; i < numTestCases; i++)
            {
                string firstWord = Console.ReadLine();
                string secondWord = Console.ReadLine();
                Console.WriteLine(GetNumberIntegers(firstWord, secondWord));
            }
        }


        private static int GetNumberIntegers(string x, string y)
        {
            if (x.Length == 0)
                return y.Length;
            if (y.Length == 0)
                return x.Length;

            int m = x.Length;
            int n = y.Length;

            int[][] editDistances = new int[m + 1][];

            for (int i = 0; i <= m; i++)
            {
                editDistances[i] = new int[n + 1];
                editDistances[i][0] = i;
            }

            for (int j = 1; j <= n; j++)
                editDistances[0][j] = j;

            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                  
                    int insertionCost = editDistances[i - 1][j] + 1;
                    int deletionCost = editDistances[i][j - 1] + 1;
                    int diff = 1;
                    if (x[i - 1] == y[j - 1])
                        diff = 0;
                    int swappingCost = editDistances[i - 1][j - 1] + diff;

                    List<int> costs = new List<int> 
                    { insertionCost, deletionCost, swappingCost };
                    editDistances[i][j] = costs.Min();
                }
            }

            return editDistances[m][n];
        }
    }
}