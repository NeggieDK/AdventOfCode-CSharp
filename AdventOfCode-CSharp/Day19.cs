using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode_CSharp.Day7;

namespace AdventOfCode_CSharp
{
    class Day19
    {
        public static void Main()
        {
            var intCodes = System.IO.File
                .ReadAllText(
                    "C:\\Users\\aarondk\\source\\repos\\AdventOfCode-CSharp\\AdventOfCode-CSharp\\Resources\\Day19.txt")
                .Split(",").Select(long.Parse).ToList();
            var scannedItems = new Dictionary<string, int>();
            for (var y = 0; y < 50; y++)
            {
                for (var x = 0; x < 50; x++)
                {
                    var intComputer = new IntComputer
                    {
                        IntCodes = intCodes.Select(i => i).ToList()
                    };
                    intComputer.Input.Enqueue(x);
                    intComputer.Input.Enqueue(y);
                    intComputer.Start();
                    var output = intComputer.DumpOutput().Item1;
                    scannedItems[$"{x},{y}"] = output;
                    Console.Write(output == 1 ? "#" : ".");
                }

                Console.WriteLine();
            }

            var found = false;
            var yLine = 0;
            while (!found)
            {
                var quantityHorizontal = QuantityAdjacentSquaresHorizontal(yLine, intCodes.Select(i => i).ToList());
                if (quantityHorizontal.Amount >= 100)
                {
                    var adjacentWith100Depth = 0;
                    for (var xLine = quantityHorizontal.EndX; xLine >= quantityHorizontal.StartX; xLine--)
                    {
                        var quantityVertical = QuantityAdjacentSquaresVertical(xLine, yLine, intCodes.Select(i => i).ToList());
                        if (quantityVertical.Amount < 100) 
                            break;
                        adjacentWith100Depth++;
                        if (adjacentWith100Depth >= 100)
                        {
                            Console.WriteLine($"Part2: {xLine*1000+yLine}");
                            break;
                            found = true;
                        }
                    }
                }

                yLine++;
            }
            //Correct answer = 18651593 (calculated with very inefficient slow code :()
        }

        public static HorizontalLine QuantityAdjacentSquaresHorizontal(int y, List<long> intCodes)
        {
            var done = false;
            var x = 0;
            var previousOutput = 0;
            var startX = 0;
            var endX = 0;
                
            while (!done)
            {
                var intComputer = new IntComputer
                {
                    IntCodes = intCodes.Select(i => i).ToList()
                };
                intComputer.Input.Enqueue(x);
                intComputer.Input.Enqueue(y);
                intComputer.Start();
                var output = intComputer.DumpOutput().Item1;
                if (previousOutput == 1 && output == 0)
                {
                    endX = x - 1;
                    return new HorizontalLine
                    {
                        EndX = endX,
                        StartX = startX,
                        Amount = endX - startX + 1,
                        Y = y
                    };
                }
                else if (previousOutput == 0 && output == 1)
                {
                    startX = x ;
                }

                if (x == 5000) done = true;
                previousOutput = output;
                x++;
            }
            return new HorizontalLine
            {
                Y = 0,
                EndX = 0,
                StartX = 0,
                Amount = 0
            };
        }

        public static VerticalLine QuantityAdjacentSquaresVertical(int x, int yOffset, List<long> intCodes)
        {
            var done = false;
            var y = yOffset;
            var previousOutput = 0;
            var startY = 0;
            var endY = 0;
                
            while (!done)
            {
                var intComputer = new IntComputer
                {
                    IntCodes = intCodes.Select(i => i).ToList()
                };
                intComputer.Input.Enqueue(x);
                intComputer.Input.Enqueue(y);
                intComputer.Start();
                var output = intComputer.DumpOutput().Item1;
                if (previousOutput == 1 && output == 0)
                {
                    endY = y - 1;
                    return new VerticalLine
                    {
                        EndY = endY,
                        StartY = startY,
                        Amount = endY - startY + 1,
                        X = x
                    };
                }
                else if (previousOutput == 0 && output == 1)
                {
                    startY = y;
                }

                if (y == 5000) done = true;
                previousOutput = output;
                y++;
            }
            return new VerticalLine
            {
                X = 0,
                EndY = 0,
                StartY = 0,
                Amount = 0
            };
        }
    }

    class HorizontalLine
    {
        public int StartX { get; set;}
        public int EndX { get; set; }
        public int Amount { get; set; }
        public int Y { get; set; }
    }

    class VerticalLine
    {
        public int StartY { get; set;}
        public int EndY { get; set; }
        public int Amount { get; set; }
        public int X { get; set; }
    }
}
