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
    public class AntiBruteForceLock
    {
        private static void Main(string[] args)
        {
            int numTestCases = int.Parse(Console.ReadLine());
            for (int testCaseNum = 1; testCaseNum <= numTestCases; testCaseNum++)
            {
                var caseLine = Console.ReadLine().Split(' ');


                int numCombinations = int.Parse(caseLine[0]);
                List<Node> allNodes = new List<Node>();

                Node zeroNode = null;
                for (int i =1; i <= numCombinations; i++)
                {
                    string combination = caseLine[i];
                 
                    Node node = new Node();
                    node.Combination = combination;
                    allNodes.Add(node);
                    if (combination.Equals("0000"))
                    {
                        zeroNode = node;
                    }
                }

                //if zero isnt already there, add it
                if (zeroNode == null)
                {
                    zeroNode = new Node();
                    zeroNode.Combination = "0000";
                    zeroNode.AddedArtificially = true;
                    allNodes.Add(zeroNode);
                }

                //all nodes added, need to calculate the scroll differences between them all and add as adjacent nodes
                foreach (Node node in allNodes)
                {
                    node.AddAdjacents(allNodes);
                }

                //graph created, get minimum spanning tree weight 
                int weight = GetMinimumSpanningTreeWeight(allNodes, zeroNode);
                Console.WriteLine("{0}", weight);

            }
        }

        private static int GetMinimumSpanningTreeWeight(IList<Node> nodes, Node startNode )
        {
            startNode.IsInTree = true;
            PriorityQueue<int, Edge> queue = new PriorityQueue<int, Edge>();

            foreach (Edge edge in startNode.Edges)
            {
                queue.Enqueue(edge.Weight, edge); 
            }

            int numberInTree = 0;
            int totalWeight = 0;

            while (!queue.IsEmpty && numberInTree != nodes.Count)
            {
                Edge currentEdge = queue.Dequeue();
                Node nodeToAdd = null;
                if (currentEdge.Source.IsInTree && currentEdge.Destination.IsInTree)
                    continue;
                if (currentEdge.Source.IsInTree)
                    nodeToAdd = currentEdge.Destination;
                else
                    continue;

                //check we arne't jumping back to 0000 if we just added it artificially
                if (currentEdge.Source.Combination == "0000" && currentEdge.Source.AddedArtificially && numberInTree != 0)
                    continue;

                nodeToAdd.IsInTree = true;
                totalWeight += currentEdge.Weight;
                numberInTree++;
               // Console.WriteLine("Added  " +nodeToAdd.Combination + " to tree" + totalWeight + " due to weight " + currentEdge.Weight);
                foreach (Edge edge in nodeToAdd.Edges)
                {
                    queue.Enqueue(edge.Weight, edge); 
                }
            }

            return totalWeight; 
        }


        public class Edge
        {
            public Node Source;
            public Node Destination;
            public int Weight;
        }
        public class Node
        {
            public string Combination;
            public IList<Edge> Edges = new List<Edge>();
            public bool IsInTree = false;
            public bool AddedArtificially = false;
            public void AddAdjacents(List<Node> nodes )
            {
                foreach (Node node in nodes)
                {
                    if (node.Combination == this.Combination)
                    {
                        //same node skip
                        continue;
                    }

                    int totalDiff = 0;
                    //compare combination
                    for (int i =0; i < Combination.Length; i++)
                    {
                        int currentInt = int.Parse(this.Combination[i].ToString());
                        int otherInt = int.Parse(node.Combination[i].ToString());

                        int diff = Math.Abs(currentInt - otherInt);

                        //if its > 5 difference, go via 0. 
                        if (diff > 5)
                            diff = 10 - diff;

                        totalDiff += diff;
                    }

                    Edge edge = new Edge();
                    edge.Source = this;
                    edge.Destination = node;
                    edge.Weight = totalDiff;
                    this.Edges.Add(edge);
                   // Console.WriteLine("Edge from " + edge.Source.Combination + " to " + edge.Destination.Combination + " weight " + edge.Weight);
                }
            }

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

