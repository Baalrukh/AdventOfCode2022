using AdventOfCode2022.Utils;

namespace AdventOfCode2022;

public class Day22 : Exercise
{
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
        int emptyLineIndex = lines.IndexOf("");
        World world = World.Parse(lines.Take(emptyLineIndex).ToArray(), 50);
        int startX = Enumerable.Range(0, world.Map.Width).First(x => world.Map[x, 0] == '.');
        (IntVector2 position, Direction direction) = world.ApplyMotionOnCube(new IntVector2(startX, 0), Direction.Right, lines[emptyLineIndex + 1]);

        // 1000 * y + 4 * x + d 
        // 1000 * 6 + 4 * 8 + 0

        return 1000L * (position.Y + 1) + 4L * (position.X + 1) + direction.Index;
    }

    public class Direction
    {
        public readonly string Name;
        public readonly IntVector2 Offset;
        private readonly int _index;

        public Direction(string name, IntVector2 offset, int index)
        {
            Name = name;
            Offset = offset;
            _index = index;
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

        public static Direction Right = new Direction("Right", new IntVector2(1, 0), 0);
        public static Direction Bottom =  new Direction("Bottom", new IntVector2(0, 1), 1);
        public static Direction Left = new Direction("Left", new IntVector2(-1, 0), 2);
        public static Direction Top = new Direction("Top", new IntVector2(0, -1), 3);

        public static readonly Direction[] Directions = new[]
        {
            Right, Bottom, Left, Top
        };
    }

    public class World
    {
        public Map2D<char> Map;
        public Interval[] HorizontalRange;
        public Interval[] VerticalRange;
        private int _faceSize;

        private delegate IntVector2 AdvanceMethod(IntVector2 position, Direction direction, int distance);

        private World(Map2D<char> map, Interval[] horizontalRange, Interval[] verticalRange, int faceSize)
        {
            Map = map;
            HorizontalRange = horizontalRange;
            VerticalRange = verticalRange;
            _faceSize = faceSize;
        }

        public (IntVector2, Direction) ApplyMotionWithWraping(IntVector2 startPosition, Direction direction, string sequence)
        {
            return ApplyMotion(startPosition, direction, sequence, AdvanceIfNecessaryWithWrapping);
        }

        public (IntVector2, Direction) ApplyMotionOnCube(IntVector2 startPosition, Direction direction, string sequence)
        {
            return ApplyMotion(startPosition, direction, sequence, AdvanceIfNecessaryOnCube);
        }

        private (IntVector2, Direction) ApplyMotion(IntVector2 startPosition, Direction direction, string sequence, AdvanceMethod advanceMethod)
        {
            IntVector2 position = startPosition;
            int distance = 0;
            foreach (char c in sequence)
            {
                if (c == 'R')
                {
                    position = advanceMethod(position, direction, distance);
                    distance = 0;
                    direction = direction.TurnRight();
                } else if (c == 'L')
                {
                    position = advanceMethod(position, direction, distance);
                    distance = 0;
                    direction = direction.TurnLeft();
                } else
                {
                    distance = distance * 10 + c - '0';
                }
            }

            if (distance > 0)
            {
                position = advanceMethod(position, direction, distance);
            }

            return (position, direction);
        }

        public IntVector2 AdvanceIfNecessaryWithWrapping(IntVector2 position, Direction direction, int distance)
        {
            if (distance == 0)
            {
                return position;
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
                    return position;
                }

                position = nextPosition;
            }
            
            return position;
        }
        
        public IntVector2 AdvanceIfNecessaryOnCube(IntVector2 position, Direction direction, int distance)
        {
            if (distance == 0)
            {
                return position;
            }

            for (int i = 0; i < distance; i++)
            {
                IntVector2 nextPosition = position + direction.Offset;
                if (direction.Offset.X != 0)
                {
                    if (nextPosition.X < HorizontalRange[nextPosition.Y].Start)
                    {
                        (nextPosition, direction) = HandleTransition(position, nextPosition);
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
                    return position;
                }

                position = nextPosition;
            }
            
            return position;
        }

        private (IntVector2 nextPosition, Direction direction) HandleTransition(IntVector2 position, IntVector2 nextPosition)
        {
            //   012
            // 0  XX
            // 1  x
            // 2 xx
            // 3 x

            IntVector2 facePos = position / _faceSize;
            IntVector2 nextFacePos = nextPosition / _faceSize;

            if (facePos == new IntVector2(1, 0))
            {
                if (nextFacePos == new IntVector2(0, 0))
                { //0, 2, left
                    return (new IntVector2(0, _faceSize * 3 - 1 - nextPosition.Y), Direction.Right);
                }

                // 1, -1 => 0, 3 left
                return (new IntVector2(0, _faceSize * 3 + nextPosition.X - _faceSize), Direction.Right);
            }

            if (facePos == new IntVector2(2, 0))
            {
                if (nextPosition == new IntVector2(2, 1))
                {
                    // 1, 1, Right
                    return (new IntVector2(2 * _faceSize - 1, nextPosition.X - 2 * _faceSize + _faceSize),
                            Direction.Left);
                }

                if (nextPosition == new IntVector2(2, -1))
                {
                    // 0, 3, bottom
                    // return (new IntVector2())
                }
                
                // 3, 0
                return (new IntVector2(2 * _faceSize - 1, 3 * _faceSize - 1 - nextPosition.Y), Direction.Left);

            }

            return (position, Direction.Bottom);

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
