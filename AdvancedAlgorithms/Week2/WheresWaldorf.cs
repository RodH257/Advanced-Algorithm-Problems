using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdvancedAlgorithms
{
    /// <summary>
    /// Wheres Waldorf homework problem - Rod Howarth n6294685 
    /// </summary>
    public class WheresWaldorf
    {
        private const int NOT_INITIALIZED = -1;
        private const char NULL_CHARACTER = '^';
        static void Main(string[] args)
        {
            int numberOfRuns = int.Parse(Console.ReadLine());

            for (int runNumber = 0; runNumber < numberOfRuns; runNumber++)
            {
                //get the space 
                Console.ReadLine();
                int numLines = NOT_INITIALIZED;
                int lineLength = NOT_INITIALIZED;
                //read the grid dimensions
                string dimensionline = Console.ReadLine();

                foreach (var number in dimensionline.Split(' '))
                {
                    if (numLines == NOT_INITIALIZED)
                        numLines = int.Parse(number);
                    else
                        lineLength = int.Parse(number);
                }

                numLines += 2;
                lineLength += 2;


                char[][] grid = new char[numLines][];
                //got our grid size, read all the lines
                for (int lineNumber = 0; lineNumber < numLines; lineNumber++)
                {
                    grid[lineNumber] = new char[lineLength];

                    string lineString = string.Empty;
                    //if its not an edge, read the line
                    if (lineNumber != 0 && lineNumber != numLines - 1)
                        lineString = Console.ReadLine().ToLower();

                    //read in the line
                    for (int linePosition = 0; linePosition < lineLength; linePosition++)
                    {
                        //if its the edges, fill it with -'s 
                        if (linePosition == 0 || lineNumber == 0 || lineNumber == numLines - 1 || linePosition == lineLength - 1)
                        {
                            grid[lineNumber][linePosition] = NULL_CHARACTER;
                        }
                        else
                        {

                            //otherwise, read a character
                            grid[lineNumber][linePosition] = lineString[linePosition - 1];
                        }
                    }
                }

                //grid is set, now do the searching.
                int numberOfSearches = int.Parse(Console.ReadLine());

                for (int searchNumber = 0; searchNumber < numberOfSearches; searchNumber++)
                {
                    string textToFind = Console.ReadLine().ToLower();

                    Console.WriteLine(FindTextInGrid(textToFind, grid));
                }
                if (runNumber != numberOfRuns - 1)
                    Console.WriteLine();
            }
        }

        /// <summary>
        /// Finds the specificed in the grid
        /// can be vertical, horizontal, diagonals, backwards or forwards
        /// </summary>
        /// <returns></returns>
        private static string FindTextInGrid(string wordToFind, char[][] grid)
        {
            char firstCharacter = wordToFind[0];
            //dont search start and end lines
            for (int line = 1; line < grid.Length - 1; line++)
            {
                //search each position for first character
                for (int position = 1; position < grid[line].Length - 1; position++)
                {
                    if (grid[line][position] == firstCharacter)
                    {
                        //found the first character, now we need to see if any of the directions have the right value

                        bool found = FindWordStartingFromCharacter(line, position, grid, 1, wordToFind);
                        if (found)
                            return line + " " + position;
                    }
                }
            }

            return "NOT FOUND!";
        }

        /// <summary>
        /// After finding the first character, continues on
        /// </summary>
        static bool FindWordStartingFromCharacter(int lineInSearch, int positionInSearch, char[][] grid, int foundCharacters, string wordToFind)
        {
            if (foundCharacters == wordToFind.Length)
            {
                //we're at the end, found them all
                return true;
            }
            int startingLineInSearch = lineInSearch;
            int startingPositionInSearch = positionInSearch;
            //look above, even, below lines
            for (int x = -1; x <= 1; x++)
            {
                lineInSearch = startingLineInSearch + x;
                //look left center right
                for (int y = -1; y <= 1; y++)
                {
                    positionInSearch = startingPositionInSearch + y;
                    if (x == 0 && y == 0)
                    {
                        //searching the same character, skip it
                        continue;
                    }

                    if (grid[lineInSearch][positionInSearch] == wordToFind[foundCharacters])
                    {
                        //found the second character, check from this point on recursively
                        bool result = CheckLine(grid, lineInSearch, positionInSearch, x, y, foundCharacters + 1,
                                                wordToFind);
                        if (result)
                            return true;

                    }
                }
            }

            return false;
        }


        /// <summary>
        /// Recursively checks a line in a certain direction
        /// </summary>
        static bool CheckLine(char[][] grid, int lineInSearch, int positionInSearch, int lineModifier,
            int positionModifier, int foundCharacters, string wordToFind)
        {
            if (foundCharacters == wordToFind.Length)
            {
                //we're at the end, found them all
                return true;
            }

            //add the line modifiers to the positions to keep it going in the same direction
            lineInSearch += lineModifier;
            positionInSearch += positionModifier;

            //check for going off edge
            if (lineInSearch == grid.Length || positionInSearch == grid[lineInSearch].Length)
                return false;

            //see if we have a match
            char currentChar = grid[lineInSearch][positionInSearch];
            if (currentChar == wordToFind[foundCharacters])
            {
                //found it, continue on
                return CheckLine(grid, lineInSearch, positionInSearch, lineModifier, positionModifier,
                                 foundCharacters + 1, wordToFind);
            }
            //no match 
            return false;
        }

    }
}
