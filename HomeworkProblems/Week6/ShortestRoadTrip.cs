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
    public class ShortestRoadTrip
    {
        private static void Main(string[] args)
        {
            int numTestCases = int.Parse(Console.ReadLine());
            for (int testCaseNum = 0; testCaseNum < numTestCases; testCaseNum++)
            {
                string[] firstInput = Console.ReadLine().Split(' ');
                int numberOfTowns = int.Parse(firstInput[0]);
                int numberOfRoads = int.Parse(firstInput[1]);
                int start = int.Parse(firstInput[2]);
                int destination = int.Parse(firstInput[3]);

                GraphNode[] nodes = new GraphNode[numberOfTowns];

                for (int town = 0; town < numberOfTowns; town++)
                {
                    string[] townInput = Console.ReadLine().Split(' ');

                    int townX = int.Parse(townInput[0]);
                    int townY = int.Parse(townInput[1]);
                    nodes[town] = new GraphNode(townX, townY, town);
                }

                for (int road = 0; road < numberOfRoads; road++)
                {
                    string[] roadInput = Console.ReadLine().Split(' ');

                    int roadStart = int.Parse(roadInput[0]);
                    int roadEnd = int.Parse(roadInput[1]);
                    nodes[roadStart].AddAdjacentNode(nodes[roadEnd]);
                    nodes[roadEnd].AddAdjacentNode(nodes[roadStart]);
                }

                //now we have our graph setup, find the shortest distance
                double shortestDistance = GetShortestDistance(nodes,  start, destination);

                Console.Write("Road Trip " + (testCaseNum + 1) + ": ");
                if (shortestDistance < 0)
                    Console.Write("IMPOSSIBLE");
                else
                    Console.Write("{0:0.00}", shortestDistance);

                Console.WriteLine();

            }
        }

        /// <summary>
        /// Uses A star to find shortest distance sfrom start to destination
        /// </summary>
        private static double GetShortestDistance(GraphNode[] nodes, int start, int destination)
        {
            //F score is weighting on queue
            EditablePriorityQueue<double, GraphNode> queue = new EditablePriorityQueue<double, GraphNode>();
            HashSet<int> processed = new HashSet<int>();

            //initialize the first element
            nodes[start].HScore = GetEucDistance(nodes[start], nodes[destination]);
            //enque the start
            queue.Enqueue(nodes[start].FScore,nodes[start]);

            while (!queue.IsEmpty)
            {
                GraphNode current = queue.Dequeue();

                //if we've dequeued the destination, by nature of the priority queue, we've found our result.
                if (current.NodeNumber == destination)
                {
                    //we're there
                    return current.GScore;
                }

                processed.Add(current.NodeNumber);

                //look through its adjacent nodes to find paths
                foreach (GraphNode adjNode in current.AdjacentNodes)
                {
                    if (!processed.Contains(adjNode.NodeNumber))
                    {
                        //calculate running G score from start to adjacent via current
                        double gScore = current.GScore + GetEucDistance(current, adjNode);

                        //if we havent queued it for visiting, or if we've found a quicker path
                        if (!queue.Contains(adjNode.FScore, adjNode) || gScore < adjNode.GScore)
                        {
                            //update the scores and queue.
                            double oldFScore = adjNode.FScore;
                            adjNode.GScore = gScore;
                            adjNode.HScore = GetEucDistance(adjNode, nodes[destination]);
                            queue.AddOrUpdate(oldFScore, adjNode.FScore, adjNode);
                        }
                    }
                }
            }
            
            return -1;
        }


        static double GetEucDistance(GraphNode start, GraphNode destination)
        {
            double xDiff2 = Math.Pow(start.X - destination.X, 2.0);
            double yDiff2 = Math.Pow(start.Y - destination.Y, 2.0);
            return Math.Sqrt(yDiff2 + xDiff2);
        }


        /// <summary>
        /// Graph node for adjacency list
        /// </summary>
        private class GraphNode : IEquatable<GraphNode>
        {
            public LinkedList<GraphNode> AdjacentNodes = new LinkedList<GraphNode>();

            public int NodeNumber;
            public int X;
            public int Y;

            //cost from start along best path
            public double GScore;

            //estimate from current to the goal
            public double HScore;

            //H+G
            public double FScore { get { return GScore + HScore; } }

            public GraphNode(int x, int y, int nodeNumber)
            {
                this.X = x;
                this.Y = y;
                this.NodeNumber = nodeNumber;
            }

            public void AddAdjacentNode(GraphNode nodeToInsert)
            {
                LinkedListNode<GraphNode> currentNode = AdjacentNodes.First;

                if (currentNode == null)
                {
                    AdjacentNodes.AddFirst(nodeToInsert);
                    return;
                }
                while (currentNode != null)
                {
                    //add it before that
                    if (currentNode.Value.NodeNumber > nodeToInsert.NodeNumber)
                    {
                        AdjacentNodes.AddBefore(currentNode, nodeToInsert);
                        return;
                    }
                    currentNode = currentNode.Next;
                }
                AdjacentNodes.AddLast(nodeToInsert);
            }

            public bool Equals(GraphNode other)
            {
                return this.NodeNumber == other.NodeNumber;
            }
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

            public bool Contains(P priority, V value)
            {
                Queue<V> q;

                if (!list.TryGetValue(priority, out q))
                    return false;

                return q.Contains(value);
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
