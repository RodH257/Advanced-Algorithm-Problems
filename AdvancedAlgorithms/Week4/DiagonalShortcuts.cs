﻿using System;

namespace AdvancedAlgorithms
{
    /// <summary>
    /// Rod Howarth - n6294685
    /// Team14
    /// </summary>
    public class DiagonalShorcuts
    {
        private const int NOT_INITIALIZED = -1;
        private static void Main(string[] args)
        {
            while (true)
            {
                string readLine = Console.ReadLine();
                int n = int.Parse(readLine.Split(' ')[0]);
                int m = int.Parse(readLine.Split(' ')[1]);

                if (n == 0 && m == 0)
                    return;

                Console.WriteLine(CountPaths(n, m));
            }
        }

        /// <summary>
        /// Counts the paths from 0,0 to x,y on a grid
        /// Allows diagonal paths
        /// </summary>
        private static Int64 CountPaths(int x, int y)
        {
            //table to store dynamic programming subproblem reults
            Int64[][] table = new Int64[x + 1][];

            //initialize edges to be 1;
            for (int i = 0; i <= x; i++)
            {
                table[i] = new Int64[y + 1];
                table[i][0] = 1;
            }
            for (int j = 0; j <= y; j++)
                table[0][j] = 1;


            //calculate each position bottom up
            for (int i = 1; i <= x; i++)
            {
                for (int j = 1; j <= y; j++)
                {
                    //the value for 1 square is the value of the ones before it added together
                    table[i][j] = table[i - 1][j] + table[i][j - 1] + table[i - 1][j - 1];
                }
            }

            return table[x][y];
        }
    }
}