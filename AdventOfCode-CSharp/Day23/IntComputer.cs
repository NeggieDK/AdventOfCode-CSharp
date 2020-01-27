using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode_CSharp.Day23
{
    internal class IntComputer : IProcess
    {
        private long InstructionPointer { get; set; }
        public Queue<long> Input { get; set; }
        public List<long> Output { get; set; }
        public ComputerStatus Status { get; set; }
        public List<long> IntCodes { get; set; }
        public long RelativeBase { get; set; }
        public Network Network { get; set; }

        public IntComputer()
        {
            Output = new List<long>();
            Input = new Queue<long>();
        }
        
        public ComputerStatus Execute()
        {
            if (InstructionPointer == 0) IntCodes.AddRange(new long[5000]);
            var test = IntCodes.Count - InstructionPointer - 4 > 0 ? 4 : IntCodes.Count - InstructionPointer;
            var currentInstruction = new Instruction(IntCodes.GetRange((int) InstructionPointer, (int) test));
            var result = ExecuteInstruction(currentInstruction);
            InstructionPointer += result.Item2;
            Status = result.Item1;
            return result.Item1;
        }

        private Tuple<ComputerStatus, long> ExecuteInstruction(Instruction instruction)
        {
            if (instruction.Opcode == 1)
            {
                var result = GetValue(instruction.Param1Value) + GetValue(instruction.Param2Value);
                SetValue(result, instruction.Param3Value);
                return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 4);
            }
            else if (instruction.Opcode == 2)
            {
                var result = GetValue(instruction.Param1Value) * GetValue(instruction.Param2Value);
                SetValue(result, instruction.Param3Value);
                return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 4);
            }
            else if (instruction.Opcode == 3)
            {
                var result = 0L;
                if (!Input.Any())
                {
                    result = -1L;
                    SetValue(result, instruction.Param1Value);
                    return new Tuple<ComputerStatus, long>(ComputerStatus.EmptyQueue, 2); 
                }
                result = Input.Dequeue();
                SetValue(result, instruction.Param1Value);
                return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 2);
            }
            else if (instruction.Opcode == 4)
            {
                var result = GetValue(instruction.Param1Value);
                Output.Add(result);
                if (Output.Count == 3)
                {
                    if (!Network.Computers.ContainsKey((int) Output[0]))
                    {
                        if (Output[0] == 255)
                        {
                            Network.NatX = Output[1];
                            Network.NatY = Output[2];
                        }
                        else
                            Console.WriteLine($"Output to unknown computer {Output[0]} with value X={Output[1]} and Y={Output[2]}");
                        Output.Clear();
                    }
                    else
                    {
                        Network.Computers[(int)Output[0]].Input.Enqueue(Output[1]);
                        Network.Computers[(int)Output[0]].Input.Enqueue(Output[2]);
                        Output.Clear();
                    }
                }
                return new Tuple<ComputerStatus, long>(ComputerStatus.Output, 2);
            }
            else if (instruction.Opcode == 5)
            {
                var result = GetValue(instruction.Param1Value);
                if (result != 0)
                    return new Tuple<ComputerStatus, long>(ComputerStatus.Running,
                        GetValue(instruction.Param2Value) - InstructionPointer);
                return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 3);
            }
            else if (instruction.Opcode == 6)
            {
                var result = GetValue(instruction.Param1Value);
                if (result == 0)
                    return new Tuple<ComputerStatus, long>(ComputerStatus.Running,
                        GetValue(instruction.Param2Value) - InstructionPointer);
                return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 3);
            }
            else if (instruction.Opcode == 7)
            {
                var param1 = GetValue(instruction.Param1Value);
                var param2 = GetValue(instruction.Param2Value);
                if (param1 < param2)
                {
                    SetValue(1, instruction.Param3Value);
                    return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 4);
                }

                SetValue(0, instruction.Param3Value);
                return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 4);
            }
            else if (instruction.Opcode == 8)
            {
                var param1 = GetValue(instruction.Param1Value);
                var param2 = GetValue(instruction.Param2Value);
                if (param1 == param2)
                {
                    SetValue(1, instruction.Param3Value);
                    return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 4);
                }

                SetValue(0, instruction.Param3Value);
                return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 4);
            }
            else if (instruction.Opcode == 9)
            {
                RelativeBase += GetValue(instruction.Param1Value);
                return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 2);
            }
            else if (instruction.Opcode == 99)
            {
                return new Tuple<ComputerStatus, long>(ComputerStatus.Halted, 0);
            }

            throw new InvalidOperationException();
        }

        private void SetValue(long value, Tuple<ParameterMode, long> adress)
        {
            if (adress.Item1 == ParameterMode.Position)
                IntCodes[(int) adress.Item2] = value;
            else if (adress.Item1 == ParameterMode.Relative)
                IntCodes[(int) RelativeBase + (int) adress.Item2] = value;
        }

        private long GetValue(Tuple<ParameterMode, long> value)
        {
            if (value.Item1 == ParameterMode.Immediate)
                return value.Item2;
            else if (value.Item1 == ParameterMode.Position)
                return IntCodes[(int) value.Item2];
            else
                return IntCodes[(int) RelativeBase + (int) value.Item2];
        }
    }
}