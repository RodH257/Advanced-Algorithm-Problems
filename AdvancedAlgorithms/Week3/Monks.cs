using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdvancedAlgorithms
{
    /// <summary>
    /// Rod Howarth - n6294685
    /// Team14
    /// </summary>
    public class Monks
    {
        private const int NOT_INITIALIZED = -1;
        private static void Main(string[] args)
        {
            while (true)
            {
                string line = Console.ReadLine();

                var splitLine = line.Split(' ');
                int jarA = int.Parse(splitLine[0]);
                int jarB = int.Parse(splitLine[1]);
                int jarC = int.Parse(splitLine[2]);

                if (jarA + jarB + jarC == 0)
                    return;

                int numberOfDays = GetSmallestPossibleDays(jarA, jarB, jarC);

                Console.WriteLine(jarA + " " + jarB + " " + jarC + " " + numberOfDays);
            }

        }

        //Represents a state of the jars
        private class JarState : IEquatable<JarState>
        {
            public const int A = 0;
            public const int B = 1;
            public const int C = 2;

            public int Level;
            public int[] Jars = new int[3];

            //initial constructor
            public JarState(int jarA, int jarB, int jarC)
            {
                Jars[A] = jarA;
                Jars[B] = jarB;
                Jars[C] = jarC;
            }

            //copy constructor
            public JarState(JarState stateToCopy)
            {
                Jars[A] = stateToCopy.Jars[A];
                Jars[B] = stateToCopy.Jars[B];
                Jars[C] = stateToCopy.Jars[C];
                this.Level = stateToCopy.Level + 1;
            }

            public bool HasEmptyJar()
            {
                return Jars[A] == 0 || Jars[B] == 0 || Jars[C] == 0;
            }

            //equatable for visited list 
            public bool Equals(JarState other)
            {
                //int thisHashCode = this.GetHashCode();
                //int otherHashCode = other.GetHashCode();
                //bool theSame = thisHashCode == otherHashCode;
                //return theSame;
                return (this.Jars[A] == other.Jars[A] && this.Jars[B] == other.Jars[B] && this.Jars[C] == other.Jars[C]);

            }

            public override bool Equals(object obj)
            {
                if (obj is JarState)
                    return Equals((JarState)obj);

                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return (Jars[0] * 10) * (Jars[1] * 20) * (Jars[2] * 30);
            }
        }

        //gets the smallest days to empty a jar 
        private static int GetSmallestPossibleDays(int jarA, int jarB, int jarC)
        {
            //setup the first jar state
            JarState firstState = new JarState(jarA, jarB, jarC) { Level = 0 };

            HashSet<JarState> visitedNodes = new HashSet<JarState>();
            Queue<JarState> nodes = new Queue<JarState>();
            nodes.Enqueue(firstState);

            while (nodes.Count != 0)
            {
                //get first node, and check it 
                JarState currentState = nodes.Dequeue();
                visitedNodes.Add(currentState);

                if (currentState.HasEmptyJar())
                    return currentState.Level;

                //wasn't empty, so find the  various options to add to the queue. 
                for (int jarIndex = 0; jarIndex < 3; jarIndex++)
                {
                    int currentJarValue = currentState.Jars[jarIndex];

                    //compare with other jars
                    for (int jarToCompareIndex = 0; jarToCompareIndex < 3; jarToCompareIndex++)
                    {
                        if (jarToCompareIndex == jarIndex)
                            continue;

                        int jarToCompareValue = currentState.Jars[jarToCompareIndex];

                        if (currentJarValue >= jarToCompareValue)
                        {
                            //we've got a new scenario.
                            JarState newState = new JarState(currentState);
                            //double the size of the smaller jar, and take the size added away from the current jar
                            newState.Jars[jarIndex] -= newState.Jars[jarToCompareIndex];
                            newState.Jars[jarToCompareIndex] *= 2;

                            if (!visitedNodes.Contains(newState))
                                nodes.Enqueue(newState);
                        }
                    }
                }
            }
            return -1;
        }

    }
}