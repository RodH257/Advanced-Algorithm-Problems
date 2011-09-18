using System;
using System.Collections.Generic;

namespace HomeworkProblems
{
    /// <summary>
    /// Rod Howarth - n6294685
    /// Team14
    /// </summary>
    public class GraphTraversal
    {
        private const int NOT_INITIALIZED = -1;
        private const int HAS_EDGE = 1;
        private static void Main(string[] args)
        {

            int numTestCases = int.Parse(Console.ReadLine());

            for (int caseNumber = 0; caseNumber < numTestCases; caseNumber++)
            {
                string line = Console.ReadLine();
                //read dimensions
                int n = int.Parse(line.Split(' ')[0]);  //size of graph
                int m = int.Parse(line.Split(' ')[1]); //number of edges

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

                    //add to graph
                    graph[edgeStart][edgeEnd] = HAS_EDGE;
                }

                //got the graph, now output it
                Console.WriteLine(caseNumber + 1);

                BreadthFirst(graph, n);
                Console.Write(Environment.NewLine);
                DepthFirst(graph, n);
                Console.Write(Environment.NewLine);
            }

        }


        private static void BreadthFirst(int[][] graph, int n)
        {
            Queue<int> nodes = new Queue<int>();
            nodes.Enqueue(0);
            bool[] visitedNodes = new bool[n];

            bool firstNode = true;
            while (nodes.Count != 0)
            {
                int currentNode = nodes.Dequeue();

                if (!firstNode)
                    Console.Write(", ");
                Console.Write(currentNode);

                visitedNodes[currentNode] = true;

                for (int i = 0; i < n; i++)
                {
                    int edge = graph[currentNode][i];
                    int reverseEdge = graph[i][currentNode];
                    //check there is an edge between them
                    if (edge != HAS_EDGE && reverseEdge != HAS_EDGE)
                        continue;

                    //check if its visited
                    if (visitedNodes[i])
                        continue;

                    //check if its in queue
                    if (!nodes.Contains(i))
                    {
                        nodes.Enqueue(i);
                    }
                }

                firstNode = false;
            }
        }

        private static void DepthFirst(int[][] graph, int n)
        {
            Stack<int> nodes = new Stack<int>();
            nodes.Push(0);
            bool[] visitedNodes = new bool[n];

            bool firstNode = true;
            while (nodes.Count != 0)
            {
                int currentNode = nodes.Pop();

                if (!firstNode)
                    Console.Write(", ");
                Console.Write(currentNode);

                visitedNodes[currentNode] = true;

                for (int i = 0; i < n; i++)
                {
                    int edge = graph[currentNode][i];
                    int reverseEdge = graph[i][currentNode];
                    //check there is an edge between them
                    if (edge != HAS_EDGE && reverseEdge != HAS_EDGE)
                        continue;

                    //check if its visited
                    if (visitedNodes[i])
                        continue;

                    //check if its in queue
                    if (!nodes.Contains(i))
                    {
                        nodes.Push(i);
                    }
                }

                firstNode = false;
            }
        }
    }
}