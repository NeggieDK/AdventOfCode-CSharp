using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode_CSharp.Day7;

namespace AdventOfCode_CSharp
{
    class Day17
    {
        public static void Main()
        {
            var intComputer = new IntComputer
            {
                IntCodes = System.IO.File
                    .ReadAllText(
                        "C:\\Users\\aarondk\\source\\repos\\AdventOfCode-CSharp\\AdventOfCode-CSharp\\Resources\\Day17.txt")
                    .Split(",").Select(long.Parse).ToList()
            };
            intComputer.Start();
            var output = intComputer.DumpFullOutput();
            var parsedOutput = ParseOutput(output);
            var intersections = GetIntersections(parsedOutput);
            Console.WriteLine($"Part1: The sum of the alignment parameters is {intersections.Values.Sum()}");
            //The split the path into different functions (A,B,C ...) print out the whole path. And search for repeating patterns.
        }

        public static List<List<string>> ParseOutput(List<long> output)
        {
            var resultList = new List<List<string>>();
            var tempList = new List<string>();
            foreach (var element in output)
            {
                if (element != 10)
                {
                    tempList.Add(element == 35 ? "#" : ".");
                }
                else
                {
                    resultList.Add(tempList.Select(i => i).ToList());
                    tempList.Clear();
                }
            }
            return resultList;
        }

        public static Dictionary<string, int> GetIntersections(List<List<string>> input)
        {
            var intersections = new Dictionary<string, int>();
            for (var i = 0; i < input.Count; i++)
            {
                for (var j = 0; j < input[i].Count; j++)
                {
                    if(IsIntersection(input, j, i))
                        Console.Write("O");
                    else
                        Console.Write(input[i][j]);
                    if(IsIntersection(input, j, i)) intersections.Add($"{j},{i}", j*i);
                }

                Console.WriteLine();
            }

            return intersections;
        }

        public static bool IsIntersection(List<List<string>> input, int x, int y)
        {
            if (x <= 1 || x >= input[y].Count - 2 || y <= 1 || y >= input.Count - 2) return false;
            return (input[y][x] == "#" || input[y][x] == "^") && input[y + 1][x] == "#" && input[y - 1][x] == "#" &&
                   input[y][x + 1] == "#" && input[y][x - 1] == "#";
        }
    }
}
