using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeworkProblems
{
    /// <summary>
    /// Rod Howarth - n6294685
    /// Team14
    /// </summary>
    public class NextPermutation
    {
        private static int N;//number of items
        private static int[] A;
        private static string Original;
        private static void Main(string[] args)
        {
            int numTestCases = int.Parse(Console.ReadLine());


            for (int testCaseNum = 0; testCaseNum < numTestCases; testCaseNum++)
            {
                string[] firstInput = Console.ReadLine().Split(' ');
                string testCaseName = firstInput[0];

                N = firstInput[1].Length;

                Original = firstInput[1];

                A = new int[N];
                for (int i = 0; i < N; i++)
                    A[i] = int.Parse(firstInput[1][i].ToString());

                Console.Write(testCaseName + " ");
                //we've got our set of numbers, find a subset that adds to the target
                Enumerate();

                Console.WriteLine();
            }

        }

        private static void Enumerate()
        {
            bool foundPermutation = false;
            for (int i = N - 2; i >= 0; i--)
            {
                if (A[i] < A[i + 1])
                {
                    int smallestGreaterThanI = i + 1;
                    //found the element to start. 
                    //find the smallest element > i
                    for (int j = i + 1; j < N; j++)
                    {
                        //check its smaller than the smallest but still greater than I. 
                        if (A[j] < A[smallestGreaterThanI] && A[j] > A[i])
                            smallestGreaterThanI = j;
                    }

                    //swap it
                    swap(i, smallestGreaterThanI);

                    //sort the rest of the array
                    int unsortedLength = N - (i + 1);
                    Array.Sort(A, i + 1, unsortedLength);
                    foundPermutation = true;
                    break;
                }
            }

            if (foundPermutation)
            {
                //we have a permutation
                //compare it to current smallest 
                string currentNumber = "";
                //reconstruct the number
                for (int i = 0; i < N; i++)
                {
                    currentNumber += A[i];
                }

                if (currentNumber.Equals(Original))
                    Console.Write("BIGGEST");
                else
                    Console.Write(currentNumber);
            }
            else
            {
                Console.Write("BIGGEST");
            }

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
