using AdventOfCode_CSharp.Day7;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode_CSharp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sw1 = new Stopwatch();
            sw1.Start();
            var result = new List<int>();
            var allPossiblePhases = new List<List<int>>();
            for (var i = 5; i <= 9; i++)
            for (var j = 5; j <= 9; j++)
            {
                if (j == i) continue;
                for (var k = 5; k <= 9; k++)
                {
                    if (k == i || j == k) continue;
                    for (var l = 5; l <= 9; l++)
                    {
                        if (l == i || j == l || l == k) continue;
                        for (var m = 5; m <= 9; m++)
                        {
                            if (m == i || j == m || l == m || m == k) continue;
                            allPossiblePhases.Add(new List<int> {i, j, k, l, m});
                        }
                    }
                }
            }

            var list = new List<long>
            {
                3, 8, 1001, 8, 10, 8, 105, 1, 0, 0, 21, 38, 55, 80, 97, 118, 199, 280, 361, 442, 99999, 3, 9, 101, 2, 9,
                9, 1002, 9, 5, 9, 1001, 9, 4, 9, 4, 9, 99, 3, 9, 101, 5, 9, 9, 102, 2, 9, 9, 1001, 9, 5, 9, 4, 9, 99, 3,
                9, 1001, 9, 4, 9, 102, 5, 9, 9, 101, 4, 9, 9, 102, 4, 9, 9, 1001, 9, 4, 9, 4, 9, 99, 3, 9, 1001, 9, 3,
                9, 1002, 9, 2, 9, 101, 3, 9, 9, 4, 9, 99, 3, 9, 101, 5, 9, 9, 1002, 9, 2, 9, 101, 3, 9, 9, 1002, 9, 5,
                9, 4, 9, 99, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 101,
                1, 9, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9,
                1002, 9, 2, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 99, 3, 9, 102, 2, 9, 9, 4, 9,
                3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4,
                9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4,
                9, 3, 9, 102, 2, 9, 9, 4, 9, 99, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 102, 2, 9,
                9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 101, 1, 9,
                9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 99, 3, 9, 1002,
                9, 2, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 102,
                2, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 101,
                2, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 99, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9,
                1002, 9, 2, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3,
                9, 1002, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9,
                99
            };
            foreach (var phases in allPossiblePhases)
            {
                var thrusterA = new IntComputer();
                thrusterA.IntCodes = list.Select(i => i).ToList();
                thrusterA.Identifier = 1;
                thrusterA.ShareProcessMemory = 2;
                thrusterA.Input.Enqueue(phases[0]);
                thrusterA.Input.Enqueue(0);


                var thrusterB = new IntComputer();
                thrusterB.IntCodes = list.Select(i => i).ToList();
                thrusterB.Identifier = 2;
                thrusterB.ShareProcessMemory = 3;
                thrusterB.Input.Enqueue(phases[1]);

                var thrusterC = new IntComputer();
                thrusterC.IntCodes = list.Select(i => i).ToList();
                thrusterC.Identifier = 3;
                thrusterC.ShareProcessMemory = 4;
                thrusterC.Input.Enqueue(phases[2]);

                var thrusterD = new IntComputer();
                thrusterD.IntCodes = list.Select(i => i).ToList();
                thrusterD.Identifier = 4;
                thrusterD.ShareProcessMemory = 5;
                thrusterD.Input.Enqueue(phases[3]);

                var thrusterE = new IntComputer();
                thrusterE.IntCodes = list.Select(i => i).ToList();
                thrusterE.Identifier = 5;
                thrusterE.ShareProcessMemory = 1;
                thrusterE.Input.Enqueue(phases[4]);

                var scheduler = new Scheduler();
                scheduler.AddActiveProcess(thrusterA);
                scheduler.QueueProcess(thrusterB);
                scheduler.QueueProcess(thrusterC);
                scheduler.QueueProcess(thrusterD);
                scheduler.QueueProcess(thrusterE);
                scheduler.Run();
                result.Add(scheduler.LastOutput);
            }

            Console.WriteLine($"Max output: {result.Max()}");
            sw1.Stop();
            Console.WriteLine($"Execution time: {sw1.ElapsedMilliseconds}");
            //Should give result: 19581200
        }
    }
}

public class Scheduler
{
    public Dictionary<int, IProcess> AllProcesses { get; set; }
    public Dictionary<DateTime, int> WaitingProcesses { get; set; }
    public IProcess ActiveProcess { get; set; }
    public Dictionary<string, int> InterProcessMemory { get; set; }
    private Dictionary<int, int> MemoryPerProcess { get; set; }

    public int LastOutput { get; set; }

    public Scheduler()
    {
        AllProcesses = new Dictionary<int, IProcess>();
        WaitingProcesses = new Dictionary<DateTime, int>();
        MemoryPerProcess = new Dictionary<int, int>();
    }

    public void Run()
    {
        while (ActiveProcess.Status == ComputerStatus.Running || ActiveProcess.Status == ComputerStatus.Waiting ||
               WaitingProcesses.Keys.ToList().Any())
            if (ActiveProcess.Status == ComputerStatus.Running)
            {
                ActiveProcess.Start();
            }
            else if (ActiveProcess.Status == ComputerStatus.Waiting)
            {
                SetSharedMemory(ActiveProcess.DumpOutput());
                WaitingProcesses.Add(ActiveProcess.WaitingSince, ActiveProcess.Identifier);
                ActiveProcess = GetLongestWaitingProcess();
                ActiveProcess.setInput(GetSharedMemory(ActiveProcess.Identifier));
            }
            else
            {
                SetSharedMemory(ActiveProcess.DumpOutput());
                ActiveProcess = GetLongestWaitingProcess();
                ActiveProcess.setInput(GetSharedMemory(ActiveProcess.Identifier));
            }

        LastOutput = ActiveProcess.DumpOutput().Item1;
    }

    private int? GetSharedMemory(int identifier)
    {
        if (!MemoryPerProcess.ContainsKey(identifier)) return null;
        var value = MemoryPerProcess[identifier];
        MemoryPerProcess.Remove(identifier);
        return value;
    }

    private void SetSharedMemory(Tuple<int, int> value)
    {
        if (value == null) return;
        // LastOutput = value.Item1;
        MemoryPerProcess[value.Item2] = value.Item1;
    }

    private IProcess GetLongestWaitingProcess()
    {
        var longestWaitingDateTime = WaitingProcesses.Keys.ToList().Min();
        var process = AllProcesses[WaitingProcesses[longestWaitingDateTime]];
        process.Status = ComputerStatus.Running;
        WaitingProcesses.Remove(longestWaitingDateTime);
        return process;
    }

    public void AddActiveProcess(IProcess process)
    {
        ActiveProcess = process;
        ActiveProcess.Status = ComputerStatus.Running;
        AllProcesses.Add(process.Identifier, process);
    }

    public void QueueProcess(IProcess process)
    {
        process.Status = ComputerStatus.Waiting;
        AllProcesses.Add(process.Identifier, process);
        WaitingProcesses.Add(DateTime.Now, process.Identifier);
    }
}