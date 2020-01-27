using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode_CSharp.Day23
{
    public class Instruction
    {
        public Instruction(List<long> instructionList)
        {
            Parse(instructionList);
        }

        public int Opcode { get; set; }
        public Tuple<ParameterMode, long> Param1Value { get; set; }
        public Tuple<ParameterMode, long> Param2Value { get; set; }
        public Tuple<ParameterMode, long> Param3Value { get; set; }

        private List<int> ParsedOpcodes { get; set; }

        public void Parse(List<long> instructionList)
        {
            while (instructionList.Count < 4) instructionList.Add(0);
            parseOpcode(instructionList[0]);
            var size = ParsedOpcodes.Count;
            if (size >= 2)
                Opcode = ParsedOpcodes[size - 2] * 10 + ParsedOpcodes[size - 1];
            else
                Opcode = ParsedOpcodes[size - 1];

            size -= 2;

            if (size > 0)
                Param1Value = new Tuple<ParameterMode, long>((ParameterMode) ParsedOpcodes[--size], instructionList[1]);
            else
                Param1Value = new Tuple<ParameterMode, long>(0, instructionList[1]);

            if (size > 0)
                Param2Value = new Tuple<ParameterMode, long>((ParameterMode) ParsedOpcodes[--size], instructionList[2]);
            else
                Param2Value = new Tuple<ParameterMode, long>(0, instructionList[2]);

            if (size > 0)
                Param3Value = new Tuple<ParameterMode, long>((ParameterMode) ParsedOpcodes[--size], instructionList[3]);
            else
                Param3Value = new Tuple<ParameterMode, long>(0, instructionList[3]);
        }

        private void parseOpcode(long opcode)
        {
            ParsedOpcodes = opcode.ToString().Select(i => int.Parse(i.ToString())).ToList();
        }
    }
}