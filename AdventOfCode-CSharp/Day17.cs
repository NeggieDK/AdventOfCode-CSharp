using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode_CSharp.Day17Objects;
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
                        "C:\\Users\\Aaron\\source\\repos\\AdventOfCode-CSharp\\AdventOfCode-CSharp\\Resources\\Day17.txt")
                    .Split(",").Select(long.Parse).ToList()
            };
            intComputer.Start();
            var output = ParseAndPrintOutput(intComputer.DumpFullOutput());
            var intersections = GetIntersections(output);
            Console.WriteLine($"Part1: The sum of the alignment parameters is {intersections.Values.Sum()}");
            var path = GetPaths(output);
            var functions = CreateFunctions(path);
            var intCodes = System.IO.File
                    .ReadAllText(
                        "C:\\Users\\Aaron\\source\\repos\\AdventOfCode-CSharp\\AdventOfCode-CSharp\\Resources\\Day17.txt")
                    .Split(",").Select(long.Parse).ToList();
            intCodes[0] = 2;
            intComputer = new IntComputer
            {
                    IntCodes = intCodes
            };
            foreach(var function in functions)
            {
                foreach(var element in function)
                {
                    intComputer.Input.Enqueue(element);
                }
            }
            intComputer.Input.Enqueue(110);
            intComputer.Input.Enqueue(10);
            intComputer.Start();
            var outputPart2 = intComputer.DumpFullOutput();
            var parsedOutputPart2 = ParseAndPrintOutput(outputPart2);
            Console.WriteLine($"Part2: amount of dust collected: {outputPart2.Last()}");
        }

        public static List<List<int>> CreateFunctions(List<string> input)
        {
            input.RemoveAt(input.Count - 1);
            var patternObjectQueue = new Queue<PatternObject>();
            patternObjectQueue.Enqueue(new PatternObject { Input = input, Patterns = new List<List<string>>(), StartingIndex = 0 });
            var matches = new List<PatternObject>();
            while(patternObjectQueue.Any())
            {
                var amount = 12;
                var currentObject = patternObjectQueue.Dequeue();
                if (currentObject.AllPatternsFound && currentObject.Patterns.Count == 3)
                {
                    matches.Add(currentObject);
                }
                else if (currentObject.Patterns.Count > 3 && !currentObject.AllPatternsFound) continue;
                while (amount >= 4 && !currentObject.AllPatternsFound)
                {
                    amount -= 2;
                    if (amount + currentObject.StartingIndex >= currentObject.Input.Count - 1) continue;
                    var pattern = currentObject.Input.GetRange(currentObject.StartingIndex, amount);
                    if (currentObject.Input.FindPattern(pattern) < 1) continue;
                    var deepCopy = currentObject.Input.Select(i => i).ToList();
                    deepCopy.ErasePattern(pattern);
                    var tempStartingIndex = currentObject.StartingIndex + amount;
                    tempStartingIndex = deepCopy.FirstNotNullIndex(tempStartingIndex);
                    var newPatternObject = new PatternObject
                    {
                        Input = deepCopy,
                        StartingIndex = tempStartingIndex,
                        Patterns = currentObject.Patterns.Select(i => i).ToList()
                    };
                    newPatternObject.Patterns.Add(pattern);
                    patternObjectQueue.Enqueue(newPatternObject);
                }
            }
            if (matches.Count == 0) throw new ArgumentOutOfRangeException();
            var patternObject = matches.First();
            var mainFunction = input.Select(i => i).ToList();
            var functions = new List<string> { "A", "B", "C" };
            var functionIt = 0;
            foreach (var pattern in patternObject.Patterns)
            {
                mainFunction = mainFunction.ReplacePattern(pattern, functions[functionIt]);
                functionIt++;
            }
            return new List<List<int>>
            {
                mainFunction.ToASCII(),
                patternObject.Patterns[0].ToASCII(),
                patternObject.Patterns[1].ToASCII(),
                patternObject.Patterns[2].ToASCII()
            };
        }



        public static List<string> GetPaths(List<List<string>> input)
        {
            var movementList = new List<string>();
            var x = 12;
            var y = 16;
            var (nextTurn, currentDirection) = GetNextTurn(input, "N", x, y);
            movementList.Add(nextTurn);
            var stepsForCurrentDirection = 0;
            while (nextTurn != "X")
            {
                _ = currentDirection switch
                {
                    "N" => y--,
                    "S" => y++,
                    "E" => x++,
                    "W" => x--,
                };
                stepsForCurrentDirection++;
                var nextTurnTemp = GetNextTurn(input, currentDirection, x, y);
                if (nextTurnTemp.Item1 != string.Empty)
                {
                    movementList.Add(stepsForCurrentDirection.ToString());
                    movementList.Add(nextTurnTemp.Item1);
                    stepsForCurrentDirection = 0;
                }
                (nextTurn, currentDirection) = nextTurnTemp;
            }
            return movementList;
        }

        public static Tuple<string, string> GetNextTurn(List<List<string>> input, string currentDirection, int x, int y)
        {
            switch (currentDirection)
            {
                case "N":
                    if (y != 0 && input[y - 1][x] == "#") return new Tuple<string, string>(string.Empty, currentDirection);
                    if (x != input[0].Count - 1 && input[y][x + 1] == "#") return new Tuple<string, string>("R", "E");
                    if (x != 0 && input[y][x - 1] == "#") return new Tuple<string, string>("L", "W");
                    break;
                case "S":
                    if (y != input.Count-1 && input[y + 1][x] == "#") return new Tuple<string, string>(string.Empty, currentDirection);
                    if (x != input[0].Count - 1 && input[y][x + 1] == "#") return new Tuple<string, string>("L", "E");
                    if (x != 0 && input[y][x - 1] == "#") return new Tuple<string, string>("R", "W");
                    break;
                case "E":
                    if (x != input[0].Count - 1 && input[y][x + 1] == "#") return new Tuple<string, string>(string.Empty, currentDirection);
                    if (y != 0 && input[y - 1][x] == "#") return new Tuple<string, string>("L", "N");
                    if (y != input.Count - 1 && input[y + 1][x] == "#") return new Tuple<string, string>("R", "S");
                    break;
                case "W":
                    if (x != 0 && input[y][x - 1] == "#") return new Tuple<string, string>(string.Empty, currentDirection);
                    if (y != 0 && input[y - 1][x] == "#") return new Tuple<string, string>("R", "N");
                    if (y != input.Count - 1 && input[y + 1][x] == "#") return new Tuple<string, string>("L", "S");
                    break;
            }
            return new Tuple<string, string>("X", string.Empty);
        }

        public static Dictionary<string, int> GetIntersections(List<List<string>> input)
        {
            var intersections = new Dictionary<string, int>();
            for (var i = 0; i < input.Count; i++)
            {
                for (var j = 0; j < input[i].Count; j++)
                {
                    if(IsIntersection(input, j, i)) intersections.Add($"{j},{i}", j*i);
                }
            }
            return intersections;
        }

        public static bool IsIntersection(List<List<string>> input, int x, int y)
        {
            if (x <= 1 || x >= input[y].Count - 2 || y <= 1 || y >= input.Count - 2) return false;
            return (input[y][x] == "#" || input[y][x] == "^") && input[y + 1][x] == "#" && input[y - 1][x] == "#" &&
                   input[y][x + 1] == "#" && input[y][x - 1] == "#";
        }

        public static List<List<string>> ParseAndPrintOutput(List<long> output)
        {
            var resultList = new List<List<string>>();
            var tempList = new List<string>();
            foreach (var element in output)
            {
                if (element != 10)
                {
                    Console.Write(((char)element).ToString());
                    tempList.Add(((char)element).ToString());
                }
                else
                {
                    Console.WriteLine();
                    if(tempList.Any())
                        resultList.Add(tempList.Select(i => i).ToList());
                    tempList.Clear();
                }
            }
            return resultList;
        }
    }
}
