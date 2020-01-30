using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode_CSharp.Day7;

namespace AdventOfCode_CSharp
{
    class Day25
    {
        public static void Main()
        {
            var intCodes = System.IO.File
                .ReadAllText(
                    "C:\\Users\\aarondk\\source\\repos\\AdventOfCode-CSharp\\AdventOfCode-CSharp\\Resources\\Day25.txt")
                .Split(",").Select(long.Parse).ToList();

            var rooms = new Dictionary<string, Room>();
            var intComputer = new IntComputer
            {
                IntCodes = intCodes.Select(i => i).ToList()
            };
            intComputer.Start();
            while (intComputer.Status == ComputerStatus.Running || intComputer.Status == ComputerStatus.Waiting)
            {
                var output = intComputer.DumpFullOutput();
                foreach (var el in output)
                {
                    Console.Write((char) el);
                }

                var prsdOutput = ParseOutput(output);
                rooms[GetRoomName(prsdOutput)] = new Room
                {
                    Items = GetItemList(prsdOutput),
                    Name = GetRoomName(prsdOutput)
                };
                if (intComputer.Status == ComputerStatus.Waiting)
                {
                    var input = Console.ReadLine();
                    foreach (var el in input)
                    {
                        intComputer.Input.Enqueue(el);
                    }

                    intComputer.Input.Enqueue(10);
                }

                intComputer.Start();
            }
        }

        public static List<List<long>> ParseOutput(List<long> output)
        {
            var outputList = new List<List<long>>();
            outputList.Add(new List<long>());
            var it = 0;
            foreach (var el in output)
            {
                if (el != 10)
                    outputList[it].Add(el);
                else
                {
                    outputList.Add(new List<long>());
                    it++;
                }
            }

            return outputList.Where(i => i.Count > 0).ToList();
        }

        public static string GetRoomName(List<List<long>> output)
        {
            return output[0].ConvertToString().Replace("=", "").Trim();
        }

        public static List<string> GetItemList(List<List<long>> output)
        {
            var itemList = new List<string>();
            var indexOfItems = 0;
            var it = 0;
            foreach (var row in output)
            {
                it++;
                if (row.ConvertToString().Contains("Items here:"))
                {
                    indexOfItems = it;
                    break;
                }
            }

            if (indexOfItems == 0) return itemList;
            for(var i = indexOfItems;i<output.Count;i++)
            {
                var currString = output[i].ConvertToString();
                if (currString.Trim().StartsWith("-"))
                {
                    itemList.Add(currString.Replace("-", "").Trim());
                }
            }

            return itemList;
        }
        public static List<string> GetDoors(List<List<long>> output)
        {
            var itemList = new List<string>();
            var indexOfItems = 0;
            var it = 0;
            foreach (var row in output)
            {
                it++;
                if (row.ConvertToString().Contains("Doors here lead:"))
                {
                    indexOfItems = it;
                    break;
                }
            }

            if (indexOfItems == 0) return itemList;
            for(var i = indexOfItems;i<output.Count;i++)
            {
                var currString = output[i].ConvertToString();
                if (currString.Trim().StartsWith("-"))
                {
                    itemList.Add(currString.Replace("-", "").Trim());
                }
            }

            return itemList;
        }
    }

    public class Room
    {
        public Room()
        {
            Items = new List<string>();
            PathToRoom = new List<string>();
        }
        public List<string> Items { get; set; }
        public string Name { get; set; }
        public List<string> PathToRoom { get; set; }
    }
}
