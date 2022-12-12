using AdventOfCode2022.Utils;

namespace AdventOfCode2022;

public class Day09 : Exercise
{
    private static readonly IReadOnlyDictionary<char, IntVector2> Directions = new Dictionary<char, IntVector2>()
    {
        {'R', new IntVector2(1, 0)},
        {'U', new IntVector2(0, 1)},
        {'L', new IntVector2(-1, 0)},
        {'D', new IntVector2(0, -1)},
    };

    public long ExecutePart1(string[] lines)
    {
        return CountPositionVisitedByTail(lines, 1);
    }

    public long ExecutePart2(string[] lines)
    {
        return CountPositionVisitedByTail(lines, 9);
    }

    private IEnumerable<IntVector2> EnumerateMotions(string[] lines)
    {
        foreach (var line in lines)
        {
            var direction = Directions[line[0]];
            int stepCount = int.Parse(line.Substring(2));
            for (int i = 0; i < stepCount; i++)
            {
                yield return direction;
            }
        }
    }

    private long CountPositionVisitedByTail(string[] lines, int tailCount)
    {
        var knots = Enumerable.Repeat(new IntVector2(0, 0), tailCount + 1).ToList();

        HashSet<IntVector2> differentTailPositions = new();
        foreach (var motion in EnumerateMotions(lines))
        {
            knots[0] += motion;
            for (int i = 0; i < tailCount; i++)
            {
                var nextIndex = i + 1;
                knots[nextIndex] = UpdateTailPosition(knots[i], knots[nextIndex]);
            }

            differentTailPositions.Add(knots[tailCount]);
        }

        return differentTailPositions.Count;
    }

    public static IntVector2 UpdateTailPosition(IntVector2 head, IntVector2 tail)
    {
        var delta = head - tail;
        if ((Math.Abs(delta.X) <= 1) && (Math.Abs(delta.Y) <= 1))
        {
            return tail;
        }

        return head - delta / 2;
    }
}
