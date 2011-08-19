using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestionE
{
    class Program
    {
        static void Main(string[] args)
        {
            int numTestCases = int.Parse(Console.ReadLine());

            for (int i = 0; i < numTestCases; i++)
            {
                string jumbledText = Console.ReadLine();

                int dictlength = int.Parse(Console.ReadLine());

                Dictionary<char, List<string>> dictionary = new Dictionary<char, List<string>>();
                int minLength = -1;
                int maxLength = -1;

                for (int x = 0; x < dictlength; x++)
                {
                    string wordToAdd = Console.ReadLine();

                    //index it by the first character
                    char firstChar = wordToAdd[0];
                    if (!dictionary.ContainsKey(firstChar))
                    {
                        dictionary.Add(firstChar, new List<string>());
                    }

                    if (!dictionary[firstChar].Contains(wordToAdd))
                        dictionary[firstChar].Add(wordToAdd);

                    if (minLength == -1 || minLength > wordToAdd.Length)
                        minLength = wordToAdd.Length;
                    if (maxLength == -1 || maxLength < wordToAdd.Length)
                        maxLength = wordToAdd.Length;
                }


                //got the dictionary, now lets get the unjumbled sentence
                GetUnjumbledSentence(dictionary, jumbledText, minLength, maxLength);
            }
        }


        private static void GetUnjumbledSentence(Dictionary<char, List<string>> dictionary, string jumbledWord, int minLength, int maxLength)
        {
            Stack<string> prefixes = new Stack<string>();
            prefixes.Push("");

            IList<string> results = new List<string>();


            while (prefixes.Count != 0)
            {
                string originalprefix = prefixes.Pop();
                string prefix = originalprefix.Replace(" ", "");

                int alreadyWorkedOutLength = prefix.Length;

                //loop through the various word lengths
                for (int wordLength = minLength; wordLength <= maxLength; wordLength++)
                {
                    if (alreadyWorkedOutLength + wordLength > jumbledWord.Length)
                    {
                        break;
                    }
                    string currentText = jumbledWord.Substring(alreadyWorkedOutLength, wordLength);
                    char startingChar = currentText[0];

                    if (!dictionary.ContainsKey(startingChar))
                        continue;
                    foreach (string dictWord in dictionary[startingChar])
                    {
                        if (dictWord.Length == wordLength && dictWord[wordLength - 1] == currentText[wordLength - 1])
                        {
                            //the dict word has same first and last char, and is same length, compare it.
                            string innerChars = "";
                            if (wordLength > 2)
                                innerChars = currentText.Substring(1, wordLength - 2);

                            string dictWordSubstring = dictWord;
                            if (dictWord.Length > 2)
                                dictWordSubstring = dictWord.Substring(1, wordLength - 2);
                            foreach (var character in dictWordSubstring)
                            {
                                if (innerChars.Contains(character))
                                {
                                    //found it, remove it so it doesnt get checked again
                                    int index = innerChars.LastIndexOf(character);

                                    innerChars = innerChars.Remove(index, 1);
                                }
                            }

                            if (innerChars.Length == 0)
                            {
                                string finalWord = originalprefix + " " + dictWord;

                                if (finalWord.Replace(" ", "").Length == jumbledWord.Length)
                                {
                                    //its an option
                                    finalWord = finalWord.Remove(0, 1);


                                    if (results.Count > 1)
                                    {
                                        //Console.WriteLine("ambiguous");
                                       // return;
                                    }
                                    results.Add(finalWord);
                                }
                                else
                                {
                                    //we found and removed all so its a match
                                    //add it to the queue
                                    string newPrefix = originalprefix + " " + dictWord;
                                    if (!prefixes.Contains(newPrefix))
                                    {
                                        prefixes.Push(newPrefix);
                                    }
                                }

                            }
                        }
                    }

                }
            }


            if (results.Count == 0)
                Console.WriteLine("impossible");
            if (results.Count == 1)
                Console.WriteLine(results[0]);

            return;
        }

    }
}
