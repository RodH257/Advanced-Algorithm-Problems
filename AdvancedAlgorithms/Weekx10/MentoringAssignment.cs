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
    public class MentoringAssignment
    {
        private const string VOLUNTEER = "Volunteer";
        private const string ADOLESCENT = "Adolescent";


        private static int[] adolescentWeights;
        private static int[] volunteerWeights;
        private static int numTraits;
        private static int numPeople;
        private static Person[] volunteers;
        private static Person[] adolescents;


        private static void Main(string[] args)
        {
            int situationNum = 1;
            while (true)
            {
                var firstLine = GetSplitInts();
                if (firstLine[0] == 0 && firstLine[1] == 0)
                {
                    //at the end, break
                    break;
                }

                numTraits = firstLine[0];
                numPeople = firstLine[1];
                adolescentWeights = GetSplitInts();
                volunteerWeights = GetSplitInts();

                volunteers = new Person[numPeople];
                adolescents = new Person[numPeople];
                int curVolunteer = 0;
                int curAdolescent = 0;
                for (int lineNum = 0; lineNum < (numPeople * 2); lineNum++)
                {
                    string[] line = Console.ReadLine().Split(' ');

                    string type = line[0];
                    string name = line[1];
                    int[] traitScores = new int[numTraits];
                    //get all the scores for traits
                    for (int traitNum = 0; traitNum < numTraits; traitNum++)
                    {
                        int currentTraitScore = int.Parse(line[2 + traitNum]);
                        traitScores[traitNum] = currentTraitScore;
                    }
                    //create a new person
                    Person person = new Person();
                    person.IsAdolescent = type == ADOLESCENT;
                    person.Name = name;
                    person.TraitScores = traitScores;

                    //add them to the arrays
                    if (person.IsAdolescent)
                    {
                        adolescents[curAdolescent] = person;
                        curAdolescent++;
                    }
                    else
                    {
                        volunteers[curVolunteer] = person;
                        curVolunteer++;
                    }
                }

                //all setup, run algorithm

                DoMatching(adolescents, volunteers, numPeople);

                Console.WriteLine("Situation {0}:", situationNum);

                //order the list lexiographically and output
                List<Person> outputPeople = adolescents.OrderBy(x => x.Name).ToList();
                foreach (Person adolescent in outputPeople)
                {
                    Console.WriteLine("{0} {1}",
                                      adolescent.OutputName,
                                      adolescent.CurrentMatch.OutputName);
                }

                situationNum++;
            }

        }

        public static void DoMatching(Person[] adolescents, Person[] volunteers, int numPairs)
        {
            for (int i = 0; i < numPairs; i++)
            {
                Person m = volunteers[i];
                while (!m.IsMatched())
                {
                    m = FindMatch(m);
                }
            }
        }
        public static Person FindMatch(Person adolescent)
        {
            int currentRank = 0;
            while (!adolescent.IsMatched())
            {
                Person volunteer = adolescent.GetNextPreference(currentRank);

                if (!volunteer.IsMatched())
                {
                    adolescent.MatchWith(volunteer);
                }
                else if (volunteer.GetQuality(adolescent) < volunteer.MatchQuality)
                {
                    //replace the current match with this 
                    Person replaced = volunteer.CurrentMatch;
                    adolescent.MatchWith(volunteer);
                    replaced.CurrentMatch = null;
                    return replaced;
                }
                currentRank++;
            }

            return adolescent;
        }


        public class Person
        {
            public bool IsAdolescent;
            public string Name;
            public int[] TraitScores;
            public Person CurrentMatch = null;
            public string OutputName
            {
                get
                {
                    if (IsAdolescent)
                        return "Adolescent " + Name;

                    return "Volunteer " + Name;

                }
            }

            public override string ToString()
            {
                return OutputName;
            }

            public double MatchQuality
            {
                get
                {
                    return GetQuality(CurrentMatch);
                }
            }

            //does the quality calculations based on weightings 
            public double GetQuality(Person other)
            {
                double quality = 0;
                if (this.IsAdolescent)
                {
                    for (int s = 0; s < numTraits; s++)
                    {
                        quality += adolescentWeights[s] * Math.Pow((this.TraitScores[s] - other.TraitScores[s]), 2);
                    }
                }
                else
                {
                    for (int s = 0; s < numTraits; s++)
                    {
                        //reversed this and other here as we are comparing an adolescent to a volunteer
                        quality += volunteerWeights[s] * Math.Pow((other.TraitScores[s] - this.TraitScores[s]), 2);
                    }
                }

                return quality;
            }

            //store the best matches so we can get the next preference without recalculating each time
            private List<Person> sortedList = null;
            //currentPersonNumber is how far down the list they are
            public Person GetNextPreference(int currentPersonNumber)
            {
                if (sortedList == null)
                {
                    //initialize the list 
                    sortedList = new List<Person>();

                    for (int i = 0; i < numPeople; i++)
                    {
                        Person personToAdd;
                        //add the quality comparison and the volunteer/adolescent to the queue
                        if (this.IsAdolescent)
                            personToAdd = volunteers[i];
                        else
                            personToAdd = adolescents[i];
                        sortedList.Add(personToAdd);
                    }

                    sortedList = sortedList.OrderBy(p => this.GetQuality(p)).ToList();
                }

                return sortedList[currentPersonNumber];
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

