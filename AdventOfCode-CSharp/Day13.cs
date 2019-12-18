using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode_CSharp.Day7;
using Microsoft.VisualBasic;

namespace AdventOfCode_CSharp
{
    class Day13
    {
        static Dictionary<string, long> _gameSurface = new Dictionary<string, long>();
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllText("C:\\Users\\aarondk\\source\\repos\\AdventOfCode-CSharp\\AdventOfCode-CSharp\\Resources\\Day13.txt").Split(",").Select(long.Parse).ToList();
            var intComputer = new IntComputer
            {
                IntCodes = input
            };
            intComputer.Start();
            while(intComputer.Status == ComputerStatus.Running || intComputer.Status == ComputerStatus.Waiting)
            {
                if(intComputer.Status == ComputerStatus.Waiting)
                {
                    ParseOutput(intComputer.DumpFullOutput());
                    intComputer.Output.Clear();
                    var ball = _gameSurface.First(i => i.Value == 4).Key.Split(";").Select(int.Parse).ToList();
                    Console.WriteLine($"Ball: ({ball[0]}, {ball[1]})");
                    var slab = _gameSurface.First(i => i.Value == 3).Key.Split(";").Select(int.Parse).ToList();
                    Console.WriteLine($"Slab: ({slab[0]}, {slab[1]})");
                    var blocksRemaning = _gameSurface.Values.Count(i => i == 2);
                    Console.WriteLine($"Blocks remaining: {blocksRemaning}");
                    Console.WriteLine("");
                    if (ball[0] > slab[0])
                        {
                            intComputer.Input.Enqueue(1);
                        }
                        else if (ball[0] < slab[0])
                        {
                            intComputer.Input.Enqueue(-1);
                        }
                        else
                        {
                            intComputer.Input.Enqueue(0);
                        }
                    intComputer.Start();
                }
            } 
            ParseOutput(intComputer.Output);
            
        }

        static void ParseOutput(List<long> output)
        {
            for (var i = 0; i < output.Count; i += 3)
            {
                var x = output[i];
                var y = output[i+1];
                var id = output[i+2];
                if(_gameSurface.ContainsKey($"{x};{y}"))
                    _gameSurface[$"{x};{y}"] = id;
                else
                    _gameSurface.Add($"{x};{y}", id);

                if (x == -1 && y == 0)
                {
                    Console.WriteLine($"Score: {id}");
                }
            }
        }
    }
}
