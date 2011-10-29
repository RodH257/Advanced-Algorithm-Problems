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
    public class MinCostMaxFlow
    {
        private const int INFINITY = int.MaxValue / 2;
        private static int[,] capacities;
        private static int[,] flow;
        private static int[,] costs;

        private static void Main(string[] args)
        {
            int numTestCases = int.Parse(Console.ReadLine());
            for (int testCaseNum = 1; testCaseNum <= numTestCases; testCaseNum++)
            {

                //read the initial data
                var firstLine = GetSplitInts();
                int numVertices = firstLine[0];
                int numEdges = firstLine[1];
                int sourceVertex = firstLine[2];
                int sinkVertex = firstLine[3];

                capacities = new int[numVertices, numVertices];
                costs = new int[numVertices, numVertices];
                flow = new int[numVertices, numVertices];
                //set to no path
                for (int vertice1 = 0; vertice1 < numVertices; vertice1++)
                {
                    for (int vertice2 = 0; vertice2 < numVertices; vertice2++)
                    {
                        capacities[vertice1, vertice2] = 0;
                        costs[vertice1, vertice2] = INFINITY;
                    }
                    //set diagonal costs to 0
                    costs[vertice1, vertice1] = 0;
                }

                //read all the edges
                for (int edgeNumber = 0; edgeNumber < numEdges; edgeNumber++)
                {
                    var edgeInts = GetSplitInts();

                    int edgeSource = edgeInts[0];
                    int edgeDest = edgeInts[1];
                    int capacity = edgeInts[2];
                    int cost = edgeInts[3];

                    //capacities are directed
                    capacities[edgeSource, edgeDest] = capacity;

                    //costs undirected
                    costs[edgeSource, edgeDest] = cost;
                    costs[edgeDest, edgeSource] = cost;
                }

                int minCost = GetMinCostMaximumFlow(sourceVertex, sinkVertex, numVertices);
                Console.WriteLine("Test {0}: Minimum Cost = {1}", testCaseNum, minCost);

            }
        }

        private static int GetMinCostMaximumFlow(int source, int sink, int numVertices)
        {
            int totalFlow = 0;
            int cost = 0;
            bool augPathFound = true;
            //keep looping until we can't get an augmenting path anymore.
            while (augPathFound)
            {
                augPathFound = false;
                //find a new augmenting path and get its capacity
                int pathCapacity = FindPath(source, sink, numVertices);
                if (pathCapacity > 0)
                    augPathFound = true;

                totalFlow += pathCapacity;
            }

            //cost 
            for (int u = 0; u < numVertices; u++)
            {
                for (int v = 0; v < numVertices; v++)
                {
                    if (flow[u, v] > 0)
                    {
                        //Console.WriteLine("Flow " + u + " to " + v + " is " + flow[u,v]);
                        cost += flow[u, v] * costs[u, v];
                    }
                }
            }

            return cost;
        }

        //Find an augmenting path
        //update the global matrix with capacities
        private static int FindPath(int source, int sink, int numVertices)
        {

            EditablePriorityQueue<int, int> queue = new EditablePriorityQueue<int, int>();
            int[] parent = new int[numVertices];
            int[] leastCosts = new int[numVertices];
            //[] from = new int[numVertices]; //best path
            for (int nodeNumber = 0; nodeNumber < numVertices; nodeNumber++)
            {
                parent[nodeNumber] = INFINITY;

                int costFromSource = int.MaxValue;

                if (nodeNumber == source)
                {
                    costFromSource = 0;
                }
                else if (capacities[source, nodeNumber] - flow[source, nodeNumber] > 0)
                {
                    //there is forward capacity straight from the source node
                    costFromSource = costs[source, nodeNumber];
                    parent[nodeNumber] = source;
                    queue.Enqueue(costFromSource, nodeNumber);
                }

                //save least cost from source to currnet node
                leastCosts[nodeNumber] = costFromSource;
            }

            //mark it processed
            while (!queue.IsEmpty)
            {
                int currentNode = queue.Dequeue();
                if (currentNode == sink)
                    break;

                //  queue.Enqueue(leastCosts[currentNode], currentNode);
                for (int i = 0; i < numVertices; i++)
                {
                    //if no capacity skip this node
                    if (capacities[currentNode, i] - flow[currentNode, i] <= 0)
                        continue;

                    int potential = leastCosts[currentNode];

                    //if there is no flow, remove the cost of that section 
                    //otherwise add it 
                    if (flow[currentNode, i] < 0)
                        //remove the cost of this leg from the potential cost
                        potential -= costs[currentNode, i];
                    else
                        //add to the cost as we will be using this leg
                        potential += costs[currentNode, i];

                    //see if adjNode woudl be cheaper if it went via current
                    if (potential < leastCosts[i])
                    {
                        int oldCost = leastCosts[i];
                        //update the cost
                        leastCosts[i] = potential;
                        //update the path
                        parent[i] = currentNode;
                        //add adjacent node to priority queue
                        queue.AddOrUpdate(oldCost, leastCosts[i], i);
                    }
                }
            }

            // compute path capacity – walk back along path
            int curr = sink;
            int prev;
            int pathCap = int.MaxValue;
            //Console.WriteLine("Walking path to get capacity...");
            //capacity is the minimum value in the path
            while (parent[curr] != INFINITY)
            {
                prev = parent[curr];
                //get residual capacity
          
                int residual = capacities[prev, curr] - flow[prev, curr];
                pathCap = Math.Min(pathCap, residual);
                //Console.WriteLine(curr + " - " + prev + " | " + pathCap);
                curr = prev;

            }
            //Console.WriteLine("Total Pathcap " + pathCap);

            if (pathCap == int.MaxValue)
                pathCap = 0;

          
            //update the residual network
            //so taht we can check in future for another augmenting path
            // another augmenting path = more flow that can fit through network. 
            curr = sink;
            //while there is a path
            while (parent[curr] != INFINITY)
            {
                //get the previous node
                prev = parent[curr];
                //add the flow to the forward edge
                flow[prev, curr] += pathCap;
                //take it away from backward edge
                flow[curr, prev] -= pathCap;
        
                //Console.WriteLine("Flow from " + prev + " to " + curr + " is " + flow[prev,curr]);
                curr = prev;
            }

          
            return pathCap;
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

