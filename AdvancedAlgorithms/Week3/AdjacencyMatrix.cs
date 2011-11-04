using System;

namespace AdvancedAlgorithms
{
    /// <summary>
    /// Rod Howarth - n6294685
    /// Team14
    /// </summary>
    public class AdjacencyMatrix
    {
        private const int NOT_INITIALIZED = -1;
        private static void Main(string[] args)
        {
            int numTestCases = int.Parse(Console.ReadLine());

            for (int caseNumber = 0; caseNumber < numTestCases; caseNumber++)
            {
                string line = Console.ReadLine();
                //read dimensionss
                int n = int.Parse(line.Split(' ')[0]); ; //size of graph
                int m = int.Parse(line.Split(' ')[1]); ;//number of edges


                //initialize the graph
                int[][] graph = new int[n][];
                for (int i = 0; i < n; i++)
                {
                    graph[i] = new int[n];

                }

                for (int edgeNumber = 0; edgeNumber < m; edgeNumber++)
                {
                    string edgeLine = Console.ReadLine();
                    int edgeStart = int.Parse(edgeLine.Split(' ')[0]);
                    int edgeEnd = int.Parse(edgeLine.Split(' ')[1]);
                    int weight = int.Parse(edgeLine.Split(' ')[2]);

                    //add to graph
                    graph[edgeStart][edgeEnd] = weight;
                }

                //got the graph, now output it
                Console.WriteLine(caseNumber + 1);

                //output first line (headings)
                string firstLine = String.Format("{0,4} ", "");
                for (int i = 0; i < n; i++)
                {
                    firstLine += String.Format("{0,4} ", i);
                }
                Console.WriteLine(firstLine);

                //output each item in grid
                for (int y = 0; y < n; y++)
                {
                    //add the line number heading
                    string lineToOutput =
                        String.Format("{0,4} ", y);

                    //add each value
                    for (int x = 0; x < n; x++)
                    {
                        lineToOutput += String.Format("{0,4} ", graph[y][x]);
                    }
                    Console.WriteLine(lineToOutput);
                }

            }


        }
    }
}