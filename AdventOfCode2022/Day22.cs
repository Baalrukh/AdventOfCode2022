using System.Diagnostics;
using AdventOfCode2022.Utils;

namespace AdventOfCode2022;

public class Day22 : Exercise
{
    public struct PositionTransformersParameters
    {
        public readonly IntVector2 DestinationFaceIndices;
        public readonly Direction EntranceDirection;
        public readonly bool InvertSideCoordinate;

        public PositionTransformersParameters(IntVector2 destinationFaceIndices, Direction entranceDirection, bool invertSideCoordinate)
        {
            DestinationFaceIndices = destinationFaceIndices;
            EntranceDirection = entranceDirection;
            InvertSideCoordinate = invertSideCoordinate;
        }
        
        
        public IntVector2 FacePositionTransformer(IntVector2 position, Direction currentDirection, IntVector2 srcFaceIndices, int faceSize)
        {
            IntVector2 localPosition = position - srcFaceIndices * faceSize;

            IntVector2 baseDstPosition = DestinationFaceIndices * faceSize;

            int lateralPos;
            if ((currentDirection == Direction.Right) || (currentDirection == Direction.Left))
            {
                lateralPos = localPosition.Y;
            }
            else
            {
                lateralPos = localPosition.X;
            }


            if (InvertSideCoordinate)
            {
                lateralPos = faceSize - lateralPos - 1;
            }
        
            if (EntranceDirection == Direction.Right)
            {
                localPosition = new IntVector2(0, lateralPos);
            }
            else if (EntranceDirection == Direction.Left)
            {
                localPosition = new IntVector2(faceSize - 1, lateralPos);
            }
            else if (EntranceDirection == Direction.Top)
            {
                localPosition = new IntVector2(lateralPos, faceSize - 1);
            }
            else
            {
                localPosition = new IntVector2(lateralPos, 0);
            }

            return localPosition + baseDstPosition;
        }
    }

    private Dictionary<(IntVector2, IntVector2), PositionTransformersParameters> _positionTransformersParameters = new()
    {
        { (new(1, 0), new(1, -1)), new(new(0, 3), Direction.Right, false) },
        { (new(1, 0), new(1,  1)), new(new(1, 1), Direction.Bottom, false) },
        { (new(1, 0), new(0,  0)), new(new(0, 2), Direction.Right, true) },
        { (new(1, 0), new(2,  0)), new(new(2, 0), Direction.Right, false) },

        { (new(2, 0), new(2, -1)), new(new(0, 3), Direction.Top, false) },
        { (new(2, 0), new(2,  1)), new(new(1, 1), Direction.Left, false) },
        { (new(2, 0), new(1,  0)), new(new(1, 0), Direction.Left, false) },
        { (new(2, 0), new(3,  0)), new(new(1, 2), Direction.Left, true) },

        { (new(1, 1), new(1, 0)), new(new(1, 0), Direction.Top, false) },
        { (new(1, 1), new(1, 2)), new(new(1, 2), Direction.Bottom, false) },
        { (new(1, 1), new(0, 1)), new(new(0, 2), Direction.Bottom, false) },
        { (new(1, 1), new(2, 1)), new(new(2, 0), Direction.Top, false) },

        { (new(0, 2), new(0,  1)), new(new(1, 1), Direction.Right, false) },
        { (new(0, 2), new(0,  3)), new(new(0, 3), Direction.Bottom, false) },
        { (new(0, 2), new(-1, 2)), new(new(1, 0), Direction.Right, true) },
        { (new(0, 2), new(1,  2)), new(new(1, 2), Direction.Right, false) },

        { (new(1, 2), new(1, 1)), new(new(1, 1), Direction.Top, false) },
        { (new(1, 2), new(1, 3)), new(new(0, 3), Direction.Left, false) },
        { (new(1, 2), new(0, 2)), new(new(0, 2), Direction.Left, false) },
        { (new(1, 2), new(2, 2)), new(new(2, 0), Direction.Left, true) },

        { (new(0, 3), new(0,  2)), new(new(0, 2), Direction.Top, false) },
        { (new(0, 3), new(0,  4)), new(new(2, 0), Direction.Bottom, false) },
        { (new(0, 3), new(-1, 3)), new(new(1, 0), Direction.Bottom, false) },
        { (new(0, 3), new(1,  3)), new(new(1, 2), Direction.Top, false) },
    };

    
    public long ExecutePart1(string[] lines)
    {
        int emptyLineIndex = lines.IndexOf("");
        World world = World.Parse(lines.Take(emptyLineIndex).ToArray(), 50);
        int startX = Enumerable.Range(0, world.Map.Width).First(x => world.Map[x, 0] == '.');
        (IntVector2 position, Direction direction) = world.ApplyMotionWithWraping(new IntVector2(startX, 0), Direction.Right, lines[emptyLineIndex + 1]);

        // 1000 * y + 4 * x + d 
        // 1000 * 6 + 4 * 8 + 0

        return 1000L * (position.Y + 1) + 4L * (position.X + 1) + direction.Index;
    }

