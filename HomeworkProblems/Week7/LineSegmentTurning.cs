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
    public class LineSegmentTurning
    {
        private static void Main(string[] args)
        {
            int numTestCases = ReadInt();
            for (int testCaseNum = 0; testCaseNum < numTestCases; testCaseNum++)
            {
                int numberOfSegments = ReadInt();
                IList<Point> points = new List<Point>();
                for (int lineSegment = 0; lineSegment < numberOfSegments; lineSegment++)
                {
                    int[] splitInts = GetSplitInts();
                    int x = splitInts[0];
                    int y = splitInts[1];

                    Point point = new Point() { X = x, Y = y };
                    points.Add(point);
                }

                WorkOutIfPossible(points);

            }
        }

        public class Point
        {
            public int X;
            public int Y;
        }

        public class Corner
        {
            public Point start;
            public Point cornerPoint;
            public Point endPoint;
        }
        public static void WorkOutIfPossible(IList<Point> points)
        {

            //get all the corners

            IList<Corner> corners = new List<Corner>();
            if (points.Count >= 3)
            {
                for (int i = 2; i < points.Count; i++)
                {
                    Corner currentCorner = new Corner();
                    currentCorner.start = points[i - 2];
                    currentCorner.cornerPoint = points[i - 1];
                    currentCorner.endPoint = points[i];
                    corners.Add(currentCorner);
                }

                Corner returnToStart = new Corner();
                returnToStart.start = points[points.Count - 2];
                returnToStart.cornerPoint = points[points.Count - 1];// last point 
                returnToStart.endPoint = points[0];
                corners.Add(returnToStart);
                Corner continuePastStart = new Corner();
                continuePastStart.start = points[points.Count - 1];
                continuePastStart.cornerPoint = points[0];
                continuePastStart.endPoint = points[1];
                corners.Add(continuePastStart);

            }

            bool allPositives = true;
            bool allNegatives = true;
            foreach (Corner c in corners)
            {

                //p2-p1
                int v2X = c.endPoint.X - c.cornerPoint.X;
                int v2Y = c.endPoint.Y - c.cornerPoint.Y;

                //p1 -p0
                int v1x = c.cornerPoint.X - c.start.X;
                int v1y = c.cornerPoint.Y - c.start.Y;


                //calculate signed area
                int cross = (v1x * v2Y) - (v2X * v1y);
                if (cross == 0)
                    continue;

                //check its all consistent at least one direction
                allPositives &= cross > 0;
                allNegatives &= cross < 0;

                if (!allPositives && !allNegatives)
                {
                    Console.WriteLine("not possible");
                    return;
                }
            }

            Console.WriteLine("possible");
        }


        public static int ReadInt()
        {
            string line = Console.ReadLine();
            return int.Parse(line);
        }

        public static int[] GetSplitInts()
        {
            string[] intStrings = Console.ReadLine().Split(' ');
            int[] splitInts = new int[intStrings.Length];
            for (int i = 0; i < intStrings.Length; i++)
                splitInts[i] = int.Parse(intStrings[i]);
            return splitInts;
        }
    }


}

