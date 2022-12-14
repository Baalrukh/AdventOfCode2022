using AdventOfCode2022.Utils;

namespace AdventOfCode2022;

public class Day14 : Exercise
{
    public long ExecutePart1(string[] lines)
    {
        var filledPositions = ParseMap(lines);

        var maxDepth = filledPositions.Max(x => x.Y);

        IntVector2 spawnPosition = new IntVector2(500, 0);
        int particleCount = 0;
        var endPosition = TryDeposeSand(spawnPosition, filledPositions, maxDepth);
        while (endPosition != null)
        {
            particleCount++;
            endPosition = TryDeposeSand(spawnPosition, filledPositions, maxDepth);
        }

        return particleCount;
    }

    public long ExecutePart2(string[] lines)
    {
        var filledPositions = ParseMap(lines);
        var maxDepth = filledPositions.Max(x => x.Y) + 2;


        var floorStart = new IntVector2(500 - maxDepth - 5, maxDepth);
        for (int i = 0; i < 2 * maxDepth + 10; i++)
        {
            filledPositions.Add(floorStart);
            floorStart += new IntVector2(1, 0);
        }

        IntVector2 spawnPosition = new IntVector2(500, 0);
        int particleCount = 1;

        var endPosition = TryDeposeSand(spawnPosition, filledPositions, maxDepth);
        while (!endPosition.Equals(spawnPosition))
        {
            endPosition = TryDeposeSand(spawnPosition, filledPositions, maxDepth);
            particleCount++;
        }

        return particleCount;
    }

    private IntVector2? TryDeposeSand(IntVector2 spawnPosition, HashSet<IntVector2> filledPositions, int maxDepth)
    {
        var position = spawnPosition;
        while (UpdateFallStep(ref position, filledPositions))
        {
            if (position.Y > maxDepth)
            {
                return null;
            }
        }

        filledPositions.Add(position);
        return position;
    }

    private static HashSet<IntVector2> ParseMap(string[] lines)
    {
        var filledPositions = new HashSet<IntVector2>();
        foreach (var line in lines)
        {
            ParseLine(line, filledPositions);
        }

        return filledPositions;
    }

    public static void ParseLine(string line, HashSet<IntVector2> filledPositions)
    {
        var cornersTxt = line.Split(" -> ");

        using var enumerator = cornersTxt.Select(ParseInt).GetEnumerator();
        enumerator.MoveNext();
        IntVector2 firstPos = enumerator.Current;
        filledPositions.Add(firstPos);
        while (enumerator.MoveNext())
        {
            IntVector2 nextPos = enumerator.Current;
            var delta = nextPos - firstPos;
            var stepCount = delta.ManhattanDistance;
            delta /= stepCount;
            for (int i = 0; i < stepCount; i++)
            {
                firstPos += delta;
                filledPositions.Add(firstPos);
            }
        }
    }

    private static IntVector2 ParseInt(string text)
    {
        var separator = text.IndexOf(',');
        return new IntVector2(int.Parse(text[..separator]), int.Parse(text[(separator + 1)..]));
    }

    public static bool UpdateFallStep(ref IntVector2 particlePosition, HashSet<IntVector2> filledPositions)
    {
        particlePosition += new IntVector2(0, 1);
        if (filledPositions.Contains(particlePosition))
        {
            particlePosition += new IntVector2(-1, 0);
            if (filledPositions.Contains(particlePosition))
            {
                particlePosition += new IntVector2(2, 0);
                if (filledPositions.Contains(particlePosition))
                {
                    particlePosition += new IntVector2(-1, -1);
                    return false;
                }
            }
        }
        return true;
    }
}