    public long ExecutePart2(string[] lines)
    {
        return GetCubePassword(lines, 50, _positionTransformersParameters);
    }

    public static long GetCubePassword(string[] lines, int faceSize,
        IReadOnlyDictionary<(IntVector2, IntVector2), PositionTransformersParameters> positionTransformers)
    {
        IntVector2 position = GetFinalPositionCube(lines, out Direction direction, faceSize, positionTransformers);

        // 1000 * y + 4 * x + d 
        // 1000 * 6 + 4 * 8 + 0

        return 1000L * (position.Y + 1) + 4L * (position.X + 1) + direction.Index;
    }

    private static IntVector2 GetFinalPositionCube(string[] lines, out Direction direction, int faceSize,
        IReadOnlyDictionary<(IntVector2, IntVector2), PositionTransformersParameters> positionTransformers)
    {
        int emptyLineIndex = lines.IndexOf("");
        World world = World.Parse(lines.Take(emptyLineIndex).ToArray(), faceSize);
        int startX = Enumerable.Range(0, world.Map.Width).First(x => world.Map[x, 0] == '.');
        (var position, direction) =
            world.ApplyMotionOnCube(new IntVector2(startX, 0), Direction.Right, lines[emptyLineIndex + 1], positionTransformers);
        return position;
    }

    public class Direction
    {
        public readonly string Name;
        public readonly IntVector2 Offset;
        private readonly int _index;
        public readonly char Char;
        
        public Direction(string name, IntVector2 offset, int index, char c)
        {
            Name = name;
            Offset = offset;
            _index = index;
            Char = c;
        }

        public int Index => _index;

        public Direction TurnLeft()
        {
            return Directions[(_index + 3) % 4];
        }
        
        public Direction TurnRight()
        {
            return Directions[(_index + 1) % 4];
        }

        public override string ToString()
        {
            return Name;
        }

        public static Direction Right = new Direction("Right", new IntVector2(1, 0), 0, '>');
        public static Direction Bottom =  new Direction("Bottom", new IntVector2(0, 1), 1, 'v');
        public static Direction Left = new Direction("Left", new IntVector2(-1, 0), 2, '<');
        public static Direction Top = new Direction("Top", new IntVector2(0, -1), 3, '^');

        public static readonly Direction[] Directions = new[]
        {
            Right, Bottom, Left, Top
        };
    }

    public class World
    {
        public readonly Map2D<char> Map;
        public readonly Interval[] HorizontalRange;
        public readonly Interval[] VerticalRange;
        private readonly int _faceSize;

        private delegate (IntVector2, Direction) AdvanceMethod(IntVector2 position, Direction direction, int distance);

        private readonly Dictionary<IntVector2, char> _pathChars = new();

        private World(Map2D<char> map, Interval[] horizontalRange, Interval[] verticalRange, int faceSize)
        {
            Map = map;
            HorizontalRange = horizontalRange;
            VerticalRange = verticalRange;
            _faceSize = faceSize;
        }

        public (IntVector2, Direction) ApplyMotionWithWraping(IntVector2 startPosition, Direction direction, string sequence)
        {
            return ApplyMotion(startPosition, direction, sequence, AdvanceIfNecessaryWithWrapping, false);
        }

        public (IntVector2, Direction) ApplyMotionOnCube(IntVector2 startPosition, Direction direction, string sequence, 
            IReadOnlyDictionary<(IntVector2, IntVector2), PositionTransformersParameters> positionTransformers)
        {
            return ApplyMotion(startPosition, direction, sequence, (position, dir, distance) => AdvanceIfNecessaryOnCube(position, dir, distance, positionTransformers), false);
        }

        private (IntVector2, Direction) ApplyMotion(IntVector2 startPosition, Direction direction, string sequence, AdvanceMethod advanceMethod, bool debug)
        {
            IntVector2 position = startPosition;
            int distance = 0;
            foreach (char c in sequence)
            {
                if (c == 'R')
                {
                    (position, direction) = advanceMethod(position, direction, distance);
                    distance = 0;
                    direction = direction.TurnRight();
                    if (debug)
                        PrintMap();
                }
                else if (c == 'L')
                {
                    (position, direction) = advanceMethod(position, direction, distance);
                    distance = 0;
                    direction = direction.TurnLeft();
                    if (debug)
                        PrintMap();
                }
                else
                {
                    distance = distance * 10 + c - '0';
                }
            }

            if (distance > 0)
            {
                (position, direction) = advanceMethod(position, direction, distance);
                if (debug)
                    PrintMap();
            }

            return (position, direction);
        }

