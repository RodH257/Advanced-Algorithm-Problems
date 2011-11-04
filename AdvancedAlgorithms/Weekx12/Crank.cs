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
    public class Crank
    {
        private static void Main(string[] args)
        {
            int numTestCases = int.Parse(Console.ReadLine());
            for (int testCaseNum = 1; testCaseNum <= numTestCases; testCaseNum++)
            {
                var firstLine = GetSplitInts();

                int blockRows = firstLine[0];
                int blockCols = firstLine[1];
                var secondLine = GetSplitInts();

                int bossY = secondLine[1] - 1;
                int bossX = secondLine[0] - 1;

                Building[,] block = new Building[blockRows, blockCols];
                //read info into matrix
                for (int blockRow = 0; blockRow < blockRows; blockRow++)
                {
                    var buildingDetails = GetSplitInts();

                    int buildingIndex = 0;
                    //get the buildings on this row
                    foreach (int height in buildingDetails)
                    {
                        //set the height in the matrix
                        block[blockRow, buildingIndex] = new Building() { height = height, Row = blockRow, Column = buildingIndex };
                        buildingIndex++;
                    }
                }

                Building bossHouse = block[bossX, bossY];
                //start from boss and find how many buildings around it increase from where boss is.
                //work out from there.
                int numBuildings = BreadthFirst(block, bossHouse, blockRows, blockCols);
                Console.WriteLine("Case #{0}: {1}", testCaseNum, numBuildings);
            }
        }

        private class Building
        {
            public int Row;
            public int Column;
            public bool Visited;
            public int height;
        }


        private static int BreadthFirst(Building[,] block, Building startingPos, int blockRows, int blockColumns)
        {
            Queue<Building> nodes = new Queue<Building>();
            nodes.Enqueue(startingPos);
            int visitedNodesCount = 0;

            while (nodes.Count != 0)
            {
                Building currentNode = nodes.Dequeue();

                currentNode.Visited = true;

                if (currentNode.Row == blockRows-1 || currentNode.Column == blockColumns-1 || currentNode.Row == 0 || currentNode.Column == 0)
                    visitedNodesCount++;
                //check all adjacencies to find one higher than it

                for (int row = -1; row <= 1; row++)
                {

                    for (int column = -1; column <= 1; column++)
                    {
                        if ((column != 0 && row != 0) || (row == 0 && column == 0))
                            continue;

                        //get point to check
                        int buildingRow = currentNode.Row + row;
                        int buildingColumn = currentNode.Column + column;

                        if (buildingRow < 0 || buildingRow >= blockRows || buildingColumn < 0 || buildingColumn >= blockColumns)
                            continue;

                        Building adjNode = block[buildingRow, buildingColumn];

                        //check if the height is greater than it 
                        if (adjNode.height >= currentNode.height && !adjNode.Visited)
                        {
                            //we can go there, add it to the queue
                            if (!nodes.Contains(adjNode))
                                nodes.Enqueue(adjNode);
                        }
                    }

                }
            }

            return visitedNodesCount;
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

