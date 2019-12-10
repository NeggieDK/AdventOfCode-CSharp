using AdventOfCode_CSharp.Day7;
using System;
using System.Collections.Generic;

namespace AdventOfCode_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<int> {3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,
1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0 };
            var thrusterA = new IntComputer();
            thrusterA.IntCodes = list;

            var thrusterB = new IntComputer();
            thrusterB.IntCodes = list;

            var thrusterC = new IntComputer();
            thrusterC.IntCodes = list;

            var thrusterD = new IntComputer();
            thrusterD.IntCodes = list;

            var thrusterE = new IntComputer();
            thrusterE.IntCodes = list;

            thrusterA.Input.Enqueue(1);
            thrusterA.Input.Enqueue(0);
            thrusterA.Start();

            thrusterB.Input.Enqueue(0);
            thrusterB.Input.Enqueue(thrusterA.Output[thrusterA.Output.Count - 1]);
            thrusterB.Start();

            thrusterC.Input.Enqueue(4);
            thrusterC.Input.Enqueue(thrusterB.Output[thrusterB.Output.Count - 1]);
            thrusterC.Start();

            thrusterD.Input.Enqueue(3);
            thrusterD.Input.Enqueue(thrusterC.Output[thrusterC.Output.Count - 1]);
            thrusterD.Start();

            thrusterE.Input.Enqueue(2);
            thrusterE.Input.Enqueue(thrusterD.Output[thrusterD.Output.Count - 1]);
            thrusterE.Start();


            Console.WriteLine(thrusterE.Output[thrusterE.Output.Count-1]);
            Console.WriteLine();
        }
    }
}
