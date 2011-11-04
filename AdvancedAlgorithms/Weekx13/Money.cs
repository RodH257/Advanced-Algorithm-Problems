using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdvancedAlgorithms
{
    /// <summary>
    /// Rod Howarth - n6294685
    /// Team14
    /// </summary>
    public class Money
    {
        private const int SENTINEL = 10000;
        private static void Main(string[] args)
        {
            int testCaseNum = 0;
            while (true)
            {
                int numberBanks = ReadInt();

                if (numberBanks == 0)
                    break;

                testCaseNum++;

                int[,] matrix = new int[numberBanks,numberBanks];

                int originalCash = 0; 

                for (int bankNumber = 0; bankNumber < numberBanks; bankNumber++)
                {
                    var line = GetSplitInts();

                    for (int adjbank = 0; adjbank < numberBanks; adjbank++)
                    {
                        matrix[bankNumber, adjbank] = line[adjbank];

                        if (line[adjbank] > 0)
                            originalCash += line[adjbank];
                    }
                }


                int totalCashNeeded = 0;
                for (int node = 0; node < numberBanks; node++)
                {
                    int totalIn = 0;
                    int totalOut = 0;
                    for (int adj = 0; adj < numberBanks; adj++)
                    {
                        totalIn += matrix[adj, node];
                        totalOut += matrix[node, adj];


                    }
                    int diff = totalOut - totalIn;
                    if (totalOut > totalIn)
                    {
                        totalCashNeeded += diff;
                    }
            
                }





            //    int shortestPathWeight = GetShortestPathWeight(numberBanks + 2, numberBanks, destNode, matrix) - (SENTINEL * 2);
                Console.WriteLine("{0}. {1} {2}", testCaseNum, originalCash, totalCashNeeded);


            }
        }





        #region Utils 
        public static int ReadInt()
        {
            string line = Console.ReadLine();
            return int.Parse(line);
        }

        public static int[] GetSplitInts(string input)
        {
            string[] intStrings = input.Split(' ');
            int[] splitInts = new int[intStrings.Length];
            for (int i = 0; i < intStrings.Length; i++)
                splitInts[i] = int.Parse(intStrings[i]);
            return splitInts;
        }

        public static int[] GetSplitInts()
        {
            string[] intStrings = Console.ReadLine().Split(' ');
            int[] splitInts = new int[intStrings.Length];
            for (int i = 0; i < intStrings.Length; i++)
                splitInts[i] = int.Parse(intStrings[i]);
            return splitInts;
        }
        #endregion
    }
}



//Notes:
/*
 * Money - augmenting/bipartite or something.
 * 
 * Anti Brute Force Lock - permutations with extra logic
 * 
 * Royal Access - a star or something?
 * 
 * May the Right-Force be with you - 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */

