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
    public class StableMarriage2
    {
        private static void Main(string[] args)
        {
            int numTestCases = int.Parse(Console.ReadLine());
            for (int testCaseNum = 0; testCaseNum < numTestCases; testCaseNum++)
            {
                int numPairs = int.Parse(Console.ReadLine());

                Person[] men = new Person[numPairs];
                Person[] women = new Person[numPairs];

                //initialize men and women
                for (int personNumber = 0; personNumber < numPairs; personNumber++)
                {
                    Person woman = new Person();
                    woman.IsWoman = true;
                    woman.Number = personNumber;
                    Person man = new Person();
                    man.IsWoman = false;
                    man.Number = personNumber;

                    men[personNumber] = man;
                    women[personNumber] = woman;
                }

                //gather mens preferencess
                for (int manNumber = 0; manNumber < numPairs; manNumber++)
                {
                    var prefLine = GetSplitInts();

                    Person curMan = men[manNumber];
                    for (int rankNumber = 0; rankNumber < numPairs; rankNumber++)
                    {
                        Person curWoman = women[prefLine[rankNumber]];
                        curMan.Preferences.Add(curWoman);
                    }
                }

                //gather female preferneces 
                for (int womanNumber = 0; womanNumber < numPairs; womanNumber++)
                {
                    var prefLine = GetSplitInts();

                    Person curWoman = women[womanNumber];

                    for (int rankNumber = 0; rankNumber < numPairs; rankNumber++)
                    {
                        Person curMan = men[prefLine[rankNumber]];
                        curWoman.Preferences.Add(curMan);
                    }
                }

                DoMatching(men, women, numPairs);

                Console.WriteLine("Case {0}:", (testCaseNum + 1));
                for (int i = 0; i < numPairs; i++)
                {
                    Console.WriteLine("{0} marries {1}",
                        men[i].Name,
                        men[i].CurrentMatch.Name);
                }


            }
        }

        public static void DoMatching(Person[] men, Person[] women, int numPairs)
        {
            for (int i =0 ;i < numPairs; i++)
            {
                Person m = men[i];
                while (!m.IsMatched())
                {
                    m = FindMatch(m);
                }
            }
        }
        public static Person FindMatch(Person man)
        {
            int currentRank = 0;
            while (!man.IsMatched())
            {
                Person woman = man.Preferences[currentRank];
                //if that woman is not matched, match them
                if (!woman.IsMatched())
                {
                    man.MatchWith(woman);
                }
                else if (woman.GetRank(man) < woman.MatchRank)
                {
                    //replace the current match with this 
                    Person replaced = woman.CurrentMatch;
                 //   Console.WriteLine("Deleting " + replaced.Name
                //        + " loves " + woman.Name);
                    man.MatchWith(woman);
                    replaced.CurrentMatch = null;


                    return replaced;
                }
                currentRank++;
            }

            return man;
        }

        public class Person
        {
            public bool IsWoman;
            public int Number;

            public string Name
            {
                get
                {
                    if (IsWoman)
                        return "Woman " + Number;

                    return "Man " + Number;

                }
            }

            public override string ToString()
            {
                return Name;
            }
            public Person CurrentMatch = null;
            public int MatchRank
            {
                get
                {
                    return GetRank(CurrentMatch);
                }
            }
            public List<Person> Preferences = new List<Person>();

            public int GetRank(Person other)
            {
                return Preferences.IndexOf(other);
            }

            public bool IsMatched()
            {
                return this.CurrentMatch != null;
            }

            public void MatchWith(Person other)
            {
                this.CurrentMatch = other;
                other.CurrentMatch = this;
              //  Console.WriteLine("Matching " + this.Name + " with " + other.Name);
            }

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

