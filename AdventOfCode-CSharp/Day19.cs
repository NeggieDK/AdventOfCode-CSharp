using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode_CSharp.Day7;

namespace AdventOfCode_CSharp
{
    internal class Day19
    {
        public static void Main()
        {
            var intCodes = System.IO.File
                .ReadAllText(
                    "C:\\Users\\aarondk\\source\\repos\\AdventOfCode-CSharp\\AdventOfCode-CSharp\\Resources\\Day19.txt")
                .Split(",").Select(long.Parse).ToList();
            var affectedPoints = 0;
            for (var y = 0; y < 50; y++)
            for (var x = 0; x < 50; x++)
            {
                var intComputer = new IntComputer
                {
                    IntCodes = intCodes.Select(i => i).ToList()
                };
                intComputer.Input.Enqueue(x);
                intComputer.Input.Enqueue(y);
                intComputer.Start();
                if (intComputer.DumpOutput().Item1 == 1)
                    affectedPoints++;
            }

            Console.WriteLine($"Amount of affected points = {affectedPoints}");

            var yLine = 100;
            var xOffset = 0;
            while (true)
            {
                var point = GetLeftMostPointInRow(yLine, xOffset, intCodes.Select(i => i).ToList());
                if (DoesPointExist(point + 99, yLine - 99, intCodes.Select(i => i).ToList()))
                {
                    Console.WriteLine($"Part2: {point * 10000 + (yLine - 99)}");
                    break;
                }

                yLine++;
                xOffset = point;
            }
        }

        public static int GetLeftMostPointInRow(int y, int xOffset, List<long> intCodes)
        {
            var x = xOffset;
            while (true)
            {
                var intComputer = new IntComputer
                {
                    IntCodes = intCodes.Select(i => i).ToList()
                };
                intComputer.Input.Enqueue(x);
                intComputer.Input.Enqueue(y);
                intComputer.Start();

                var output = intComputer.DumpOutput().Item1;
                if (output == 1) return x;
                x++;
            }
        }

        public static bool DoesPointExist(int x, int y, List<long> intCodes)
        {
            var intComputer = new IntComputer
            {
                IntCodes = intCodes.Select(i => i).ToList()
            };
            intComputer.Input.Enqueue(x);
            intComputer.Input.Enqueue(y);
            intComputer.Start();
            var output = intComputer.DumpOutput().Item1;
            return output == 1;
        }
    }
}