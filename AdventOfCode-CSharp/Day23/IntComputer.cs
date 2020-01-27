using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode_CSharp.Day23
{
    internal class IntComputer : IProcess
    {
        public int Identifier { get; set; }
        public DateTime WaitingSince { get; set; }
        private long InstructionPointer { get; set; }
        public Queue<long> Input { get; set; }
        public List<long> Output { get; set; }
        public ComputerStatus Status { get; set; }
        public List<long> IntCodes { get; set; }
        public int ShareProcessMemory { get; set; }
        public long RelativeBase { get; set; }

        public IntComputer()
        {
            Output = new List<long>();
            Input = new Queue<long>();
        }

        public void Start()
        {
            if (InstructionPointer == 0) IntCodes.AddRange(new long[5000]);
            while (Execute() == ComputerStatus.Running) ;
            if (Status == ComputerStatus.Waiting)
                WaitingSince = DateTime.Now;
            return;
        }

        public ComputerStatus Execute()
        {
            var test = IntCodes.Count - InstructionPointer - 4 > 0 ? 4 : IntCodes.Count - InstructionPointer;
            var currentInstruction = new Instruction(IntCodes.GetRange((int) InstructionPointer, (int) test));
            var result = executeInstruction(currentInstruction);
            InstructionPointer += result.Item2;
            Status = result.Item1;
            return result.Item1;
        }

        private Tuple<ComputerStatus, long> executeInstruction(Instruction instruction)
        {
            if (instruction.Opcode == 1)
            {
                var result = getValue(instruction.Param1Value) + getValue(instruction.Param2Value);
                setValue(result, instruction.Param3Value);
                return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 4);
            }
            else if (instruction.Opcode == 2)
            {
                var result = getValue(instruction.Param1Value) * getValue(instruction.Param2Value);
                setValue(result, instruction.Param3Value);
                return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 4);
            }
            else if (instruction.Opcode == 3)
            {
                if (!Input.Any()) return new Tuple<ComputerStatus, long>(ComputerStatus.Waiting, 0);
                var result = Input.Dequeue();
                setValue(result, instruction.Param1Value);
                return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 2);
            }
            else if (instruction.Opcode == 4)
            {
                var result = getValue(instruction.Param1Value);
                Output.Add(result);
                //Console.WriteLine(result);
                return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 2);
            }
            else if (instruction.Opcode == 5)
            {
                var result = getValue(instruction.Param1Value);
                if (result != 0)
                    return new Tuple<ComputerStatus, long>(ComputerStatus.Running,
                        getValue(instruction.Param2Value) - InstructionPointer);
                return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 3);
            }
            else if (instruction.Opcode == 6)
            {
                var result = getValue(instruction.Param1Value);
                if (result == 0)
                    return new Tuple<ComputerStatus, long>(ComputerStatus.Running,
                        getValue(instruction.Param2Value) - InstructionPointer);
                return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 3);
            }
            else if (instruction.Opcode == 7)
            {
                var param1 = getValue(instruction.Param1Value);
                var param2 = getValue(instruction.Param2Value);
                if (param1 < param2)
                {
                    setValue(1, instruction.Param3Value);
                    return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 4);
                }

                setValue(0, instruction.Param3Value);
                return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 4);
            }
            else if (instruction.Opcode == 8)
            {
                var param1 = getValue(instruction.Param1Value);
                var param2 = getValue(instruction.Param2Value);
                if (param1 == param2)
                {
                    setValue(1, instruction.Param3Value);
                    return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 4);
                }

                setValue(0, instruction.Param3Value);
                return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 4);
            }
            else if (instruction.Opcode == 9)
            {
                RelativeBase += getValue(instruction.Param1Value);
                return new Tuple<ComputerStatus, long>(ComputerStatus.Running, 2);
            }
            else if (instruction.Opcode == 99)
            {
                return new Tuple<ComputerStatus, long>(ComputerStatus.Halted, 0);
            }

            throw new InvalidOperationException();
        }

        private void setValue(long value, Tuple<ParameterMode, long> adress)
        {
            if (adress.Item1 == ParameterMode.Position)
                IntCodes[(int) adress.Item2] = value;
            else if (adress.Item1 == ParameterMode.Relative)
                IntCodes[(int) RelativeBase + (int) adress.Item2] = value;
        }

        private long getValue(Tuple<ParameterMode, long> value)
        {
            if (value.Item1 == ParameterMode.Immediate)
                return value.Item2;
            else if (value.Item1 == ParameterMode.Position)
                return IntCodes[(int) value.Item2];
            else
                return IntCodes[(int) RelativeBase + (int) value.Item2];
        }

        public Tuple<int, int> DumpOutput()
        {
            if (!Output.Any()) return null;
            var value = new Tuple<int, int>((int) Output[Output.Count - 1], ShareProcessMemory);
            Output.Clear();
            return value;
        }

        public List<long> DumpFullOutput()
        {
            if (!Output.Any()) return null;
            var value = Output.Select(i => i).ToList();
            Output.Clear();
            return value;
        }

        public void setInput(int? input)
        {
            if (input != null)
                Input.Enqueue(input.GetValueOrDefault());
        }
    }
}