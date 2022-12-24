using System.Diagnostics;
using System.Text;
using AdventOfCode2022.Utils;

namespace AdventOfCode2022;

public class Day23 : Exercise
{
    private static readonly IntVector2[] AllSurroundingCells =
    {
        new IntVector2(-1, -1),
        new IntVector2(0, -1),
        new IntVector2(1, -1),
        new IntVector2(1, 0),
        new IntVector2(1, 1),
        new IntVector2(0, 1),
        new IntVector2(-1, 1),
        new IntVector2(-1, 0),
    };
    
    public long ExecutePart1(string[] lines)
    {
        HashSet<IntVector2> positions = ParsePositions(lines);

        List<MotionRule> rules = CreateRules();
        World world = new World(positions, rules);
        
        for (int i = 0; i < 10; i++)
        {
            // Debug.WriteLine(world.Plot());
            world.Update();
        }

        int minX = world.Positions.Min(p => p.X);
        int minY = world.Positions.Min(p => p.Y);
        int maxX = world.Positions.Max(p => p.X);
        int maxY = world.Positions.Max(p => p.Y);

        int area = (maxX - minX + 1) * (maxY - minY + 1);

        return area - world.Positions.Count;
    }

    public static List<MotionRule> CreateRules()
    {
        return new()
        {
            new OrthogonalRule(new[] { new IntVector2(-1, -1), new IntVector2(0, -1), new IntVector2(1, -1) }, new IntVector2(0, -1)),
            new OrthogonalRule(new[] { new IntVector2(-1, 1), new IntVector2(0, 1), new IntVector2(1, 1) }, new IntVector2(0, 1)),
            new OrthogonalRule(new[] { new IntVector2(-1, -1), new IntVector2(-1, 0), new IntVector2(-1, 1) }, new IntVector2(-1, 0)),
            new OrthogonalRule(new[] { new IntVector2(1, -1), new IntVector2(1, 0), new IntVector2(1, 1) }, new IntVector2(1, 0)),
        };
    }

    public static HashSet<IntVector2> ParsePositions(string[] lines)
    {
        HashSet<IntVector2> positions = new();
        for (int y = 0; y < lines.Length; y++)
        {
            string line = lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == '#')
                {
                    positions.Add(new IntVector2(x, y));
                }
            }
        }

        return positions;
    }

    public long ExecutePart2(string[] lines)
    {
        HashSet<IntVector2> positions = ParsePositions(lines);

        List<MotionRule> rules = CreateRules();
        World world = new World(positions, rules);
        int i = 1;
        
        while (world.Update())
        {
            i++;
        }

        return i;
    }

    public class World
    {
        private HashSet<IntVector2> _positions;
        private List<MotionRule> _rules;

        public HashSet<IntVector2> Positions => _positions;

        public World(HashSet<IntVector2> positions, List<MotionRule> rules)
        {
            _positions = positions;
            _rules = rules;
        }

        public IntVector2 GetDestination(IntVector2 position)
        {
            if (AllSurroundingCells.All(o => !_positions.Contains(position + o)))
            {
                return position;
            }

            foreach (MotionRule motionRule in _rules)
            {
                if (motionRule.TryApply(position, out var destination, _positions))
                {
                    return destination;
                }
            }

            return position;
        }

        public bool Update()
        {
            Dictionary<IntVector2,IntVector2> motions = _positions.ToDictionary(x => x, GetDestination);
            var groups = motions.GroupBy(x => x.Value);
            bool hasMoved = false;
            foreach (var group in groups)
            {
                List<KeyValuePair<IntVector2,IntVector2>> pairs = group.ToList();
                if (pairs.Count == 1)
                {
                    if (pairs[0].Key != pairs[0].Value)
                    {
                        _positions.Remove(pairs[0].Key);
                        _positions.Add(pairs[0].Value);
                        hasMoved = true;
                    }
                }
            }

            MotionRule motionRule = _rules[0];
            _rules.RemoveAt(0);
            _rules.Add(motionRule);
            
            return hasMoved;
        }

        public string Plot()
        {
            int minX = _positions.Min(p => p.X);
            int minY = _positions.Min(p => p.Y);
            int maxX = _positions.Max(p => p.X);
            int maxY = _positions.Max(p => p.Y);

            StringBuilder builder = new();
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    builder.Append(_positions.Contains(new IntVector2(x, y)) ? '#' : ".");
                }

                builder.Append('\n');
            }
            return builder.ToString();
        }
    }

    public interface MotionRule
    {
        bool TryApply(IntVector2 position, out IntVector2 destination, HashSet<IntVector2> positions);
    }

    public class OrthogonalRule : MotionRule
    {
        private IntVector2[] _offsetsToCheck;
        private IntVector2 _destinationOffset;

        public OrthogonalRule(IntVector2[] offsetsToCheck, IntVector2 destinationOffset)
        {
            _offsetsToCheck = offsetsToCheck;
            _destinationOffset = destinationOffset;
        }

        public bool TryApply(IntVector2 position, out IntVector2 destination, HashSet<IntVector2> positions)
        {
            if (_offsetsToCheck.Any(o => positions.Contains(position + o)))
            {
                destination = default;
                return false;
            }
            
            destination = position + _destinationOffset;
            return true;
        }
    }
}

