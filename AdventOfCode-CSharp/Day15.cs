using System;
using AdventOfCode_CSharp.Day7;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;

namespace AdventOfCode_CSharp
{
    internal class Day15
    {
        public static void Main()
        {
            var input = System.IO.File
                .ReadAllText(
                    "C:\\Users\\Aaron\\source\\repos\\AdventOfCode-CSharp\\AdventOfCode-CSharp\\Resources\\Day15.txt")
                .Split(",").Select(long.Parse).ToList();
            var checkPoints = new Queue<CheckPoint>();
            var moveStacks = new Stack<Direction2>();
            var environment = new Dictionary<string, int>();
            environment.Add("0,0", 1);
            var currentState = State.CheckNeighbours;
            var amountNeighboursChecked = 0;
            CheckPoint currentCheckPoint = null;
            var currentX = 0;
            var currentY = 0;
            var currentDirection = Direction2.West;
            var tries = 0;
            var goBack = false;
            var skipInput = false;
            var found = false;
            var stepsToCoolerList = new List<int>();
            var intComputer = new IntComputer
            {
                IntCodes = input
            };
            intComputer.Start();
            while ((intComputer.Status == ComputerStatus.Running || intComputer.Status == ComputerStatus.Waiting) &&
                   !found)
                if (intComputer.Status == ComputerStatus.Waiting)
                {
                    //Move the robot according to the output
                    var output = intComputer.DumpOutput()?.Item1;
                    if (output == 1 || output == 2)
                    {
                        var (item1, item2) = UpdateDirection(currentX, currentY, currentDirection);
                        currentX = item1;
                        currentY = item2;
                        Console.WriteLine($"Current at point: {currentX}, {currentY}");
                    }

                    if (true)
                    {
                        if (currentState == State.CheckNeighbours)
                        {
                            if (goBack == false)
                            {
                                if (amountNeighboursChecked == 0) currentDirection = Direction2.North;
                                if (amountNeighboursChecked == 1) currentDirection = Direction2.South;
                                if (amountNeighboursChecked == 2) currentDirection = Direction2.West;
                                if (amountNeighboursChecked == 3) currentDirection = Direction2.East;
                                moveStacks.Push(currentDirection);
                                goBack = true;
                                amountNeighboursChecked++;
                                Console.WriteLine($"Checking neighbour {currentDirection}");
                            }
                            else
                            {
                                if (output == 0)
                                {
                                    var tempPosition = UpdateDirection(currentX, currentY, currentDirection);
                                    if (!environment.ContainsKey($"{tempPosition.Item1},{tempPosition.Item2}"))
                                        environment.Add($"{tempPosition.Item1},{tempPosition.Item2}", 0);
                                    Console.WriteLine("Neighbour is a wall");
                                    moveStacks.Pop();
                                    skipInput = true;
                                }
                                else if (output == 1 || output == 2)
                                {
                                    if (output == 2) stepsToCoolerList.Add(moveStacks.Count);
                                    var checkPoint = new CheckPoint
                                    {
                                        X = currentX,
                                        Y = currentY,
                                        DirectionsToPoint = StackExtensions.ToReverseQueue(moveStacks)
                                    };
                                    Console.WriteLine("Neighbour is not a wall");
                                    Console.WriteLine("Going back to checkpoint");
                                    if (!environment.ContainsKey($"{currentX},{currentY}"))
                                    {
                                        environment.Add($"{currentX},{currentY}", output.GetValueOrDefault());
                                        checkPoints.Enqueue(checkPoint);
                                    }

                                    currentDirection = GetOppositeDirection(moveStacks.Pop());
                                }

                                if (amountNeighboursChecked >= 4)
                                {
                                    currentState = State.GoingBack;
                                    amountNeighboursChecked = 0;
                                }

                                goBack = false;
                            }
                        }
                        else if (currentState == State.GoingBack)
                        {
                            Console.WriteLine("Going back");
                            if (moveStacks.Count > 0)
                            {
                                currentDirection = GetOppositeDirection(moveStacks.Pop());
                            }
                            else
                            {
                                currentState = State.GoingToPoint;
                                skipInput = true;
                            }
                        }
                        else if (currentState == State.GoingToPoint)
                        {
                            Console.WriteLine("Going to new point");
                            if (currentCheckPoint != null && currentCheckPoint.DirectionsToPoint.Count > 0)
                            {
                                if (output == 1 || output == 2)
                                {
                                    currentDirection = currentCheckPoint.DirectionsToPoint.Dequeue();
                                    moveStacks.Push(currentDirection);
                                }
                                else
                                {
                                    throw new ArgumentException();
                                }
                            }
                            else if (currentCheckPoint != null && currentCheckPoint.DirectionsToPoint.Count == 0)
                            {
                                currentState = State.CheckNeighbours;
                                currentCheckPoint = null;
                            }
                            else
                            {
                                if (checkPoints.Count == 0)
                                {
                                    found = true;
                                }
                                else
                                {
                                    currentCheckPoint = checkPoints.Dequeue();
                                    currentDirection = currentCheckPoint.DirectionsToPoint.Dequeue();
                                    moveStacks.Push(currentDirection);
                                }
                            }

                            if (currentCheckPoint != null && currentCheckPoint.DirectionsToPoint.Count == 0)
                                currentState = State.CheckNeighbours;
                        }
                    }

                    if (skipInput)
                    {
                        skipInput = false;
                    }
                    else
                    {
                        Console.WriteLine($"Current direction: {currentDirection}");
                        Console.WriteLine();
                        intComputer.Input.Enqueue((int) currentDirection + 1);
                        intComputer.Start();
                        //Thread.Sleep(10);
                    }
                }

            Console.WriteLine($"Least amount of steps to cooler: {stepsToCoolerList.Min()}");
            //printHashMap(environment);

            var generations = new Dictionary<string, int>();
            var pointsToCheck = new Queue<string>();
            pointsToCheck.Enqueue("14,20");
            generations.Add("14,20", 0);
            while (pointsToCheck.Count > 0)
            {
                var point = pointsToCheck.Dequeue();
                if (!environment.ContainsKey(point)) continue;
                var pointX = int.Parse(point.Split(",")[0]);
                var pointY = int.Parse(point.Split(",")[1]);
                var generation = generations[point];
                if (environment.ContainsKey($"{pointX},{pointY + 1}") && environment[$"{pointX},{pointY + 1}"] == 1)
                    if (!generations.ContainsKey($"{pointX},{pointY + 1}"))
                    {
                        pointsToCheck.Enqueue($"{pointX},{pointY + 1}");
                        generations.Add($"{pointX},{pointY + 1}", generation + 1);
                    }

                if (environment.ContainsKey($"{pointX},{pointY - 1}") && environment[$"{pointX},{pointY - 1}"] == 1)
                    if (!generations.ContainsKey($"{pointX},{pointY - 1}"))
                    {
                        pointsToCheck.Enqueue($"{pointX},{pointY - 1}");
                        generations.Add($"{pointX},{pointY - 1}", generation + 1);
                    }

                if (environment.ContainsKey($"{pointX + 1},{pointY}") && environment[$"{pointX + 1},{pointY}"] == 1)
                    if (!generations.ContainsKey($"{pointX + 1},{pointY}"))
                    {
                        pointsToCheck.Enqueue($"{pointX + 1},{pointY}");
                        generations.Add($"{pointX + 1},{pointY}", generation + 1);
                    }

                if (environment.ContainsKey($"{pointX - 1},{pointY}") && environment[$"{pointX - 1},{pointY}"] == 1)
                    if (!generations.ContainsKey($"{pointX - 1},{pointY}"))
                    {
                        pointsToCheck.Enqueue($"{pointX - 1},{pointY}");
                        generations.Add($"{pointX - 1},{pointY}", generation + 1);
                    }
            }

            Console.WriteLine($"Part 2: it takes {generations.Values.Max()} minutes for the room to fill with oxygen");
        }

