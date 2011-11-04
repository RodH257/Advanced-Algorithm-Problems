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
    public class MaximumCardinalityBipartiteMatching
    {
        private static void Main(string[] args)
        {
            int numTestCases = int.Parse(Console.ReadLine());
            for (int testCaseNum = 0; testCaseNum < numTestCases; testCaseNum++)
            {
                var ints = GetSplitInts();
                int n = ints[0]; // number of virtices in one bipartition
                int m = ints[1]; //number of vertice in other bipartition
                int e = ints[2]; // number of edges between them

                int totalNodes = n + m;
                Node[] nodes = new Node[totalNodes];

                for (int i = 0; i < totalNodes; i++)
                {
                    nodes[i] = new Node() {Number = i};
         
                }


                for (int edgeNumber = 0; edgeNumber < e; edgeNumber++)
                {
                    var edgeInts = GetSplitInts();
                    int u = edgeInts[0];
                    int v = edgeInts[1];

                    Node start = nodes[u];
                    Node end = nodes[v];

                    start.AdjacentNodes.Add(end);
                    end.AdjacentNodes.Add(start);
                }


                //we have our graph
                int maximumCardinality = GetMaximumCardinality(nodes, n, m);
                Console.WriteLine("Test {0}: Maximum cardinality = {1}", (testCaseNum+1), maximumCardinality);
            }
        }

        private static int GetMaximumCardinality(Node[] nodes, int n, int m)
        {
            int cardinality = 0;

            for (int leftNodeNum = 0; leftNodeNum< n; leftNodeNum++)
            {
                Node leftNode = nodes[leftNodeNum];
    
                //set all the right vertexes to falses
                for (int rightNodeNum = n; rightNodeNum < nodes.Length; rightNodeNum++)
                {
                    nodes[rightNodeNum].Visited = false;
                }

                if (FindAugmentingPath(leftNode))
                {
                    cardinality++;
                }

            }
            return cardinality;
        }


        private static bool FindAugmentingPath(Node source)
        {
            foreach (Node dest in source.AdjacentNodes)
            {
                if (!dest.Visited )
                {
                    dest.Visited = true;
                    if (dest.Match == null || FindAugmentingPath(dest.Match))
                    {
                        //source and dest are matched
                        source.Match = dest;
                        dest.Match = source;
                        return true;
                    }

                }
            }
            return false;
        }

        private class Node
        {
            public int Number;
            public bool Visited = false;
            public Node Match = null;
            public List<Node> AdjacentNodes = new List<Node>();

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

