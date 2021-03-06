﻿using System;
using System.Linq;
using AdventOfCode_CSharp.Day7;

namespace AdventOfCode_CSharp
{
    internal class Day21
    {
        public static void Main()
        {
            var intCodes = System.IO.File
                .ReadAllText(
                    "C:\\Users\\aarondk\\source\\repos\\AdventOfCode-CSharp\\AdventOfCode-CSharp\\Resources\\Day21.txt")
                .Split(",").Select(long.Parse).ToList();
            var instructions = System.IO.File
                .ReadAllText(
                    "C:\\Users\\aarondk\\source\\repos\\AdventOfCode-CSharp\\AdventOfCode-CSharp\\Resources\\Day21Instruction.txt");
            var instructions2 = System.IO.File
                .ReadAllText(
                    "C:\\Users\\aarondk\\source\\repos\\AdventOfCode-CSharp\\AdventOfCode-CSharp\\Resources\\Day21InstructionP2.txt");
            var intComputer = new IntComputer
            {
                IntCodes = intCodes.Select(i => i).ToList()
            };
            foreach (var el in instructions)
            {
                if (el == 13) continue;
                intComputer.Input.Enqueue(el);
            }

            intComputer.Start();
            var output = intComputer.DumpFullOutput();
            foreach (var el in output) Console.Write((char) el);
            Console.WriteLine($"Part1: amount of hull damage {output.Last()}");
            var intComputer2 = new IntComputer
            {
                IntCodes = intCodes.Select(i => i).ToList()
            };
            foreach (var el in instructions2)
            {
                if (el == 13) continue;
                intComputer2.Input.Enqueue(el);
            }

            intComputer2.Start();
            var output2 = intComputer2.DumpFullOutput();
            foreach (var el in output2) Console.Write((char) el);
            Console.WriteLine($"Part2: amount of hull damage {output2.Last()}");
        }
    }
}