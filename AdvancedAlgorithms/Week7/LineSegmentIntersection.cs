using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedAlgorithms.Week7.LSI
{
    /// <summary>
    /// Rod Howarth - n6294685
    /// Team14
    /// </summary>
    public class LineSegmentIntersection
    {
        private static void Main(string[] args)
        {
            int caseCount = 1;

            while (true)
            {
                bool first = true;
                string textLine = Console.ReadLine();
                if (textLine == "#")
                    break;

                IList<Line> lines = new List<Line>();
                while (true)
                {
                    if (!first)
                        textLine = Console.ReadLine();
                    if (textLine == "#")
                        break;

                    first = false;

                    //read the points 
                    var ints = GetSplitInts(textLine);
                    Point point1 = new Point() { X = ints[0], Y = ints[1] };
                    Point point2 = new Point() { X = ints[2], Y = ints[3] };

                    //construct the line
                    Line currentLine = new Line() { StartPoint = point1, EndPoint = point2 };
                    lines.Add(currentLine);
                }

                //got all the lines
                var points = CalculateCorners(lines);

                Console.WriteLine("Test Case {0}:", caseCount);
                Console.WriteLine("{0} corners", points.Count);
                foreach (var item in points.OrderBy(x => x.X).ThenBy(x => x.Y).Distinct())
                {
                    Console.WriteLine("{0:0.00} {1:0.00}", item.X, item.Y);
                }
                caseCount++;
            }
        }


        private static IList<Point> CalculateCorners(IList<Line> lines)
        {
            //all cordners added here
            IList<Point> corners = new List<Point>();
            //only corners that are in corners twice get added here, as it checks both ways.
            IList<Point> finalCorners = new List<Point>();
            foreach (Line line1 in lines)
            {
                foreach (Line line2 in lines)
                {
                    if (line2 == line1)
                        continue;
                    //see if they intersect
                    Point intersectPoint = GetIntersectionPoint(line1, line2);
                    if (intersectPoint != null)//&& !corners.Contains(intersectPoint))
                    {
                        if (corners.Contains(intersectPoint))
                            finalCorners.Add(intersectPoint);
                        corners.Add(intersectPoint);
                    }
                }
            }

            return finalCorners;
        }

        private static Point GetIntersectionPoint(Line line1, Line line2)
        {
            //Vector along 1 segment
            double v0X = line2.EndPoint.X - line2.StartPoint.X;
            double v0Y = line2.EndPoint.Y - line2.StartPoint.Y;

            //Vector from base of Line 2 to Start of Line1
            double v1X = line2.EndPoint.X - line1.StartPoint.X;
            double v1Y = line2.EndPoint.Y - line1.StartPoint.Y;

            //Vector from base of Line 2 to End of Line1
            double v2X = line2.StartPoint.X - line1.EndPoint.X;
            double v2Y = line2.StartPoint.Y - line1.EndPoint.Y;

            double crossA = (v0X * v1Y) - (v1X * v0Y);
            double crossB = (v0X * v2Y) - (v2X * v0Y);

            bool isIntersection = !((crossA > 0 && crossB > 0) || (crossA < 0 && crossB < 0));
            if (isIntersection)
            {
                //there is an interseection, find the coordinates
                var x1 = line1.StartPoint.X;
                var x2 = line1.EndPoint.X;
                var y1 = line1.StartPoint.Y;
                var y2 = line1.EndPoint.Y;

                var y3 = line2.StartPoint.Y;
                var y4 = line2.EndPoint.Y;
                var x3 = line2.StartPoint.X;
                var x4 = line2.EndPoint.X;

                //find alpha
                var top = x4 - x2 - (((x4 - x3) * (y4 - y2)) / (y4 - y3));
                var bottom = x1 - x2 - (((x4 - x3) * (y1 - y2)) / (y4 - y3));
                var alpha = top / bottom;

                //plug it into slide 3
                var xInt = (alpha * x1) + (1 - alpha) * x2;
                var yInt = (alpha * y1) + (1 - alpha) * y2;

                return new Point() { X = xInt, Y = yInt };

            }
            return null;
        }

        
        private class Line
        {
            public Point StartPoint;
            public Point EndPoint;
        }

        public class Point : IEquatable<Point>
        {
            public double X;
            public double Y;

            #region IEquatable<Point> Members

            public bool Equals(Point other)
            {
                double EPSILON = 0.00000001;
                return Math.Abs(X - other.X) < EPSILON && Math.Abs(Y - other.Y) < EPSILON;
            }

            #endregion
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

