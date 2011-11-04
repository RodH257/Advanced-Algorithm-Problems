using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedAlgorithms
{
    /// <summary>
    /// Rod Howarth - n6294685
    /// Team14
    /// TESTED WORKING 31-08-2011 12:05am
    /// </summary>
    public class NaiveSubsetSum
    {
        private const string POSSIBLE = "possible";
        private const string NOT_POSSIBLE = "not possible";

        private static int N;//number of items
        private static int[] Set;
        private static int[] Bits;
        private static int Target = 0;
        private static bool Possible = false;
        private static void Main(string[] args)
        {
            int numTestCases = int.Parse(Console.ReadLine());

            for (int testCaseNum = 0; testCaseNum < numTestCases; testCaseNum++)
            {
                Possible = false;
                string[] firstInput = Console.ReadLine().Split(' ');
                N = int.Parse(firstInput[0]);
                Target = int.Parse(firstInput[1]);

                string[] allNumbersLine = Console.ReadLine().Split(' ');

                Set = new int[N];

                Bits = new int[N];
                for (int i = 0; i < N; i++)
                {
                    Set[i] = int.Parse(allNumbersLine[i]);
                }

                //we've got our set of numbers, find a subset that adds to the target
                Enumerate(0);

                if (Possible)
                    Console.WriteLine(POSSIBLE);
                else
                    Console.WriteLine(NOT_POSSIBLE);
            }

        }

        private static void Enumerate(int k)
        {
            if (backtrack(k))
                return;

            if (k == N || Possible)
                return;

            //enumerate with it out of the set
            Enumerate(k + 1);

            //enumerate with it in the set
            Bits[k] = 1;
            Enumerate(k + 1);

            //reset it
            Bits[k] = 0;
        }


        private static bool backtrack(int k)
        {
            int total = 0;

            //check current scores
            for (int i = 0; i < N; i++ )
            {
                if (Bits[i] == 1)
                    total += Set[i];
                if (total > Target)
                    return true;

                if (total == Target)
                {
                    Possible = true;
                    return true;
                }
            }
            return false;
        }

        

    }
}
