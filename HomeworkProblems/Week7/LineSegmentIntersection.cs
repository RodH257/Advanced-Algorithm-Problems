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
    public class LineSegmentIntersection
    {
        private static void Main(string[] args)
        {
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
                    Line currentLine = new Line() { Point1 = point1, Point2 = point2 };
                    lines.Add(currentLine);
                }

                //got all the lines
                CalculateCorners(lines);

            }
        }

        public static void CalculateCorners(IList<Line> lines)
        {
            IList<Point> corners = new List<Point>();
            foreach (Line line1 in lines)
            {
                foreach (Line line2 in lines)
                {
                    if (line2 == line1)
                        continue;

                    //see if they intersect
                    Point intersectPoint = GetIntersectionPoint(line1, line2);
                    if (intersectPoint != null)
                        corners.Add(intersectPoint);
                }
            }
        }

        public static Point GetIntersectionPoint(Line line1, Line line2)
        {
            //Vector along 1 segment
            int v0X = line2.Point2.X - line2.Point1.X;
            int v0Y = line2.Point2.Y - line2.Point1.Y;


            //Vector from base of Line 2 to Start of Line1
            int v1X = line2.Point1.X - line1.Point1.X;
            int v1Y = line2.Point2.Y - line1.Point1.Y;


            //Vector from base of Line 2 to End of Line1
            int v2X = line2.Point1.X - line1.Point2.X;
            int v2Y = line2.Point2.Y - line1.Point2.Y;

            int crossA = (v0X * v1Y) - (v1X * v0Y);
            int crossB = (v0X * v2Y) - (v2X * v0Y);

            if ((crossA > 0 && crossB > 0) || (crossA < 0 && crossB < 0))
            {
                //no intersection
            } else
            {
                //there is an interseection, find the coordinates

                var x1 = line1.Point1.X;
                var x2 = line1.Point2.X;
                var x3 = line2.Point1.X;
                var x4 = line2.Point2.X;
                var y1 = line1.Point1.Y;
                var y2 = line1.Point2.Y;
                var y3 = line2.Point1.Y;
                var y4 = line2.Point2.Y;

                var top = x4 - x2 - ((x4 - x3)*(y4 - y2)/(y4 - y3));
                var bottom = x1 - x2 - ((x4 - x3)*(y1 - y2)/y4 - y3);
                var alpha = top/bottom;


                //Line 1
                var line1vecO = line1;
                var line1vecD = new Point() { X = line1.Point2.X - line1.Point1.X, Y = line1.Point2.Y - line1.Point1.Y };
                var line1vecNormal = new Point() { X = -line1vecD.Y, Y = line1vecD.X };

                var line1k = line1.Point1.X * line1vecNormal.X + line1.Point1.Y * line1vecNormal.Y;

                //Line 1
                var line2vecO = line2;
                var line2vecD = new Point() { X = line2.Point2.X - line2.Point1.X, Y = line2.Point2.Y - line2.Point1.Y };
                var line2vecNormal = new Point() { X = -line2vecD.Y, Y = line2vecD.X };

                var line2k = line2.Point1.X * line2vecNormal.X + line2.Point1.Y * line2vecNormal.Y;



            }
            return null;
        }

        public class Line
        {
            public Point Point1;
            public Point Point2;
        }
        public class Point
        {
            public int X;
            public int Y;
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

