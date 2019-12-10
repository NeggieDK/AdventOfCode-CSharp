﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode_CSharp.Day7
{
    class IntComputer : IProcess
    {
        public int Identifier { get; set; }
        public DateTime WaitingSince { get; set; }
        int InstructionPointer { get; set; }
        public Queue<int> Input { get; set; }
        public List<int> Output { get; set; }
        public ComputerStatus Status { get; set; }
        public List<int> IntCodes { get; set; }
        public int ShareProcessMemory { get; set; }

        public IntComputer()
        {
            Output = new List<int>();
            Input = new Queue<int>();
        }

        public void Start()
        {
            while (Execute() == ComputerStatus.Running);
            if(Status == ComputerStatus.Waiting)
                WaitingSince = DateTime.Now;
            return;
        }

        public ComputerStatus Execute()
        {
            var test = IntCodes.Count - InstructionPointer - 4 > 0 ? 4 : IntCodes.Count - InstructionPointer;
            var currentInstruction = new Instruction(IntCodes.GetRange(InstructionPointer, test));
            var result = executeInstruction(currentInstruction);
            InstructionPointer += result.Item2;
            Status = result.Item1;
            return result.Item1;
        }

        private Tuple<ComputerStatus, int> executeInstruction(Instruction instruction)
        {
            if(instruction.Opcode == 1)
            {
                var result = getValue(instruction.Param1Value) + getValue(instruction.Param2Value);
                setValue(result, instruction.Param3Value.Item2);
                return new Tuple<ComputerStatus, int>(ComputerStatus.Running, 4);
            }
            else if(instruction.Opcode == 2)
            {
                var result = getValue(instruction.Param1Value) * getValue(instruction.Param2Value);
                setValue(result, instruction.Param3Value.Item2);
                return new Tuple<ComputerStatus, int>(ComputerStatus.Running, 4);

            }
            else if (instruction.Opcode == 3)
            {
                if(!Input.Any()) return new Tuple<ComputerStatus, int>(ComputerStatus.Waiting, 0);
                var result = Input.Dequeue();
                setValue(result, instruction.Param1Value.Item2);
                return new Tuple<ComputerStatus, int>(ComputerStatus.Running, 2);

            }
            else if (instruction.Opcode == 4)
            {
                var result = getValue(instruction.Param1Value);
                Output.Add(result);
                return new Tuple<ComputerStatus, int>(ComputerStatus.Running, 2);

            }
            else if (instruction.Opcode == 5)
            {
                var result = getValue(instruction.Param1Value);
                if(result != 0)
                {
                    return new Tuple<ComputerStatus, int>(ComputerStatus.Running, getValue(instruction.Param2Value)-InstructionPointer);
                }
                return new Tuple<ComputerStatus, int>(ComputerStatus.Running, 3);
            }
            else if (instruction.Opcode == 6)
            {
                var result = getValue(instruction.Param1Value);
                if (result == 0)
                {
                    return new Tuple<ComputerStatus, int>(ComputerStatus.Running, getValue(instruction.Param2Value)-InstructionPointer);
                }
                return new Tuple<ComputerStatus, int>(ComputerStatus.Running, 3);

            }
            else if (instruction.Opcode == 7)
            {
                var param1 = getValue(instruction.Param1Value);
                var param2 = getValue(instruction.Param2Value);
                if (param1 < param2)
                {
                    setValue(1, instruction.Param3Value.Item2);
                    return new Tuple<ComputerStatus, int>(ComputerStatus.Running, 4);
                }
                setValue(0, instruction.Param3Value.Item2);
                return new Tuple<ComputerStatus, int>(ComputerStatus.Running, 4);

            }
            else if (instruction.Opcode == 8)
            {
                var param1 = getValue(instruction.Param1Value);
                var param2 = getValue(instruction.Param2Value);
                if (param1 == param2)
                {
                    setValue(1, instruction.Param3Value.Item2);
                    return new Tuple<ComputerStatus, int>(ComputerStatus.Running, 4);
                }
                setValue(0, instruction.Param3Value.Item2);
                return new Tuple<ComputerStatus, int>(ComputerStatus.Running, 4);
            }
            else if(instruction.Opcode == 99)
            {
                return new Tuple<ComputerStatus, int>(ComputerStatus.Halted, 0);
            }
            throw new InvalidOperationException();
        }

        private void setValue(int value, int adress)
        {
            IntCodes[adress] = value;
        }

        private int getValue(Tuple<ParameterMode, int> value)
        {
            if(value.Item1 == ParameterMode.Immediate)
            {
                return value.Item2;
            }
            else
            {
                return IntCodes[value.Item2];
            }
        }

        public Tuple<int, int> DumpOutput()
        {
            if (!Output.Any()) return null;
            var value = new Tuple<int, int>(Output[Output.Count-1], ShareProcessMemory);
            Output.Clear();
            return value;
        }

        public void setInput(int? input)
        {
            if(input != null)
                Input.Enqueue(input.GetValueOrDefault());
        }
    }
}