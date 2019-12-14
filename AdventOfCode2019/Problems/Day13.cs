﻿using AdventOfCode2019.Utilities;
using AdventOfCode2019.Utilities.TwoDimensions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019.Problems
{
    public class Day13 : Problem<int>
    {
        private const int gridSize = 10000;

        public override int RunPart1() => General(Part1GeneralFunction, Part1Returner);
        public override int RunPart2() => General(Part2GeneralFunction, Part2Returner);

        private void Part1GeneralFunction(IntcodeComputer computer) { }
        private void Part2GeneralFunction(IntcodeComputer computer) => computer.SetStaticMemoryAt(0, 2);

        private int Part1Returner(Dictionary<TileType, int> tileCounts, int score) => tileCounts[TileType.Block];
        private int Part2Returner(Dictionary<TileType, int> tileCounts, int score) => score;

        private T General<T>(GeneralFunction beforeOperation, Returner<T> returner)
        {
            int startingRow = Console.CursorTop;

            var grid = new TileType[gridSize, gridSize];
            int outputs = 0;
            bool gameStarted = false;

            var currentLocation = new Location2D();
            var tileCounts = new Dictionary<TileType, int>
            {
                { TileType.Empty, 0 },
                { TileType.Wall, 0 },
                { TileType.Block, 0 },
                { TileType.HorizontalPaddle, 0 },
                { TileType.Ball, 0 },
            };
            int score = 0;

            var currentBallLocation = new Location2D();
            var currentBallVelocity = new Location2D(1);
            var currentPaddleLocation = new Location2D();

            var computer = new IntcodeComputer(FileContents);
            computer.InputRequested += InputRequested;
            computer.OutputWritten += OutputWritten;

            beforeOperation(computer);

            computer.RunToHalt();

            return returner(tileCounts, score);

            BigInteger InputRequested()
            {
                gameStarted = true;

                PrintGrid();

                var newBallLocation = currentBallLocation + currentBallVelocity;

                int paddleMovement = currentBallLocation.X.CompareTo(currentPaddleLocation.X);

                return paddleMovement;

                void CheckBoundary(int x, int y, Location2D newVelocity)
                {
                    if (grid[x, y] > TileType.Empty)
                    {
                        if (grid[x, y] == TileType.Block)
                            grid[x, y] = TileType.Empty;
                        currentBallVelocity = newVelocity;
                        newBallLocation = currentBallLocation + currentBallVelocity;
                    }
                }
            }
            void OutputWritten(BigInteger output)
            {
                int intput = (int)output;
                //Console.WriteLine(intput);
                switch (outputs % 3)
                {
                    case 0:
                        currentLocation.X = intput;
                        break;
                    case 1:
                        currentLocation.Y = intput;
                        break;
                    case 2:
                        if (currentLocation == (-1, 0))
                            score = intput;
                        else
                        {
                            var (x, y) = currentLocation;
                            tileCounts[grid[x, y]]--;
                            tileCounts[grid[x, y] = (TileType)intput]++;
                            switch (grid[x, y])
                            {
                                case TileType.Ball:
                                    if (gameStarted)
                                        currentBallVelocity = currentLocation - currentBallLocation;
                                    currentBallLocation = currentLocation;
                                    break;
                                case TileType.HorizontalPaddle:
                                    currentPaddleLocation = currentLocation;
                                    break;
                            }
                        }
                        break;
                }
                outputs++;
            }


            void PrintGrid()
            {
                Console.SetCursorPosition(0, startingRow);

                var builder = new StringBuilder();
                for (int y = 0; y < 25; y++)
                {
                    for (int x = 0; x < 50; x++)
                        builder.Append(grid[x, y] switch
                        {
                            TileType.Empty => ' ',
                            TileType.Wall => '#',
                            TileType.Block => '+',
                            TileType.HorizontalPaddle => '_',
                            TileType.Ball => 'O',
                        });
                    builder.AppendLine();
                }
                Console.Write(builder.ToString());
            }
        }

        private enum TileType : byte
        {
            Empty,
            Wall,
            Block,
            HorizontalPaddle,
            Ball
        }

        private delegate void GeneralFunction(IntcodeComputer computer);
        private delegate T Returner<T>(Dictionary<TileType, int> tileCounts, int score);
    }
}