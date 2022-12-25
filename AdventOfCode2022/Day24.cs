using System.Diagnostics;
using AdventOfCode2022.Utils;

namespace AdventOfCode2022;

public class Day24 : Exercise
{

    public class Direction
    {
        public readonly IntVector2 Offset;
        public readonly char Character;

        private Direction(IntVector2 offset, char character)
        {
            Offset = offset;
            Character = character;
        }

        public override string ToString()
        {
            return Character.ToString();
        }

        public static Direction Right = new Direction(new IntVector2(1, 0), '>');
        public static Direction Down =  new Direction(new IntVector2(0, 1), 'v');
        public static Direction Left = new Direction(new IntVector2(-1, 0), '<');
        public static Direction Up = new Direction(new IntVector2(0, -1), '^');

        public static Direction[] AllDirections = new[] { Right, Down, Left, Up};

        public static Dictionary<char, Direction> DirectionsFromChar =
            AllDirections.ToDictionary(x => x.Character, x => x);
    }

    public class Blizzard
    {
        public IntVector2 Position { get; private set; }
        public readonly Direction Direction;

        public Blizzard(IntVector2 position, Direction direction)
        {
            Direction = direction;
            Position = position;
        }

        protected bool Equals(Blizzard other)
        {
            return Direction.Equals(other.Direction) && Position.Equals(other.Position);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Blizzard)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Direction, Position);
        }

        public override string ToString()
        {
            return Position + " " + Direction;
        }

        public void Update(int width, int height)
        {
            Position += Direction.Offset;
            if (Position.X == -1)
            {
                Position = new IntVector2(width - 1, Position.Y);
            }
            else if (Position.X == width)
            {
                Position = new IntVector2(0, Position.Y);
            }

            if (Position.Y == -1)
            {
                Position = new IntVector2(Position.X, height - 1);
            } 
            else if (Position.Y == height)
            {
                Position = new IntVector2(Position.X, 0);
            }
        }
    }


    public class BlizzardsState
    {
        private readonly int _width;
        private readonly int _height;
        public readonly IReadOnlyList<Blizzard> Blizzards;

        private List<ValleyWalkability> _walkabilities = new List<ValleyWalkability>(); 
            
        public BlizzardsState(IReadOnlyList<Blizzard> blizzards, int width, int height)
        {
            Blizzards = blizzards;
            _width = width;
            _height = height;
        }

        public ValleyWalkability GetValleyWalkability(int time)
        {
            while (time >= _walkabilities.Count)
            {
                UpdateBlizzards();
                _walkabilities.Add(GetWalkabilityMap());
            }

            return _walkabilities[time];
        }

        internal ValleyWalkability GetWalkabilityMap()
        {
            bool[] map = new bool[_width * _height];
            Array.Fill(map, true)
                ;
            foreach (Blizzard blizzard in Blizzards)
            {
                map[blizzard.Position.Y * _width + blizzard.Position.X] = false;
            }

            return new ValleyWalkability(_width, map);
        }

        public void UpdateBlizzards()
        {
            foreach (Blizzard blizzard in Blizzards)
            {
                blizzard.Update(_width, _height);
            }
        }
    }
    
    public class Valley
    {
        public readonly int Width;
        public readonly int Height;
        public readonly IntVector2 Entrance;
        public readonly IntVector2 Exit;
        public readonly BlizzardsState BlizzardState;

        public Valley(int width, int height, IntVector2 entrance, IntVector2 exit, BlizzardsState blizzardState)
        {
            Width = width;
            Height = height;
            Entrance = entrance;
            Exit = exit;
            BlizzardState = blizzardState;
        }

        protected bool Equals(Valley other)
        {
            return Width == other.Width && Height == other.Height && Entrance.Equals(other.Entrance) && Exit.Equals(other.Exit);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Valley)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Width, Height, Entrance, Exit);
        }

        public override string ToString()
        {
            return $"W{Width}, H{Height}, Entrance={Entrance}, Exit={Exit}";
        }
        
        private static IntVector2[] Offsets = new[]
        {
            new IntVector2(1, 0),
            new IntVector2(0, 1),
            new IntVector2(0, 0),
            new IntVector2(-1, 0),
            new IntVector2(0, -1)
        };

        public int GetMinPathLength(IntVector2 position, int time, HashSet<(IntVector2, int)> absoluteVisited)
        {
            if ((Exit - position).ManhattanDistance + time >= 350)
            {
                return int.MaxValue;
            }

            absoluteVisited.Add((position, time));
            
            int bestPathLength = int.MaxValue;
            foreach (IntVector2 offset in Offsets)
            {
                var nextPosition = position + offset;
                int nextTime = time + 1;
                if (nextPosition == Exit)
                {
                    return nextTime;
                }

                if (absoluteVisited.Contains((nextPosition, nextTime)))
                {
                    continue;
                }
                
                ValleyWalkability valleyWalkability = BlizzardState.GetValleyWalkability(time);
                if ((IsInsideValley(nextPosition) && valleyWalkability[nextPosition])
                 || (nextPosition == Entrance))
                {
                    int foundPathLength = GetMinPathLength(nextPosition, nextTime, absoluteVisited);
                    if (foundPathLength < bestPathLength)
                    {
                        bestPathLength = foundPathLength;
                    }
                }
            }

            return bestPathLength;
        }
        
        public int GetMinPathLength()
        {
            return GetMinPathLength(Entrance, 0, new HashSet<(IntVector2, int)>());
        }

        private bool IsInsideValley(IntVector2 position)
        {
            return ((position.X >= 0) && (position.X < Width)
                && (position.Y >= 0) && (position.Y < Height));
        }
    }
    
    public long ExecutePart1(string[] lines)
    {
        Valley valley = ParseValley(lines);
        return valley.GetMinPathLength();
    }

    public long ExecutePart2(string[] lines)
    {
        return -2;
    }

    public static Valley ParseValley(string[] lines)
    {
        int width = lines[0].Length - 2;
        int height = lines.Length - 2;
        IntVector2 entrance = new(lines[0].IndexOf('.') - 1, -1);
        IntVector2 exit = new(lines[^1].IndexOf('.') - 1, lines.Length - 2);
        List<Blizzard> blizzards = new List<Blizzard>();
        for (int y = 1; y < lines.Length; y++)
        {
            for (int x = 1; x < width + 1; x++)
            {
                char c = lines[y][x];
                if (Direction.DirectionsFromChar.TryGetValue(c, out var direction))
                {
                    blizzards.Add(new Blizzard(new IntVector2(x - 1, y - 1), direction));
                }
            }
        }

        return new Valley(width, height, entrance, exit, new BlizzardsState(blizzards, width, height));
    }

    public class ValleyWalkability
    {
        private readonly int _width;
        internal readonly bool[] _walkability;

        public ValleyWalkability(int width, bool[] walkability)
        {
            _width = width;
            _walkability = walkability;
        }

        public bool this[IntVector2 position] => _walkability[position.Y * _width + position.X];
    }
}
