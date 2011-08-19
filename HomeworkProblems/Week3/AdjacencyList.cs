using System;
using System.Collections.Generic;

namespace HomeworkProblems
{
    /// <summary>
    /// Rod Howarth - n6294685
    /// </summary>
    public class AdjacencyList
    {
        private const int NOT_INITIALIZED = -1;

        private class GraphNode
        {
            public GraphNode FirstNode;
            public LinkedList<GraphNode> AdjacentNodes = new LinkedList<GraphNode>();

            public int NodeNumber;
            public GraphNode(int nodeNumber)
            {
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

        }
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
                GraphNode[] nodes = new GraphNode[n];
                for (int edgeNumber = 0; edgeNumber < m; edgeNumber++)
                {
                    string edgeLine = Console.ReadLine();
                    int edgeStart = int.Parse(edgeLine.Split(' ')[0]);
                    int edgeEnd = int.Parse(edgeLine.Split(' ')[1]);

                    if (nodes[edgeStart] == null)
                    {
                        //first entry
                        nodes[edgeStart] = new GraphNode(edgeStart);
                    }

                    if (nodes[edgeEnd] == null)
                    {
                        nodes[edgeEnd] = new GraphNode(edgeEnd);
                    }

                    nodes[edgeStart].AddAdjacentNode(nodes[edgeEnd]);
                    nodes[edgeEnd].AddAdjacentNode(nodes[edgeStart]);

                }

                //got the graph, now output it
                Console.WriteLine(caseNumber + 1);

                foreach (GraphNode node in nodes)
                {
                    string lineToOutput = string.Format("{0}: ", node.NodeNumber);
                    bool first = true;
                    foreach (GraphNode adjNode in node.AdjacentNodes)
                    {
                        if (!first)
                            lineToOutput += ", ";
                        lineToOutput += adjNode.NodeNumber;
                        first = false;
                    }

                    Console.WriteLine(lineToOutput);
                }

            }


        }
    }
}