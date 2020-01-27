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

    class Network
    {
        public Dictionary<int, IntComputer> Computers { get; set; }
        public List<long> IntCodes { get; set; }
        public long NatX { get; set; }
        public long NatY { get; set; }

        public Network(int amount, List<long> intCodes)
        {
            IntCodes = intCodes;
            Computers = new Dictionary<int, IntComputer>();
            for (int i = 0; i < amount; i++)
            {
                var tempComp = new IntComputer
                {
                    Network = this,
                    IntCodes = IntCodes.Select(j => j).ToList(),
                };
                tempComp.Input.Enqueue(i);
                Computers[i] = tempComp;
            }
        }

        public void Start()
        {
            var amountTimesIdle = 0;
            while (Computers.Values.Any(i => i.Status != ComputerStatus.Halted))
            {
                var amountEmptyQueue = 0;
                foreach (var intComputer in Computers)
                {
                    intComputer.Value.Execute();
                    if (intComputer.Value.Status == ComputerStatus.EmptyQueue || (intComputer.Value.Input.Count == 0 && intComputer.Value.Status != ComputerStatus.Output))
                        amountEmptyQueue++;
                }

                if (amountEmptyQueue == 50 )
                    amountTimesIdle++;
                else
                    amountTimesIdle = 0;

                if (amountTimesIdle >= 2)
                {
                    Computers[0].Input.Enqueue(NatX);
                    Computers[0].Input.Enqueue(NatY);
                    amountTimesIdle = 0;
                }

            }
        }
    }
}