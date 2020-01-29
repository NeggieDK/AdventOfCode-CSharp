using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace AdventOfCode_CSharp.Day23
{
    internal class IntComputer : IProcess
    {
        public int Identifier { get; set; }
        private long InstructionPointer { get; set; }
        public Queue<long> Input { get; set; }
        public List<long> Output { get; set; }
        public ComputerStatus Status { get; set; }
        public List<long> IntCodes { get; set; }
        public long RelativeBase { get; set; }
        public Network Network { get; set; }
        public int? DestinationAddress { get; set; }
        public int PackagesSent { get; set; }

        public IntComputer()
        {
            DestinationAddress = null;
            Input = new Queue<long>();
            Output = new List<long>();
        }

        public ComputerStatus Execute()
        {
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
                return new Tuple<ComputerStatus, long>(ComputerStatus.Input, 2);
            }
            else if (instruction.Opcode == 4)
            {
                var result = GetValue(instruction.Param1Value);
                Output.Add(result);
                if (Output.Count == 3)
                {
                    Console.WriteLine($"MACHINE ID: {Identifier} | OP: 4 (OUTPUT) | VAL: {Output[0]}");
                    Console.WriteLine($"MACHINE ID: {Identifier} | OP: 4 (OUTPUT) | VAL: {Output[1]}");
                    Console.WriteLine($"MACHINE ID: {Identifier} | OP: 4 (OUTPUT) | VAL: {Output[2]}");
                    if (Output[0] == 255)
                    {
                        Network.NatX = Output[1];
                        Network.NatY = Output[2];
                    }
                    else
                    {
                        if (Network.Memory.ContainsKey(Output[0]))
                        {
                            Network.Memory[Output[0]].Add(new Tuple<long, long>(Output[1], Output[2]));
                        }
                        else
                        {
                            Network.Memory[Output[0]] = new List<Tuple<long, long>>();
                            Network.Memory[Output[0]].Add(new Tuple<long, long>(Output[1], Output[2]));
                        }
                    }

                    Output.Clear();
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