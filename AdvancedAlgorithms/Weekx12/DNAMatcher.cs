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
    public class DNAMatcher
    {
        private static void Main(string[] args)
        {
            int numTestCases = int.Parse(Console.ReadLine());
            for (int testCaseNum = 1; testCaseNum <= numTestCases; testCaseNum++)
            {
                string firstString = Console.ReadLine();
                string secondString = Console.ReadLine();

                SuffixTree tree = new SuffixTree();
                tree.ConstructTree(firstString + "$", 1);
                tree.ConstructTree(secondString + "@", 2);
                string longestSubstring = tree.FindLongestCommonSubstring("$", "@");
                //  tree.root.Output();

                //find the longest common substring
                Console.WriteLine("Test {0}: {1}-{2}", testCaseNum, longestSubstring.Length, longestSubstring);
            }
        }

        public class SuffixTree
        {
            public TreeNode root;
            public SuffixTree()
            {
                this.root = new TreeNode();
            }

            /// <summary>
            /// Constructs a tree with the supplied text 
            /// 
            /// Does this by looping through the string character by character
            /// from start to end. Creates suffixes for each character from that point on 
            /// 
            /// ie 
            /// SOCCER
            /// OCCER
            /// CCER
            /// CER
            /// ER
            /// R
            /// 
            /// on first loop then adds those to the tree
            /// When those are added to the tree, the tree is checked to see if they already contain those exact ones
            /// If the first one is there, its rmeoved, adn second one is checked, this is dwindled until it gets to the last X about
            /// that aren't already present. These are added as a subset of the current position
            /// ie if it gets to SOCC and realises that CER isnt there it adds CER ER and R as children
            /// 
            /// this is repeated again with 
            /// OCCER 
            /// CCER
            /// CER
            /// ER
            /// R
            /// 
            /// and so on
            /// 
            /// </summary>
            public void ConstructTree(string text, int stringNumber)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    //create a list of all suffixes from this letter on
                    //this list will be dwindled down to only new suffixes
                    List<String> suffixList = new List<String>();
                    for (int k = i; k < text.Length; k++)
                        suffixList.Add(text[k] + "");
                    this.root.AddSuffix(suffixList, stringNumber);
                }
            }

            private TreeNode deepestCommonNode;
            //find the longest substring that ends in both ending1 and ending2
            public string FindLongestCommonSubstring(string ending1, string ending2)
            {
                //find the deepest node that has both ending1 and ending2 in its children
                deepestCommonNode = root;

                //traverse the nodes to find the deepest common one
                Traverse(root);
                TreeNode currentNode = deepestCommonNode;
                string output = "";
                while (currentNode != root)
                {
                    output = currentNode.Value + output;
                    currentNode = currentNode.parent;
                }

                return output;
            }

            public void Traverse(TreeNode parent)
            {
                foreach (TreeNode child in parent.Children)
                {
                    //calculate string coverage
                    if (child.BelongsToStrings.Count == 2)
                    {
                        if (child.stringDepth > deepestCommonNode.nodeDepth)
                            deepestCommonNode = child;

                        Traverse(child);
                    }
                }
            }
        }

        /// <summary>
        /// Represents a treenode in the suffix tree
        /// </summary>
        public class TreeNode
        {
            public string Value;
            public int nodeDepth;
            public IList<TreeNode> Children = new List<TreeNode>();
            public TreeNode parent;

            public HashSet<int> BelongsToStrings = new HashSet<int>(); //used to identify different words in the tree
            public int stringDepth;

            public TreeNode(TreeNode parent, string incomingLabel, int depth, int stringNumber)
            {
                this.Value = incomingLabel;
                this.nodeDepth = depth;
                this.parent = parent;
                this.stringDepth = parent.stringDepth + incomingLabel.Length;

                //mark it as belonging to a certain string
                this.BelongsToStrings.Add(stringNumber);
            }
            public TreeNode() { }

            public void AddSuffix(List<string> suffix, int stringNumber)
            {

                //find position to insert at 
                TreeNode insertAt = Search(this, suffix, stringNumber);
                Insert(insertAt, suffix, stringNumber);
            }

            /// <summary>
            /// Searches for the node to insert the suffixes under
            /// Dwindles down the suffix list to remove ones already inserted
            /// </summary>
            public TreeNode Search(TreeNode startNode, List<string> suffix, int stringNumber)
            {
                foreach (TreeNode child in startNode.Children)
                {
                    if (child.Value.Equals(suffix[0]))
                    {
                        //its already there 
                        //check that its belonging to this string number
                        if (!child.BelongsToStrings.Contains(stringNumber))
                            child.BelongsToStrings.Add(stringNumber);

                        suffix.RemoveAt(0);
                        if (suffix.Count == 0)
                            return child;

                        return Search(child, suffix, stringNumber);
                    }
                }
                return startNode;
            }

            //Inserts the suffixes at the certain position
            private void Insert(TreeNode insertAt, List<string> suffix, int stringNumber)
            {
                foreach (string x in suffix)
                {
                    TreeNode child = new TreeNode(insertAt, x, insertAt.nodeDepth + 1, stringNumber);
                    insertAt.Children.Add(child);
                    insertAt = child;
                }
            }

            /// <summary>
            /// Outputs a pretty poor representation of the tree.
            /// </summary>
            public void Output()
            {
                string output = "";
                for (int i = 1; i <= this.nodeDepth; i++)
                    output += " ";

                if (!string.IsNullOrEmpty(this.Value))
                {
                    output += this.Value + " strings  ";
                    foreach (int stringNumber in this.BelongsToStrings)
                    {
                        output += stringNumber + ", ";
                    }
                }
                Console.Write(output);

                foreach (TreeNode child in Children)
                    child.Output();

                if (Children.Count == 0)
                    Console.WriteLine();
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

