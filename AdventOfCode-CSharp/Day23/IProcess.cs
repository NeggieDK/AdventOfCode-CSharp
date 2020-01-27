using System;

namespace AdventOfCode_CSharp.Day23
{
    public interface IProcess
    {
        ComputerStatus Status { get; set; }
    }
}