﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AdventOfCode2019.Utilities.TwoDimensions
{
    public struct DirectionalLocation
    {
        private static Dictionary<Direction, Location2D> locations = new Dictionary<Direction, Location2D>
        {
            { Direction.Up, (0, 1) },
            { Direction.Down, (0, -1) },
            { Direction.Left, (-1, 0) },
            { Direction.Right, (1, 0) },
        };
        private static Direction[] orderedDirections =
        {
            Direction.Up,
            Direction.Right,
            Direction.Down,
            Direction.Left,
        };
        private static Dictionary<Direction, int> orderedDirectionsIndices;

        private int directionIndex;

        public Direction Direction
        {
            get => orderedDirections[directionIndex];
            set => directionIndex = orderedDirectionsIndices[value];
        }
        public Location2D LocationOffset
        {
            get
            {
                var l = locations[Direction];
                if (InvertX)
                    l = l.InvertX;
                if (InvertY)
                    l = l.InvertY;
                return l;
            }
        }
        
        public bool InvertX { get; set; }
        public bool InvertY { get; set; }

        static DirectionalLocation()
        {
            orderedDirectionsIndices = new Dictionary<Direction, int>(4);
            for (int i = 0; i < 4; i++)
                orderedDirectionsIndices.Add(orderedDirections[i], i);
        }

        public DirectionalLocation(Direction d, bool invertX = false, bool invertY = false)
            : this()
        {
            InvertX = invertX;
            InvertY = invertY;
            Direction = d;
        }

        public Direction TurnLeft() => Turn(3);
        public Direction TurnRight() => Turn(1);

        private Direction Turn(int indexAdjustment)
        {
            directionIndex = (directionIndex + indexAdjustment) % 4;
            return Direction;
        }

        public static DirectionalLocation Parse(char direction)
        {
            return new DirectionalLocation(direction switch
            {
                'U' => Direction.Up,
                'D' => Direction.Down,
                'L' => Direction.Left,
                'R' => Direction.Right,
                _ => default,
            });
        }

        public override string ToString() => Direction.ToString();
    }
}
