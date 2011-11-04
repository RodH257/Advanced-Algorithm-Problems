using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdvancedAlgorithms
{
    /// <summary>
    /// Rod Howarth - n6294685
    /// Team14
    /// </summary>
    public class GetConnected
    {
        private static void Main(string[] args)
        {
            int numTestCases = int.Parse(Console.ReadLine());
            for (int testCaseNum = 0; testCaseNum < numTestCases; testCaseNum++)
            {
                int numIslands = int.Parse(Console.ReadLine());

                Node[] nodes = new Node[numIslands];
                for (int i = 0; i < numIslands; i++)
                {
                    nodes[i] = new Node();
                    nodes[i].Edges = new List<Edge>();

                    string[] coords = Console.ReadLine().Split(' ');
                    int x = int.Parse(coords[0]);
                    int y = int.Parse(coords[1]);
                    nodes[i].X = x;
                    nodes[i].Y = y;
                }

                //initialize edges
                for (int islandNumber = 0; islandNumber < numIslands; islandNumber++)
                {
                    Node source = nodes[islandNumber];

                    for (int destinationNumber = 0; destinationNumber < numIslands; destinationNumber++)
                    {
                        if (islandNumber == destinationNumber)
                            continue;
                        Node destination = nodes[destinationNumber];
                        Edge edge = new Edge()
                        {
                            Destination = destination,
                            Source = source,
                            Weight = GetEucDistance(source, destination)
                        };
                        source.Edges.Add(edge);
                    }
                }
                double minimumWeight = GetMinimumSpanningTreeWeight(nodes, numIslands);
                Console.WriteLine("Island Group {0} is connected in {1:#.00} days", (testCaseNum+1), minimumWeight);
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
            public List<Edge> Edges;
            public int X;
            public int Y;
            public bool IsInTree = false;
        }

        private class Edge
        {
            public Node Source;
            public Node Destination;
            public double Weight;
        }

        private static double GetMinimumSpanningTreeWeight(Node[] nodes, int n)
        {
            Node startNode = nodes[0];
            startNode.IsInTree = true;
            PriorityQueue<double, Edge> queue = new PriorityQueue<double, Edge>();

            foreach (Edge edge in startNode.Edges)
                queue.Enqueue(edge.Weight, edge);

            int numberInTree = 0;
            //double totalWeight = 0.0;
            double max = 0;
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
             //   totalWeight += currentEdge.Weight;
                numberInTree++;

                //we want the max
                if (currentEdge.Weight > max)
                    max = currentEdge.Weight;

                foreach (Edge edge in nodeToAdd.Edges)
                    queue.Enqueue(edge.Weight, edge);
            }
            return max;
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
