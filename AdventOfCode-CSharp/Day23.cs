using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode_CSharp.Day23;

namespace AdventOfCode_CSharp
{
    internal class Day23Main
    {
        public static void Main()
        {
            var intCodes = System.IO.File
                .ReadAllText(
                    "C:\\Users\\aarondk\\source\\repos\\AdventOfCode-CSharp\\AdventOfCode-CSharp\\Resources\\Day23.txt")
                .Split(",").Select(long.Parse).ToList();
            var network = new Network(50, intCodes);
            network.Start();
        }
    }

    internal class Network
    {
        public Dictionary<int, IntComputer> Computers { get; set; }
        public List<long> IntCodes { get; set; }
        public long NatX { get; set; }
        public long NatY { get; set; }
        public long PrevNatYSend { get; set; }
        public Dictionary<long, List<Tuple<long, long>>> Memory { get; set; }

        public Network(int amount, List<long> intCodes)
        {
            Memory = new Dictionary<long, List<Tuple<long, long>>>();
            IntCodes = intCodes;
            Computers = new Dictionary<int, IntComputer>();
            for (var i = 0; i < amount; i++)
            {
                var newIntCodes = IntCodes.Select(j => j).ToList();
                newIntCodes.AddRange(new long[5000]);
                var tempComp = new IntComputer
                {
                    Identifier = i,
                    Network = this,
                    IntCodes = newIntCodes
                };
                tempComp.Input.Enqueue(i);
                Computers[i] = tempComp;
            }
        }

        public void Start()
        {
            var idleConsecutive = 0;
            var timesIdle = 0;
            while (Computers.Values.Any(i => i.Status != ComputerStatus.Halted))
            {
                var outputTotal = 0;
                var emptyInputTotal = 0;
                var inputTotal = 0;
                var inputCount = Computers.Sum(i => i.Value.Input.Count);
                foreach (var intComputer in Computers)
                {
                    intComputer.Value.Execute();
                    if (intComputer.Value.Status == ComputerStatus.Output)
                        outputTotal++;

                    if (intComputer.Value.Status == ComputerStatus.EmptyQueue)
                        emptyInputTotal++;

                    if (intComputer.Value.Status == ComputerStatus.Input)
                        inputTotal++;
                }

                ShareMemory();

                if (outputTotal == 0 && inputTotal == 0 && inputCount == 0 && Memory.Count == 0 && emptyInputTotal != 0)
                    timesIdle++;
                else
                    timesIdle = 0;

                if (timesIdle > 1000)
                {
                    //if (NatX == 0 && NatY == 0) Computers[0].Input.Enqueue(-1);
                    if (PrevNatYSend != 0 && PrevNatYSend == NatY)
                    {
                        Console.WriteLine($"Part2: {NatY}");
                        break;
                    }

                    Computers[0].Input.Enqueue(NatX);
                    Computers[0].Input.Enqueue(NatY);
                    PrevNatYSend = NatY;
                }
            }
        }

        public void ShareMemory()
        {
            for (var i = 0; i < 50; i++)
                if (Memory.TryGetValue(i, out var valueList))
                {
                    foreach (var value in valueList)
                    {
                        Computers[i].Input.Enqueue(value.Item1);
                        Computers[i].Input.Enqueue(value.Item2);
                    }

                    Memory.Remove(i);
                }
        }
    }
}