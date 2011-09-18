using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeworkProblems
{
    /// <summary>
    /// Rod Howarth - n6294685
    /// Team14
    /// </summary>
    public class ShortestPathProblem
    {
        private const int NO_PATH = int.MaxValue;
        private static void Main(string[] args)
        {
            int numTestCases = int.Parse(Console.ReadLine());
            for (int testCaseNum = 0; testCaseNum < numTestCases; testCaseNum++)
            {
                string[] initValues = Console.ReadLine().Split(' ');
                int n = int.Parse(initValues[0]);
                int numberEdges = int.Parse(initValues[1]);
                int sourceNode = int.Parse(initValues[2]);

                //initialize the graph
                int[][] graph = new int[n][];
                for (int i = 0; i < n; i++)
                {
                    graph[i] = new int[n];

                    for (int x = 0; x < n; x++)
                    {
                        graph[i][x] = NO_PATH;
                        if (x == i)
                            graph[i][x] = 0;
                    }
                }

                for (int edgeNumber = 0; edgeNumber < numberEdges; edgeNumber++)
                {
                    string[] edgeStrings = Console.ReadLine().Split(' ');
                    int edgeStart = int.Parse(edgeStrings[0]);
                    int edgeEnd = int.Parse(edgeStrings[1]);
                    int weight = int.Parse(edgeStrings[2]);
                    graph[edgeStart][edgeEnd] = weight;
                }

                int[] shortestPathWeights = GetShortestPathWeights(n, sourceNode, graph);

                Console.WriteLine((testCaseNum + 1));
                foreach (int weight in shortestPathWeights)
                {
                    if (weight == NO_PATH)
                        Console.Write("NP");
                    else
                        Console.Write(weight);

                    Console.Write(" ");
                }
                Console.WriteLine();
            }

        }

        public static int[] GetShortestPathWeights(int n, int sourceNode, int[][] graph)
        {

            HashSet<int> processed = new HashSet<int>();

            //queue with WEIGHT_FROM_S and NODE_NUMBER
            EditablePriorityQueue<int, int> queue = new EditablePriorityQueue<int, int>();

            for (int nodeNumber = 0; nodeNumber < n; nodeNumber++)
            {
                if (nodeNumber != sourceNode)
                    queue.Enqueue(graph[sourceNode][nodeNumber], nodeNumber);
            }

            //mark it processed
            processed.Add(sourceNode);

            while (!queue.IsEmpty)
            {
                int nodeNumber = queue.Dequeue();
                //mark processed
                processed.Add(nodeNumber);

                for (int adjNode = 0; adjNode < n; adjNode++)
                {
                    //same 
                    if (adjNode == nodeNumber || processed.Contains(adjNode))
                        continue;

                    //check that its actually adjacent
                    if (graph[nodeNumber][adjNode] == NO_PATH)
                        continue;

                    //ensure you can get from the source node
                    if (graph[sourceNode][nodeNumber] == NO_PATH)
                        continue;

                    //check if there is a path to the adjacent node VIA the current node 
                    int existingPath = graph[sourceNode][adjNode];

                    //path via current node to adjacent 
                    int viaCurrent = graph[sourceNode][nodeNumber] + graph[nodeNumber][adjNode];

                    if (existingPath == NO_PATH || viaCurrent < existingPath)
                    {
                        int oldPriority = graph[sourceNode][adjNode];
                        graph[sourceNode][adjNode] = viaCurrent;
                        queue.AddOrUpdate(oldPriority, graph[sourceNode][adjNode], adjNode);
                    }
                }
            }
            return graph[sourceNode];
        }

        class EditablePriorityQueue<P, V>
        {
            private SortedDictionary<P, Queue<V>> list = new SortedDictionary<P, Queue<V>>();
            public void Enqueue(P priority, V value)
            {
                Queue<V> q;
                if (!list.TryGetValue(priority, out q))
                {
                    q = new Queue<V>();
                    list.Add(priority, q);
                }


                q.Enqueue(value);
            }

            public void AddOrUpdate(P oldPriority, P newPriority, V value)
            {
                Queue<V> q;
                //find and remove the current item in the queue
                //by dequeuing and requeing all
                if (list.TryGetValue(oldPriority, out q))
                {
                    Queue<V> newQueue = new Queue<V>();

                    while (q.Count != 0)
                    {
                        V current = q.Dequeue();
                        if (!current.Equals(value))
                            newQueue.Enqueue(current);
                    }
                    list.Remove(oldPriority);
                    if (newQueue.Count != 0)
                        list.Add(oldPriority, newQueue);
                }
                //removed him, now we can enqueue it 
                Enqueue(newPriority, value);
            }

            public V Dequeue()
            {
                // will throw if there isn’t any first element!
                var pair = list.First();
                var v = pair.Value.Dequeue();
                if (pair.Value.Count == 0) // nothing left of the top priority.
                    list.Remove(pair.Key);
                return v;
            }
            public bool IsEmpty
            {
                get { return !list.Any(); }
            }
        }

    }



}
