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
    public class ChannelAllocation
    {
        private const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private static void Main(string[] args)
        {
            while (true)
            {
                int numRepeaters = int.Parse(Console.ReadLine());
                if (numRepeaters == 0)
                    return;

               Node[] repeaters = new Node[numRepeaters];

                //setup the initial repeater nodes
                for (int repeaterNum = 0; repeaterNum < numRepeaters; repeaterNum++)
                {
                    Node repeater = new Node() { Number = repeaterNum };
                    repeaters[repeaterNum] = repeater;
                }

                //get the adjacent nodes for each
                for (int repeaterNum = 0; repeaterNum < numRepeaters; repeaterNum++ )
                {
                    //get the line, strip the first 2 characters
                    string line = Console.ReadLine().Replace(alphabet[repeaterNum] + ":", "");

                    //find the adjacent nodes
                    for (int letterIndex = 0; letterIndex < line.Length; letterIndex++)
                    {
                        //get the adjacent nodes letter
                        char letter = line[letterIndex];
                        int letterValue = alphabet.IndexOf(letter);

                        // look up its node
                        Node adjNode = repeaters[letterValue];

                        //add adjacents to each.
                        repeaters[repeaterNum].AdjacentNodes.Add(adjNode);
                        adjNode.AdjacentNodes.Add(repeaters[repeaterNum]);
                    }

                }

                //order by number of adjacent nodes
                var sortedRepeaters = repeaters.OrderByDescending(r => r.AdjacentNodes.Count);

                int maxColor = ColorGraphAndGetColorCount(sortedRepeaters);

                if (maxColor == 1)
                    Console.WriteLine("1 channel needed.");
                else
                    Console.WriteLine("{0} channels needed.", maxColor);
            }

        }

        /// <summary>
        /// Colors the graph and returns the number of colors needed
        /// </summary>
        private static int ColorGraphAndGetColorCount(IOrderedEnumerable<Node> sortedRepeaters )
        {
            int maxColor = 0;

            foreach(Node node in sortedRepeaters)
            {
                int minColorNotUsed = 0;
                List<int> colorsUsed = new List<int>();
                foreach (Node adjacentNode in node.AdjacentNodes)
                {
                    if (adjacentNode.Color != -1)
                        colorsUsed.Add(adjacentNode.Color);
                }

                //its a planar graph so no more than 4 colors needed
                for (int i = 0; i <=3; i ++)
                {
                    if (!colorsUsed.Contains(i))
                    {
                        minColorNotUsed = i;
                        break;
                    }
                }

                //now we have the minimum color not used by adjacent nodes, color the node
                node.Color = minColorNotUsed;
                if (minColorNotUsed > maxColor)
                    maxColor = minColorNotUsed;
            }

            return maxColor+1;
        }


        private class Node
        {
            public char Letter
            {
                get { return alphabet[Number]; }
            }
            public int Number;
            public List<Node> AdjacentNodes = new List<Node>();
            public int Color = -1;
        }

    }
}

