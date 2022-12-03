using AdventOfCode2022.Utils;

namespace AdventOfCode2022;

public class Day03 : Exercise
{
    public long ExecutePart1(string[] lines)
    {
        return lines.Select(SplitHalf)
            .SelectMany(x => x.left.Intersect(x.right))
            .Sum(GetItemPriority);
    }

    private int GetItemPriority(char item)
    {
        if (item <= 'Z')
        {
            return item - 'A' + 27;
        }

        return item - 'a' + 1;
    }

    public long ExecutePart2(string[] lines)
    {
        return lines.Batch(3).Select(GetCommonItem).Sum(GetItemPriority);
    }

    private char GetCommonItem(List<string> group)
    {
        var allChars = Enumerable.Range(0, 26)
            .SelectMany(i => new[] { (char)(i + 'a'), (char)(i + 'A') });
        char singleChar = group.Aggregate(allChars, (aggregate, line) => aggregate.Intersect(line))
            .Single();
        return singleChar;
    }

    private (string left, string right) SplitHalf(string line)
    {
        int halfLength = line.Length / 2;
        return (line.Substring(0, halfLength), line.Substring(halfLength));
    }
}
