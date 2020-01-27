using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode_CSharp.Day17Objects
{
    public class PatternObject
    {
        public List<List<string>> Patterns { get; set; }
        public List<string> Input { get; set; }
        public int StartingIndex { get; set; }

        public bool AllPatternsFound
        {
            get { return Input.All(i => i == null); }
        }
    }
}