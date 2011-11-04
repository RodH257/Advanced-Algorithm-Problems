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
    public class Spreadsheet
    {
        private static void Main(string[] args)
        {
            int numTestCases = int.Parse(Console.ReadLine());

            for (int testCaseNum = 0; testCaseNum < numTestCases; testCaseNum++)
            {
                var ints = GetSplitInts();
                int cols = ints[0];
                int rows = ints[1];
                notComputable = false;

                Dictionary<string, Node> nodesReference = new Dictionary<string, Node>();

                List<Node> computedNodesList = new List<Node>();
                List<Node> allNodes = new List<Node>();

                for (int rowNumber = 1; rowNumber <= rows; rowNumber++)
                {
                    string[] rowSplit = Console.ReadLine().Split(' ');

                    for (int colNumber = 1; colNumber <= cols; colNumber++)
                    {

                        string columName = GetColumnName(colNumber);
                        Node node = new Node();
                        node.Name = columName + rowNumber;
                        node.Value = rowSplit[colNumber - 1];
                        nodesReference.Add(node.Name, node);

                        if (node.IsComputed())
                            computedNodesList.Add(node);

                        allNodes.Add(node);
                    }
                }

                //add the adjacent nodes
                foreach (Node node in computedNodesList)
                {
                    var references = node.Value.Replace("=", "").Split('+');
                    foreach (var reference in references)
                        node.ReferencedNodes.Add(nodesReference[reference]);
                }


                Console.WriteLine((testCaseNum + 1) + ":");
                //compute all the computed nodes
                ComputeReferences(allNodes);

                if (notComputable)
                    continue;

                for (int i = 0; i < allNodes.Count; i++)
                {

                    Console.Write(allNodes[i].Value + " ");
                    if (i % cols == cols - 1)
                        Console.WriteLine();
                }

            }
        }


        private static void ComputeReferences(List<Node> allnodes)
        {
            Stack<Node> nodeStack = new Stack<Node>();

            foreach (Node node in allnodes)
            {
                if (!node.Discovered)
                {
                    DepthFirstSearch(node, nodeStack);
                    if (notComputable)
                    {
                        //acyclic, cancel
                        Console.WriteLine("Not computable!");
                        return;
                    }
                }
            }

            //now in topological order, get the references filled out
            while (nodeStack.Count != 0)
            {
                Node currentNode = nodeStack.Pop();
                currentNode.GetComputedValue();
            }
        }
        private static bool notComputable;

        private static void DepthFirstSearch(Node node, Stack<Node> nodeStack)
        {
            node.Discovered = true;
            foreach (Node adjNode in node.ReferencedNodes)
            {
                if (!adjNode.Discovered)
                {
                    DepthFirstSearch(adjNode, nodeStack);
                }
                else if (!adjNode.Processed)
                {
                    //Not computable! 
                    notComputable = true;
                }
            }
            node.Processed = true;
            nodeStack.Push(node);

        }

        private static string GetColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }


        private class Node
        {
            public string Name;
            public string Value;
            public bool Processed = false;
            public bool Discovered = false;
            public bool IsComputed()
            {
                return this.Value.Contains("=");
            }

            public List<Node> ReferencedNodes = new List<Node>();

            public int GetComputedValue()
            {
                if (this.IsComputed())
                {
                    int value = 0;

                    foreach (Node adjNode in ReferencedNodes)
                    {
                        value += adjNode.GetComputedValue();
                    }

                    this.Value = value.ToString();
                }
                return int.Parse(this.Value);
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

