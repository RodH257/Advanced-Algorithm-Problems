using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeworkProblems.Week6
{
    class NQueens
    {
        private static int N;//number of items
        private static int[] A;
        private static int counter = 0;

        private static void Main(string[] args)
        {

            int n = int.Parse(Console.ReadLine());

            while (n != 0)
            {
                counter = 0;
                N = n;
                A = new int[N];

                //initialize each to i
                for (int i = 0; i < N; i++)
                    A[i] = i;

                //enumerate starting from 0
                enumerate(0);

                Console.WriteLine("N={0}: {1} permutations", n, counter);
                n = int.Parse(Console.ReadLine());
            }
        }

        private static void enumerate(int k)
        {
            if (k == N)
            {
                counter++;
                return;
            }

            for (int i = k; i < N; i++)
            {
                swap(i, k);
                if (!backtrack(k))
                    enumerate(k + 1);
                swap(i, k);
            }

        }

        private static bool backtrack(int k)
        {
            for (int i = 0; i < k; i++)
            {
                //check if diagonal hits
                if ((A[i] - A[k] == k - i)
                    || (A[k] - A[i] == k - i))
                {
                    return true;
                }
            }
            return false;
        }

        private static void swap(int i, int k)
        {
            var currentI = A[i];
            var currentK = A[k];

            A[i] = currentK;
            A[k] = currentI;
        }
    }


}
