using System;
using System.Linq;

namespace HomeworkProblems
{
    /// <summary>
    /// Rod Howarth - n6294685
    /// Team14
    /// </summary>
    public class KnapSack
    {
        private static void Main(string[] args)
        {
            int numTestCases = int.Parse(Console.ReadLine());

            for (int testCase = 0; testCase < numTestCases; testCase++)
            {
                int vehicleCapacity = int.Parse(Console.ReadLine());
                int numItems = int.Parse(Console.ReadLine());

                //parse the weights and values
                string[] weightsStrings = Console.ReadLine().Split(' ');
                string[] valuesStrings = Console.ReadLine().Split(' ');

                int[] weights = new int[numItems+1];
                int[] values = new int[numItems+1];
                for (int itemNumber = 1; itemNumber <= numItems; itemNumber++)
                    weights[itemNumber] = int.Parse(weightsStrings[itemNumber-1]);

                for (int itemNumber = 1; itemNumber <= numItems; itemNumber++)
                    values[itemNumber] = int.Parse(valuesStrings[itemNumber-1]);

                Console.WriteLine(GetMaximumValueOfLoad(vehicleCapacity, numItems, weights, values));
            }
        }

        private static int GetMaximumValueOfLoad(int vehicleCapacity, int numItems, int[] weights, int[] values)
        {
            int[][] optimalValues = new int[numItems+1][];

            for (int i = 0; i <= numItems; i++)
            {
                //create an array to hold up to the vehicle capacity
                optimalValues[i] = new int[vehicleCapacity+1];
            }

            for (int j = 1; j <= vehicleCapacity; j++)
            {
                for (int i = 1; i <= numItems; i++)
                {
                    int currentWeight = weights[i];
                    int currentValue = values[i];
                    int previousOptimalValue = optimalValues[i - 1][j];
                    int spaceLeft = j - currentWeight;

                    if (spaceLeft < 0)
                    {
                        //too heavy, use last value 
                        optimalValues[i][j] = previousOptimalValue;
                    } else
                    {
                        //there is space to fit this new value in.
                        //see if its better to keep the old one, or add the value of this one, to whatever the value of 
                        // the one that fits into the left over space is. 

                        int[] options = { previousOptimalValue, optimalValues[i - 1][spaceLeft] + currentValue };
                        optimalValues[i][j] = options.Max();
                    }
                }
                    
            }

            return optimalValues[numItems][vehicleCapacity];
        }
    }
}