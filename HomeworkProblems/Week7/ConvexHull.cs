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
            var numCases = int.Parse(Console.ReadLine());

            for (int c = 0; c < numCases; c++)
            {
                var numPlants = int.Parse(Console.ReadLine());

                var plants = new List<Point>();
                for (int plant = 0; plant < numPlants; plant++)
                {
                    var input = Console.ReadLine().Split(' ').Select(x => int.Parse(x)).ToList();

                    plants.Add(new Point(input[0], input[1]));
                }

                var outline = plants.FindConvexHull();

                double totalLength = 0;
                for (int i = 0; i < outline.Count; i++)
                {


                    if (i == outline.Count - 1)
                    {
                        totalLength += outline[i].LengthTo(outline[0]);
                    }
                    else
                    {
                        totalLength += outline[i].LengthTo(outline[i + 1]);
                    }
                }

                var cost = totalLength*5 + outline.Count;
                Console.WriteLine("${0:0.00}", cost);
            }
        }
    }

    static class Utils
    {
        public static List<Point> FindConvexHull(this List<Point> points)
        {
            var min = new Point(int.MaxValue, int.MaxValue);

            //Find bottom left point
            foreach (var point in points)
            {
                if (point.Y < min.Y)
                {
                    min = point;
                }
                else if (point.Y == min.Y && point.X < min.X)
                {
                    min = point;
                }
            }

            var vectorsFromZero = new SortedDictionary<double, Vector>();

            foreach (var point in points)
            {
                var vector = min.VectorTo(point);
                //Gets angle to y = 0
                var angle = vector.AngleTo(new Vector(min.X + 4, min.Y));


                if (vectorsFromZero.ContainsKey(angle))
                {
                    var currentLength = min.LengthTo(point);
                    var compareLength = min.LengthTo(new Point(vectorsFromZero[angle].X, vectorsFromZero[angle].Y));

                    if (currentLength > compareLength)
                    {
                        vectorsFromZero[angle] = vector;
                    }
                }
                else
                {
                    vectorsFromZero.Add(angle, vector);
                }
            }

            var sortedVectors = vectorsFromZero.Values.ToList();

            var stack = new Stack<Vector>(sortedVectors.Take(3));

            for (int i = 3; i < sortedVectors.Count; i++)
            {
                //if(i == sortedVectors.Count-1)
                //{
                //    var newStack = stack.ToList();

                //    stack = new Stack<Vector>();
                //}

                while (true)
                {
                    var topOfStack = stack.Pop();
                    var secondFromTop = stack.Peek();
                  

                    var first = secondFromTop.VectorTo(topOfStack);
                    var second = topOfStack.VectorTo(sortedVectors[i]);

                    bool isRightTurn = IsRightTurn(secondFromTop, topOfStack, sortedVectors[i]);
                    if (!isRightTurn)
                    {
                        stack.Push(topOfStack);
                        break;
                    }

                }

                stack.Push(sortedVectors[i]);

            }

            return stack.Select(x => new Point(x.X, x.Y)).ToList();
        }

        public static bool IsRightTurn(Vector start, Vector cornerPoint, Vector endPoint)
        {
            //p2-p1
            int v2X = endPoint.X - cornerPoint.X;
            int v2Y = endPoint.Y - cornerPoint.Y;

            //p1 -p0
            int v1x = cornerPoint.X - start.X;
            int v1y = cornerPoint.Y - start.Y;


            //calculate signed area
            int cross = (v1x * v2Y) - (v2X * v1y);
            if (cross == 0)
                return true;//straight is a non left turn

            //positive means right turn
            return cross < 0;
        }

        public static double LengthTo(this Point from, Point to)
        {
            return Math.Sqrt(Math.Pow(to.X - from.X, 2) + Math.Pow(to.Y - from.Y, 2));
        }

        public static Vector VectorTo(this Point from, Point to)
        {
            return new Vector(to.X - from.X, to.Y - from.Y);
        }

        public static Vector VectorTo(this Vector from, Vector to)
        {
            return new Vector(to.X - from.X, to.Y - from.Y);
        }

        public static double AngleTo(this Vector v1, Vector v2)
        {
            var dotProd = (v1.X * v2.X) + (v1.Y * v2.Y);
            var length = v1.Length * v2.Length;

            if (length == 0)
                return 0;

            var angle = Math.Acos(dotProd / length);

            return angle;
        }
    }

    class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point Empty()
        {
            return new Point(0, 0);
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return String.Format("({0},{1})",X,Y);
        }
    }

    class Vector
    {
        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector Empty()
        {
            return new Vector(0, 0);
        }

        public int X { get; set; }
        public int Y { get; set; }
        
        public double Length
        {
            get { return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2)); }
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }
    }
}

