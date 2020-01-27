using System;

namespace AdventOfCode_CSharp.Day23
{
    public interface IProcess
    {
        int Identifier { get; set; }
        DateTime WaitingSince { get; set; }
        ComputerStatus Status { get; set; }
        void Start();
        Tuple<int, int> DumpOutput();
        void setInput(int? input);
    }
}