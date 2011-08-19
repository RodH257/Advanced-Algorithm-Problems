//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace HomeworkProblems.Week1
//{
//    /// <summary>
//    /// Rod Howarth - n6294685
//    /// </summary>
//    public class LightMoreLight
//    {
//        private static void Main(string[] args)
//        {
//            int input = int.Parse(Console.ReadLine());

//            while (input != 0)
//            {
//                FindLightMoreLight(input);

//                //get next input
//                input = int.Parse(Console.ReadLine());
//            }
//        }

//        static void FindLightMoreLight(int n)
//        {
//            //square root is even number it must be pressed.
//            bool lastBulb = (Math.Sqrt(n)%1) == 0;

//            //final condition of last bulb, on or off?
//            if (lastBulb)
//                Console.WriteLine("yes");
//            else
//                Console.WriteLine("no");

//        }
//    }
//}