        public static void printHashMap(Dictionary<string, int> map)
        {
            for (var i = 50; i > -50; i--)
            {
                for (var j = -50; j < 50; j++)
                {
                    if (!map.ContainsKey($"{j},{i}"))
                    {
                        Console.Write("F");
                        continue;
                    }

                    if (i == 0 && j == 0)
                    {
                        Console.Write("O");
                        continue;
                    }

                    var value = map[$"{j},{i}"];
                    if (value == 0)
                        Console.Write("#");
                    else if (value == 1)
                        Console.Write(" ");
                    else if (value == 2)
                        Console.Write("X");
                }

                Console.WriteLine();
            }
        }

        public static Tuple<int, int> UpdateDirection(int x, int y, Direction2 direction)
        {
            switch (direction)
            {
                case Direction2.East:
                    x++;
                    break;
                case Direction2.West:
                    x--;
                    break;
                case Direction2.North:
                    y++;
                    break;
                default:
                    y--;
                    break;
            }

            return new Tuple<int, int>(x, y);
        }

        public static Direction2 GetNextDirection(Direction2 direction)
        {
            var intDirection = (int) direction + 1;
            return intDirection >= 3 ? (Direction2) 0 : (Direction2) intDirection;
        }

        public static Direction2 GetOppositeDirection(Direction2 direction)
        {
            switch (direction)
            {
                case Direction2.East:
                    return Direction2.West;
                case Direction2.West:
                    return Direction2.East;
                case Direction2.North:
                    return Direction2.South;
                default:
                    return Direction2.North;
            }
        }
    }

    public class CheckPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Queue<Direction2> DirectionsToPoint { get; set; }
    }

    public enum Direction2
    {
        North,
        South,
        West,
        East
    }

    public enum State
    {
        GoingBack,
        GoingToPoint,
        CheckNeighbours
    }

    public class StackExtensions
    {
        public static Queue<Direction2> ToReverseQueue(Stack<Direction2> stack)
        {
            var queue = new Queue<Direction2>();
            var stackList = stack.ToList();
            stackList.Reverse();
            foreach (var direction in stackList) queue.Enqueue(direction);
            return queue;
        }

        public static Direction2 GetOppositeDirection(Direction2 direction)
        {
            switch (direction)
            {
                case Direction2.East:
                    return Direction2.West;
                case Direction2.West:
                    return Direction2.East;
                case Direction2.North:
                    return Direction2.South;
                default:
                    return Direction2.North;
            }
        }
    }
}