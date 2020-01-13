using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

            var t = doesLineContain100AdjacentSquares(0, intCodes.Select(i => i).ToList());
            Console.WriteLine(t);
            //Scan every horizontal line to see if 100 squares fit, if not just go to next
            //If yes go to next and see if that fits
        }

        public static int doesLineContain100AdjacentSquares(int y, List<long> intCodes)
        {
            var done = false;
            var x = 0;
            var previousOutput = 0;
            var adjacentSquares = 0;
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
                if (previousOutput == 1 && output != 1 && adjacentSquares < 100)
                {
                    done = true;
                    return adjacentSquares;
                }
                else if(output == 1)
                {
                    previousOutput = 1;
                    adjacentSquares++;
                }
                else if(adjacentSquares >= 100)

                {
                    return adjacentSquares;
                }

                x++;
            }
            return 0;
        }
    }
}
