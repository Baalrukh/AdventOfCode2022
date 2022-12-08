using AdventOfCode2022.Utils;

namespace AdventOfCode2022;

public class Day08 : Exercise
{
    private static readonly IntVector2[] _Directions =
    {
        new(-1, 0),
        new(0, -1),
        new(1, 0),
        new(0, 1)
    };

    public long ExecutePart1(string[] lines)
    {
        var treeHeights = ParseTreeHeights(lines);

        HashSet<IntVector2> visibility = new();

        // visibility per line
        for (int y = 0; y < treeHeights.Height; y++)
        {
            int maxHeight = -1;
            int maxHeightReverse = -1;
            for (int x = 0; x < treeHeights.Width; x++)
            {
                int height = treeHeights[x, y];
                if (height > maxHeight)
                {
                    visibility.Add(new (x, y));
                    maxHeight = height;
                }

                var xReverse = treeHeights.Width - x;
                int heightReverse = treeHeights[xReverse, y];
                if (heightReverse > maxHeightReverse)
                {
                    visibility.Add(new (xReverse, y));
                    maxHeightReverse = heightReverse;
                }
            }
        }

        // visibility per column
        for (int x = 0; x < treeHeights.Width; x++)
        {
            int maxHeight = -1;
            int maxHeightReverse = -1;
            for (int y = 0; y < treeHeights.Height; y++)
            {
                int height = treeHeights[x, y];
                if (height > maxHeight)
                {
                    visibility.Add(new (x, y));
                    maxHeight = height;
                }

                var yReverse = treeHeights.Height - y - 1;
                int heightReverse = treeHeights[x, yReverse];
                if (heightReverse > maxHeightReverse)
                {
                    visibility.Add(new (x, yReverse));
                    maxHeightReverse = heightReverse;
                }
            }
        }

        return visibility.Count;
    }

    public static Map2D<int> ParseTreeHeights(string[] lines)
    {
        return Map2D<int>.Parse(lines, c => c - '0', () => -1);
    }

    public long ExecutePart2(string[] lines)
    {
        var treeHeights = ParseTreeHeights(lines);
        return Enumerable.Range(1, treeHeights.Width - 2)
            .SelectMany(x => Enumerable.Range(1, treeHeights.Height - 2).Select(y => new IntVector2(x, y)))
            .Max(pos => GetTreeVisibilityScore(treeHeights, pos));
    }

    public static int GetTreeVisibilityScore(Map2D<int> map, IntVector2 pos)
    {
        return _Directions.Aggregate(1, (score, direction) => score *= GetVisibleTreeCountInDirection(map, pos, direction));
    }

    public static int GetVisibleTreeCountInDirection(Map2D<int> map, IntVector2 pos, IntVector2 direction)
    {
        int posHeight = map[pos];
        int count = 1;
        pos += direction;
        while (map.IsInside(pos) && map[pos] < posHeight)
        {
            pos += direction;
            if (!map.IsInside(pos))
            {
                break;
            }
            count++;
        }

        return count;
    }
}
