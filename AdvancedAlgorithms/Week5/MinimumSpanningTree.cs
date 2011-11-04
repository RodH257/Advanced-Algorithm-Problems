using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdvancedAlgorithms
{
    /// <summary>
    /// Rod Howarth - n6294685
    /// Team14
    /// </summary>
    public class MinimumSpanningTree
    {
        private static void Main(string[] args)
        {

            int numTestCases = int.Parse(Console.ReadLine());
            for (int testCaseNum = 0; testCaseNum < numTestCases; testCaseNum++)
            {
                string[] nodesAndEdges = Console.ReadLine().Split(' ');
                int n = int.Parse(nodesAndEdges[0]);
                int e = int.Parse(nodesAndEdges[1]);

                Node[] nodes = new Node[n];
                for (int i = 0; i < n; i++)
                {
                    nodes[i] = new Node();
                    nodes[i].Edges = new List<Edge>();
                }

                for (int edgeNumber = 0; edgeNumber < e; edgeNumber++)
                {
                    string[] edgeStrings = Console.ReadLine().Split(' ');
                    int edgeStart = int.Parse(edgeStrings[0]);
                    int edgeEnd = int.Parse(edgeStrings[1]);
                    int weight = int.Parse(edgeStrings[2]);

                    Node source = nodes[edgeStart];
                    Node destination = nodes[edgeEnd];

                    Edge edge = new Edge()
                                    {
                                        Destination = destination,
                                        Source = source,
                                        Weight = weight
                                    };
                    source.Edges.Add(edge);
                    destination.Edges.Add(edge);
                }

                int minimumWeight = GetMinimumSpanningTreeWeight(nodes, n);
                Console.WriteLine("Test " + (testCaseNum + 1) + ", minimum spanning tree weight = " + minimumWeight);
            }
        }

        private class Node
        {
            public List<Edge> Edges;
            public bool IsInTree = false;
        }

        private class Edge
        {
            public Node Source;
            public Node Destination;
            public int Weight;
        }

    


        private static int GetMinimumSpanningTreeWeight(Node[] nodes, int n)
        {
            Node startNode = nodes[0];
            startNode.IsInTree = true;
            PriorityQueue<int, Edge> queue = new PriorityQueue<int, Edge>();

            foreach (Edge edge in startNode.Edges)
                queue.Enqueue(edge.Weight, edge);

            int numberInTree = 0;
            int totalWeight = 0;

            while (!queue.IsEmpty && numberInTree != n)
            {
                Edge currentEdge = queue.Dequeue();
                Node nodeToAdd = null;
                if (currentEdge.Source.IsInTree && currentEdge.Destination.IsInTree)
                    //all in tree, continue
                    continue;
                if (currentEdge.Source.IsInTree)
                    //source is in tree but not dest
                    nodeToAdd = currentEdge.Destination;
                else if (currentEdge.Destination.IsInTree)
                    //dest is in tree but not source
                    nodeToAdd = currentEdge.Source;
                else
                    continue; //neither in there

                nodeToAdd.IsInTree = true;
                totalWeight += currentEdge.Weight;
                numberInTree++;

                foreach (Edge edge in nodeToAdd.Edges)
                    queue.Enqueue(edge.Weight, edge);
            }
            return totalWeight;
        }

        class PriorityQueue<P, V>
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

