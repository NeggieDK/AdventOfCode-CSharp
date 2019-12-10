using AdventOfCode_CSharp.Day7;
using System;

public interface IProcess
{
    int Identifier { get;set; }
    DateTime WaitingSince { get; set; }
    ComputerStatus Status { get; set; }
    void Start();
    Tuple<int, int> DumpOutput();
    void setInput(int? input);
}
