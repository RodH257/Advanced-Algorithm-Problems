using System;

namespace AdvancedAlgorithms
{
    /// <summary>
    /// Rod Howarth - n6294685
    /// Team14
    /// Calculates permutations of how many rooks can 
    /// be on a chess board without being able to hit each other.
    /// </summary>
    public class NRooks
    {
        private static int _n;//number of items
        private static int[] _a; 
        private static int counter = 0;

        private static void Main(string[] args)
        {
            _n = 8; // size of chess board
            _a = new int[_n];

            //initialize each to i
            for (int i = 0; i < _n; i++)
                _a[i] = i;

            //enumerate starting from 0
            enumerate(0);

            Console.WriteLine(counter);
        }
       
        private static void enumerate(int k)
        {
            if (k == _n)
            {
                counter++;
                return;
            }

            for (int i = k; i < _n; i++)
            {
                swap(i, k);
                enumerate(k + 1);
                swap(i, k);
            }

        }

        private static void swap(int i, int k)
        {
            var currentI = _a[i];
            var currentK = _a[k];

            _a[i] = currentK;
            _a[k] = currentI;
        }
    }
}

//public class EnumerateBits {

//   int N; // number of bits
//   int[] a; // bits (0 or 1)

//   private void run() {
//      N = 4;
//      a = new int[N];
//      enumerate(0);
//   }

//   private void enumerate(int k) {
//      if (k == N) {
//         process();
//         return;
//      }
//      enumerate(k + 1);
//      a[k] = 1;
//      enumerate(k + 1);
//      a[k] = 0; // clean up
//   }
