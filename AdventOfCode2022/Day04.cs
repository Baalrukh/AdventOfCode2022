using AdventOfCode2022.Utils;

namespace AdventOfCode2022;

public class Day04 : Exercise
{
    public long ExecutePart1(string[] lines)
    {
        return lines.Select(ParseIntervalPair)
            .Count(i => IsOneIncludedInOther(i[0], i[1]));
    }

    private Interval[] ParseIntervalPair(string x)
    {
        return x.Split(',').Select(ParseInterval).ToArray();
    }

    private bool IsOneIncludedInOther(Interval a, Interval b)
    {
        return a.IsInside(b) || b.IsInside(a);
    }
    
    private Interval ParseInterval(string intervalStr)
    {
        string[] tokens = intervalStr.Split('-');
        return new Interval(int.Parse(tokens[0]), int.Parse(tokens[1]));
    }

    public long ExecutePart2(string[] lines)
    {
        return lines.Select(ParseIntervalPair)
            .Count(i => i[0].Intersects(i[1]));
    }
}
