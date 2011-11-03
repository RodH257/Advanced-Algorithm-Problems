using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeworkProblems.Week1
{
    /// <summary>
    /// Rod Howarth - n6294685
    /// </summary>
    public class GreatestCommonDenominator
    {
        private const int NOT_INITIALIZED = -1;
        private static void Main(string[] args)
        {

            int testsToRun = int.Parse(Console.ReadLine());

            for (int testNumber = 0; testNumber < testsToRun; testNumber++)
            {
                string line = Console.ReadLine();

                int m = NOT_INITIALIZED;
                int n = NOT_INITIALIZED;
                foreach (string value in line.Split(','))
                {
                    if (m == NOT_INITIALIZED)
                        m = int.Parse(value);
                    else
                        n = int.Parse(value.Replace(" ", ""));
                }

                Console.WriteLine(Week1_GreatestCommonDivisor(m, n));
            }

        }

        /// <summary>
        /// Uses Euclids algorithm
        /// </summary>
        static int Week1_GreatestCommonDivisor(int a, int b)
        {
            if (b == 0)
                return a;
            return Week1_GreatestCommonDivisor(b, a % b);
        }
    }
}
