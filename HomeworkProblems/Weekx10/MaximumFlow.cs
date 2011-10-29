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
    public class MaximumFlow
    {
        private const int SENTINEL = -1;
        private static int[,] matrix;
        private static void Main(string[] args)
        {
            int numTestCases = int.Parse(Console.ReadLine());
            for (int testCaseNum = 0; testCaseNum < numTestCases; testCaseNum++)
            {
                var ints = GetSplitInts();
                int numVertices = ints[0];
                int numEdges = ints[1];
                int source = ints[2];
                int sink = ints[3];

                matrix = new int[numVertices, numVertices];
                //set to no path
                for (int vertice1 = 0; vertice1 < numVertices; vertice1++)
                {
                    for (int vertice2 = 0; vertice2 < numVertices; vertice2++)
                    {
                        matrix[vertice1, vertice2] = -1;
                    }
                }


                for (int edgeNumber = 0; edgeNumber < numEdges; edgeNumber++)
                {
                    var line = GetSplitInts();
                    int sourceVertex = line[0];
                    int destinationVertex = line[1];
                    int capacity = line[2];

                    matrix[sourceVertex, destinationVertex] = capacity;
                }

                Console.WriteLine("Test {0}: Maximum flow = {1}", (testCaseNum + 1),
                    GetMaximumFlow(source, sink, numVertices));
            }
        }

        private static int GetMaximumFlow(int source, int sink, int numVertices)
        {

            int result = 0; //the resulting maximum flow
            bool augPathFound = true;

            //keep looping until we can't get an augmenting path anymore.
            while (augPathFound)
            {
                augPathFound = false;
                //find a new augmenting path and get its capacity
                int pathCapacity = FindPath(source, sink, numVertices);
                if (pathCapacity > 0)
                    augPathFound = true;

                result += pathCapacity;
            }

            return result;
        }

        //Find an augmenting path
        //update the global matrix with capacities
        private static int FindPath(int source, int sink, int numVertices)
        {
            Queue<int> queue = new Queue<int>();
            bool[] visited = new bool[numVertices];
            int[] from = new int[numVertices];
            for (int i = 0; i < numVertices; i++)
                from[i] = SENTINEL;


            //push source to queue and mark visited
            queue.Enqueue(source);
            visited[source] = true;

            //do the breadth first search to create a path
            while (queue.Count > 0)
            {
                int current = queue.Dequeue();
                bool foundEnd = false;
                for (int adj = 0; adj < numVertices; adj++)
                {
                    //skip current one 
                    if (adj == current)
                        continue;

                    //if no capacity, skip over
                    if (matrix[current, adj] <= 0)
                        continue;

                    //if its not visited and has capacity
                    //enque it and mark visited
                    if (!visited[adj])
                    {
                        queue.Enqueue(adj);
                        visited[adj] = true;
                        from[adj] = current;
                    }

                    if (adj == sink)
                    {
                        //exit outer loop
                        foundEnd = true;
                        break;
                    }
                }
                if (foundEnd)
                    break;
            }




            // compute path capacity – walk back along path
            int curr = sink;
            int prev;
            int pathCap = int.MaxValue;

            //capacity is the minimum value in the path
            while (from[curr] != SENTINEL)
            {
                prev = from[curr];
                pathCap = Math.Min(pathCap, matrix[prev, curr]);
                curr = prev;
            }

            //update the residual network
            //so taht we can check in future for another augmenting path
            // another augmenting path = more flow that can fit through network. 
            curr = sink;
            //while there is a path
            while (from[curr] != SENTINEL)
            {
                //get the previous node
                prev = from[curr];
                //take the capacity away from the forward path
                matrix[prev, curr] -= pathCap;
                //add the flow going through to the backward path.
                matrix[curr, prev] += pathCap;
                curr = prev;
            }

            if (pathCap == int.MaxValue)
                pathCap = 0;
            return pathCap;
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

