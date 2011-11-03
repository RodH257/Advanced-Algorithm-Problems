﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeworkProblems.ConvexHull
{
    /// <summary>
    /// Rod Howarth - n6294685
    /// Team14
    /// </summary>
    class ConvexHull
    {
        public static void Main(string[] args)
        {
            //get number of test case
            var numCases = Int32.Parse(Console.ReadLine());

            for (int testCaseNum = 0; testCaseNum < numCases; testCaseNum++)
            {
                int numPlants = Int32.Parse(Console.ReadLine());

                List<Point> plantLocations = new List<Point>();
                for (int plant = 0; plant < numPlants; plant++)
                {
                    var splitInts = GetSplitInts(Console.ReadLine());
                    //add x y
                    plantLocations.Add(new Point(splitInts[0], splitInts[1]));
                }


                List<Point> hull = FindConvexHull(plantLocations);

                double totalLength = 0;
                for (int i = 0; i < hull.Count; i++)
                {
                    if (i == hull.Count - 1)
                        totalLength += hull[i].LengthTo(hull[0]);
                    else
                        totalLength += hull[i].LengthTo(hull[i + 1]);
                }

                var cost = totalLength * 5 + hull.Count;
                Console.WriteLine("${0:0.00}", cost);
            }
        }

        public static List<Point> FindConvexHull(List<Point> points)
        {
            Point min = new Point(int.MaxValue, int.MaxValue);

            //Find bottom left point
            foreach (var point in points)
            {
                if (point.Y < min.Y)
                    min = point;
                else if (point.Y == min.Y && point.X < min.X)
                    min = point;
            }

            var vectorsFromZero = new SortedDictionary<double, Vector>();

            foreach (var point in points)
            {
                //get the vector from the starting poitn (minimum), to current point 
                var vector = Vector.VectorBetween(min, point);

                //Gets angle to y = 0
                var angle = vector.AngleTo(new Vector(min.X +4, min.Y));

                //dirty hack
                vector.PointItBelongsTo = point;
                if (vectorsFromZero.ContainsKey(angle))
                {
                    //we only want to add the one poitn in there, the furtherest away.
                    var currentLength = min.LengthTo(point);
                    var compareLength = min.LengthTo(new Point(vectorsFromZero[angle].X, vectorsFromZero[angle].Y));

                    if (currentLength > compareLength)
                        vectorsFromZero[angle] = vector;
                }
                else
                {
                    vectorsFromZero.Add(angle, vector);
                }
            }

            List<Vector> sortedVectors = vectorsFromZero.Values.ToList();
            Stack<Vector> stack = new Stack<Vector>(sortedVectors.Take(3));

            for (int i = 3; i < sortedVectors.Count; i++)
            {
                while (true)
                {
                    Vector cornerPoint = stack.Pop();
                    //stack is one less so we can just peek
                    Vector startPoint = stack.Peek();
                    Vector endPoint = sortedVectors[i];
                    //pretend the vectors are points, and get the vector between those oitns
                    //first vector will be from the start to the corner
                    Vector firstLine = Vector.VectorBetween(startPoint, cornerPoint);   
                    //from the corner to next point 
                    Vector secondLine = Vector.VectorBetween(cornerPoint, endPoint);

                    bool isRightTurn = IsNonLeftTurn(firstLine, secondLine);
                    if (!isRightTurn)
                    {
                        //put the top one back on
                        stack.Push(cornerPoint);
                        break;
                    }
                    //effectively popping here
                }
                stack.Push(sortedVectors[i]);
            }

            //we only need to know the lengths, so no need to convert the vector points back into proper points to 0.
            return stack.Cast<Point>().ToList();
        }

        public static bool IsNonLeftTurn(Vector firstLine, Vector secondLine)
        {
            //calculate signed area
            int cross = (firstLine.X * secondLine.Y) - (secondLine.X * firstLine.Y);
            if (cross == 0)
                return true;//straight

            //positive means right turn
            return cross < 0;
        }

        public class Point
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public override string ToString()
            {
                return String.Format("({0},{1})", X, Y);
            }

            /// <summary>
            /// Gets the length from this point to another point 
            /// </summary>
            public double LengthTo(Point to)
            {
                return Math.Sqrt(Math.Pow(to.X - this.X, 2) + Math.Pow(to.Y - this.Y, 2));
            }
        }

        public class Vector: Point
        {
            public Point PointItBelongsTo = null;
            public Vector(int x, int y) : base(x, y)
            {
            }

            public double Length
            {
                get { return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2)); }
            }

            public override string ToString()
            {
                return String.Format("({0},{1})", X, Y);
            }

            public static Vector VectorBetween(Point from, Point to)
            {
                return new Vector(to.X - from.X, to.Y - from.Y);
            }

            public double AngleTo(Vector v2)
            {
                var dotProd = (this.X * v2.X) + (this.Y * v2.Y);
                var length = this.Length * v2.Length;

                double EPSILON = 0.00000001;
                if (Math.Abs(length - 0) < EPSILON)
                    return 0;

                var angle = Math.Acos(dotProd / length);
                return angle;
            }

        }



        #region Utils
        public static int ReadInt()
        {
            string line = Console.ReadLine();
            return Int32.Parse(line);
        }

        public static int[] GetSplitInts(string input)
        {
            string[] intStrings = input.Split(' ');
            int[] splitInts = new int[intStrings.Length];
            for (int i = 0; i < intStrings.Length; i++)
                splitInts[i] = Int32.Parse(intStrings[i]);
            return splitInts;
        }

        public static int[] GetSplitInts()
        {
            string[] intStrings = Console.ReadLine().Split(' ');
            int[] splitInts = new int[intStrings.Length];
            for (int i = 0; i < intStrings.Length; i++)
                splitInts[i] = Int32.Parse(intStrings[i]);
            return splitInts;
        }
        #endregion
    }

}

