using System;
using AdventOfCode_CSharp.Day7;

public class Process : IProcess
{
    public string Message { get; set; }
    public int Identifier { get; set; }
    public DateTime WaitingSince { get; set; }
    public ComputerStatus Status { get; set; }
    private int maxAmountTimesRun = 5;
    private int amountTimesRun = 0;
    public int? Input { get; set; }

    public void Start()
    {
        if (amountTimesRun < maxAmountTimesRun)
        {
            amountTimesRun++;
            Console.WriteLine($"Message: {Message}");
            Console.WriteLine($"Input from other process: {Input}");
            Status = ComputerStatus.Waiting;
            WaitingSince = DateTime.Now;
        }
        else
        {
            Status = ComputerStatus.Halted;
        }
    }

    public void setInput(int? input)
    {
        Input = input;
    }

    public Tuple<int, int> DumpOutput()
    {
        return new Tuple<int, int>(Identifier * 10, Identifier + 1);
    }
}