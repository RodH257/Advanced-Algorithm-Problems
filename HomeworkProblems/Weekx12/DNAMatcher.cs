using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeworkProblems
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

            public void ConstructTree(string text, int stringNumber)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    //create a list of all suffixes from this letter on
                    //this list will be dwindled
                    List<String> suffixList = new List<String>();
                    for (int k = i; k < text.Length; k++)
                    {
                        suffixList.Add(text[k] + "");
                    }
                    this.root.AddSuffix(suffixList, stringNumber);
                }
            }

            private TreeNode deepestCommonNode;
            //find the longest substring that ends in both ending1 and ending2
            public string FindLongestCommonSubstring(string ending1, string ending2)
            {
                //find the deepest node that has both ending1 and ending2 in its children
                deepestCommonNode = root;

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

        public class TreeNode
        {
            public string Value;
            public int nodeDepth;
            public IList<TreeNode> Children = new List<TreeNode>();
            public TreeNode parent;
            public HashSet<int> BelongsToStrings = new HashSet<int>();
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

            private void Insert(TreeNode insertAt, List<string> suffix, int stringNumber)
            {
                foreach (string x in suffix)
                {
                    TreeNode child = new TreeNode(insertAt, x, insertAt.nodeDepth + 1, stringNumber);
                    insertAt.Children.Add(child);
                    insertAt = child;
                }
            }

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

        //public class TreeEdge
        //{
        //    public String Value = null;

        //    public TreeEdge(String value)
        //    {
        //        this.Value = value;
        //    }

        //}


        //public class SuffixTree
        //{
        //    public Node Root;

        //    public void ConstructTree(string text, int stringNumber)
        //    {
        //        Root = new Node();


        //        List<string> suffixlist = new List<string>();

        //        //setup  a list of all variations on the word 
        //        //ie all suffixes
        //        for (int i =0; i < text.Length; i++)
        //        {
        //            string newPrefix = text.Substring(i, text.Length-1);
        //            suffixlist.Add(newPrefix);
        //        }

        //        //now merge those into a tree

        //        foreach (string suffix in suffixlist)
        //        {


        //        }

        //        //foreach (char character in text)
        //        //{
        //        //    List<string> suffixlist = new List<string>();

        //        //    //create a suffix for every character
        //        //    foreach (char suffixChar in text)
        //        //    {
        //        //        suffixlist.Add(suffixChar.ToString());
        //        //    }

        //        //    Root.AddSuffix(suffixlist);
        //        //}
        //    }
        //}

        //public class Node : IEquatable<Node>
        //{
        //    public HashSet<int> BelongsToStrings = new HashSet<int>();
        //    public string Value;
        //    public IList<Node> Children = new List<Node>();

        //    public void AddSuffix(string suffix)
        //    {
        //          Node insertAt = this;
        //          insertAt = FindNodeToInsertSuffix(this, suffixList);

        //        //find the node with the biggest substring  

        //        //insert it there

        //    }


        //    public Node FindNodeToInsertSuffix(Node start, List<string> suffix)
        //    {
        //        foreach (Node child in start.Children)
        //        {
        //            if (child.Value.)
        //        }
        //    }

        //    public void AddCharacter(char character, int stringNumber)
        //    {
        //        string charString = character.ToString();
        //        //create node
        //        Node newNode = new Node() { Value = charString };
        //        newNode.BelongsToStrings.Add(stringNumber);

        //        //if we aren't the root, add the character to our string
        //        if (!string.IsNullOrEmpty(Value))
        //        {
        //            Value += character;
        //        }

        //        foreach (Node child in Children)
        //        {
        //            //we want to append the prefix to each of them. 
        //            child.AddCharacter(character, stringNumber);
        //        }

        //        if (string.IsNullOrEmpty(Value))
        //            //add a new child for it 
        //            Children.Add(newNode);
        //    }

        //    public bool Equals(Node other)
        //    {
        //        return this.Value == other.Value;
        //    }


        //    public void Output()
        //    {
        //        if (!string.IsNullOrEmpty(Value))
        //            Console.Write(Value + " - ");
        //        foreach (Node child in Children)
        //        {
        //            child.Output();
        //        }

        //        if (Children.Count == 0)
        //            Console.WriteLine();
        //    }
        //}

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