        private void PrintMap()
        {
            Debug.WriteLine("");
            
            char[][] map = Enumerable.Range(0, Map.Height).Select(x => new char[Map.Width]).ToArray();

            for (int y = 0; y < Map.Height; y++)
            {
                char[] line = map[y];
                for (int x = 0; x < Map.Width; x++)
                {
                    line[x] = Map[x, y];
                }
            }

            foreach (var pair in _pathChars)
            {
                map[pair.Key.Y][pair.Key.X] = pair.Value;
            }

            foreach (char[] chars in map)
            {
                Debug.WriteLine(new string(chars, 0, chars.Length));
            }
        }

        public (IntVector2, Direction) AdvanceIfNecessaryWithWrapping(IntVector2 position, Direction direction, int distance)
        {
            if (distance == 0)
            {
                return (position, direction);
            }

            for (int i = 0; i < distance; i++)
            {
                IntVector2 nextPosition = position + direction.Offset;
                if (direction.Offset.X != 0)
                {
                    if (nextPosition.X < HorizontalRange[nextPosition.Y].Start)
                    {
                        nextPosition = new IntVector2(HorizontalRange[nextPosition.Y].End, nextPosition.Y);
                    }
                    else if (nextPosition.X > HorizontalRange[nextPosition.Y].End)
                    {
                        nextPosition = new IntVector2(HorizontalRange[nextPosition.Y].Start, nextPosition.Y);
                    }
                }
                else
                {
                    if (nextPosition.Y < VerticalRange[nextPosition.X].Start)
                    {
                        nextPosition = new IntVector2(nextPosition.X, VerticalRange[nextPosition.X].End);
                    }
                    else if (nextPosition.Y > VerticalRange[nextPosition.X].End)
                    {
                        nextPosition = new IntVector2(nextPosition.X, VerticalRange[nextPosition.X].Start);
                    }
                }
                
                if (Map[nextPosition] == '#')
                {
                    return (position, direction);
                }

                position = nextPosition;
            }
            
            return (position, direction);
        }

        public delegate (IntVector2 position, Direction direction) PositionTransformer(IntVector2 position,
            Direction direction, int faceSize);

        public (IntVector2, Direction) AdvanceIfNecessaryOnCube(IntVector2 position, Direction direction, int distance,
            IReadOnlyDictionary<(IntVector2, IntVector2), PositionTransformersParameters> positionTransformers)
        {
            if (distance == 0)
            {
                return (position, direction);
            }

            for (int i = 0; i < distance; i++)
            {
                IntVector2 nextPosition = position + direction.Offset;

                IntVector2 faceIndices = position / _faceSize;
                IntVector2 nextFaceIndices = (nextPosition + new IntVector2(_faceSize, _faceSize)) / _faceSize - new IntVector2(1, 1);

                Direction nextDirection;
                if (nextFaceIndices != faceIndices)
                {
                    if (!positionTransformers.TryGetValue((faceIndices, nextFaceIndices), out var parameters))
                    {
                        throw new Exception($"Need transition from {faceIndices} to {nextFaceIndices}");
                    }

                    nextPosition = parameters.FacePositionTransformer(position, direction, faceIndices, _faceSize);
                    nextDirection = parameters.EntranceDirection;
                }
                else
                {
                    nextDirection = direction;
                }
                
                if (Map[nextPosition] == '#')
                {
                    return (position, direction);
                }

                _pathChars[position] = direction.Char;
                _pathChars[nextPosition] = 'x';
                
                position = nextPosition;
                direction = nextDirection;
            }
            
            return (position, direction);
        }

        public static World Parse(string[] lines, int faceSize)
        {
            int width = lines.Max(x => x.Length);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length < width)
                {
                    lines[i] = Enumerable.Repeat(" ", width - lines[i].Length).Aggregate(lines[i], (a, b) => a + b);
                }
            }
            
            Map2D<char> map = Map2D<char>.Parse(lines, x => x, () => ' ');
            Interval[] horizontalRange = Enumerable.Range(0, map.Height).Select(y => FindHorizontalRange(map, y)).ToArray();
            Interval[] verticalRange = Enumerable.Range(0, map.Width).Select(x => FindVerticalRange(map, x)).ToArray();
            return new World(map, horizontalRange, verticalRange, faceSize);
        }

        private static Interval FindHorizontalRange(Map2D<char> map, int y)
        {
            int x = 0;

            while (map[x, y] == ' ')
            {
                x++;
            }

            int start = x;

            while ((x < map.Width) && (map[x, y] != ' '))
            {
                x++;
            }

            return Interval.FromToIncluded(start, x - 1);
        }

        private static Interval FindVerticalRange(Map2D<char> map, int x)
        {
            int y = 0;

            while (map[x, y] == ' ')
            {
                y++;
            }

            int start = y;

            while ((y < map.Height) && (map[x, y] != ' '))
            {
                y++;
            }

            return Interval.FromToIncluded(start, y - 1);
        }
    }
}
