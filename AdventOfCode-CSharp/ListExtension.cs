using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode_CSharp
{
    public static class ListExtension
    {
        public static int FindPattern<T>(this List<T> input, List<T> pattern) where T : class
        {
            if (pattern.Exists(i => i == null)) return 0;
            var patterns = 0;
            for (var i = 0; i < input.Count; i++)
            {
                var element = input[i];
                if(element == pattern[0] && i + pattern.Count < input.Count)
                {
                    var found = true;
                    for (var j = 1; j < pattern.Count; j++)
                    {

                        if (input[i + j] == null || pattern[j] == null || !input[i + j].Equals(pattern[j]))
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found) patterns++;
                }
            }
            return patterns;
        }

        public static void ErasePattern<T>(this List<T> input, List<T> pattern) where T : class
        {
            for (var i = 0; i < input.Count; i++)
            {
                var element = input[i];
                if (element == pattern[0] && i + pattern.Count <= input.Count)
                {
                    var found = true;
                    for (var j = 1; j < pattern.Count; j++)
                    {
                        if (input[i + j] == null || pattern[j] == null || !input[i + j].Equals(pattern[j]))
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        for (var j = 0; j < pattern.Count; j++)
                        {
                            input[i + j] = default;
                        }
                    }
                }
            }
        }

        public static List<T> ReplacePattern<T>(this List<T> input, List<T> pattern, T replaceObject) where T : class
        {
            var newList = input.Select(i => i).ToList();
            for (var i = 0; i < input.Count; i++)
            {
                var element = input[i];
                if (element == pattern[0] && i + pattern.Count <= input.Count)
                {
                    var found = true;
                    for (var j = 1; j < pattern.Count; j++)
                    {
                        if (input[i + j] == null || pattern[j] == null || !input[i + j].Equals(pattern[j]))
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        newList[i] = replaceObject;
                        for (var j = 1; j < pattern.Count; j++)
                        {
                            newList[i + j] = default;
                        }
                    }
                }
            }
            return newList.Where(i => i != null).ToList();
        }

        public static int FirstNotNullIndex<T>(this List<T> input, int startIndex) where T : class
        {
            for (var i = startIndex; i < input.Count; i++)
            {
                if (input[i] != null) return i;
            }
            return 0;
        }

        public static List<int> ToASCII(this List<string> input)
        {
            var newList = new List<int>();
            for (int i = 0; i < input.Count; i+=2)
            {
                var element = input[i];
                var element2 = input[i + 1];
                newList.Add(element[0]);
                newList.Add(","[0]);
                newList.Add(element2[0]);
                newList.Add(","[0]);

            }
            newList[newList.Count - 1] = 10;
            return newList;
        }
    }
}
