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
    public class TravellingSalesman
    {
        private static void Main(string[] args)
        {
            int numTestCases = int.Parse(Console.ReadLine());
            for (int testCaseNum = 1; testCaseNum <= numTestCases; testCaseNum++)
            {
                int numPoints = GetSplitInts()[0];

                //initialize nodes
                Node[] nodes = new Node[numPoints];
                for (int i = 0; i < numPoints; i++)
                {
                    nodes[i] = new Node();
                }

                for (int nodeNum = 0; nodeNum < numPoints; nodeNum++)
                {
                    var line = GetSplitInts();
                    int x = line[0];
                    int y = line[1];

                    Node node = new Node() { X = x, Y = y, NodeNumber = nodeNum };
                    nodes[nodeNum] = node;
                }

                //all nodes added, add edges between them 
                for (int nodeNum = 0; nodeNum < numPoints; nodeNum++)
                {
                    for (int otherCity = 0; otherCity < numPoints; otherCity++)
                    {
                        //work out euclidean distance
                        double weight = GetEucDistance(nodes[nodeNum], nodes[otherCity]);

                        Edge edge = new Edge() { Source = nodes[nodeNum], Destination = nodes[otherCity] };
                        edge.Weight = weight;
                        nodes[nodeNum].Edges.Add(edge);
                    }
                }

                Console.Write("{0}: ", testCaseNum);
                //get the minimum spanning tree and its weight
                double totalWeight = GetMinimumSpanningTreeWeightWithRTS(nodes, numPoints);

                Node currentNode = nodes[0];
                bool first = true;
                while (true)
                {
                    Console.Write(currentNode.NodeNumber + " ");

                    //if its the second time we've reached home, stop here
                    if (currentNode == nodes[0] && !first)
                        break;

                    first = false;
                    currentNode = currentNode.NextNodeInTree;
                }

                //round it up as per example.
                totalWeight = Math.Ceiling(totalWeight);

                Console.WriteLine("= {0,0}",totalWeight);
            }
        }

        static double GetEucDistance(Node start, Node destination)
        {
            double xDiff2 = Math.Pow(start.X - destination.X, 2.0);
            double yDiff2 = Math.Pow(start.Y - destination.Y, 2.0);
            return Math.Sqrt(yDiff2 + xDiff2);
        }

        private class Node
        {
            public List<Edge> Edges = new List<Edge>();
            public int NodeNumber = 0;
            public int X;
            public int Y;
            public bool IsInTree = false;
            public Node NextNodeInTree = null;
            public Edge EdgeToNextNode = null;
        }

        private class Edge
        {
            public Node Source;
            public Node Destination;
            public double Weight;
        }

        /// <summary>
        /// Modified to return to start
        /// </summary>
        private static double GetMinimumSpanningTreeWeightWithRTS(Node[] nodes, int n)
        {
            Node startNode = nodes[0];
            startNode.IsInTree = true;
            PriorityQueue<double, Edge> queue = new PriorityQueue<double, Edge>();

            foreach (Edge edge in startNode.Edges)
                queue.Enqueue(edge.Weight, edge);

            int numberInTree = 0;
            double totalWeight = 0;

            Node lastNode = startNode;
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


                //set the path
                lastNode.NextNodeInTree = nodeToAdd;
                lastNode.EdgeToNextNode = currentEdge;

                lastNode = nodeToAdd;

                foreach (Edge edge in nodeToAdd.Edges)
                    queue.Enqueue(edge.Weight, edge);
            }

            lastNode.NextNodeInTree = startNode;

            //add return to start weight
            foreach (Edge edge in lastNode.Edges)
            {
                if (edge.Destination == startNode)
                {
                    totalWeight += edge.Weight;
                    lastNode.EdgeToNextNode = edge;
                    break;
                }
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

