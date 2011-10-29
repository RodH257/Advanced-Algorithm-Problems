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
    public class Taxi
    {
        private static void Main(string[] args)
        {
            int numTestCases = int.Parse(Console.ReadLine());
            for (int testCaseNum = 0; testCaseNum < numTestCases; testCaseNum++)
            {
                var ints = GetSplitInts();
                var numPersons = ints[0];
                var numTaxis = ints[1];
                var speed = ints[2]; //speed in meters per second 
                var timeLimit = ints[3]; //time limit to collect a person

                int totalNodes = numTaxis + numPersons;

                Node[] nodes = new Node[totalNodes];

                for (int personNum = 0; personNum < numPersons; personNum++)
                {
                    var personLine = GetSplitInts();
                    Node person = new Node() {Number = personNum};
                    person.X = personLine[0];
                    person.Y = personLine[1];
                    nodes[personNum] = person;
                }

                for (int taxiNum = 0; taxiNum < numTaxis; taxiNum++)
                {
                    var taxiLine = GetSplitInts();
                    Node taxi = new Node() { Number = numPersons+taxiNum };
                    taxi.X = taxiLine[0];
                    taxi.Y = taxiLine[1];
                    nodes[taxi.Number] = taxi;
                }

                //find the bipartite graph
                FindBipartiteGraph(nodes, numPersons, numTaxis, speed, timeLimit);

                //work out the max cardinality
                int maxCardinality = GetMaximumCardinality(nodes, numPersons, numTaxis);

                Console.WriteLine("{0}: {1}", testCaseNum+1, maxCardinality);
            }
        }

        private static void FindBipartiteGraph(Node[] nodes, int numPeople,
            int numTaxis, int speed, int maxTime)
        {
            for (int taxiNum = numPeople; taxiNum < nodes.Length; taxiNum++)
            {
                Node taxi = nodes[ taxiNum];
                //find all the people it an reach
                for (int personNum = 0; personNum < numPeople; personNum++)
                {
                    Node person = nodes[personNum];

                    //abs(x1-x2) + y1-y2;
                    int manhatDistance = Math.Abs(taxi.X - person.X) 
                        + Math.Abs(taxi.Y - person.Y);

                    double meterDistance = manhatDistance*200;
                    double timetakes =  meterDistance/speed;

                    if (timetakes <= maxTime)
                    {
                        //its able to reach it, so its adjacent
                        taxi.AdjacentNodes.Add(person);
                    }
                }
            }

        }


        private static int GetMaximumCardinality(Node[] nodes, int m, int n)
        {
            //stuffed up, taxis need to be on the left, people on right
            //of bipartite, reverse and swap m and n to fix
            nodes = nodes.Reverse().ToArray();

            int cardinality = 0;

            for (int leftNodeNum = 0; leftNodeNum < n; leftNodeNum++)
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
                if (!dest.Visited)
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
            public int X;
            public int Y;
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

