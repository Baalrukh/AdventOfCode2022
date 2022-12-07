using AdventOfCode2022.Utils;

namespace AdventOfCode2022;

public class Day01 : Exercise
{
    public long ExecutePart1(string[] lines)
    {
        return GetBatchSums(lines).Max();
    }

    private List<int> GetBatchSums(string[] lines)
    {
        var batches = lines.Batch(x => x.Length == 0, true);
        return batches.Select(b => b.Sum(int.Parse)).ToList();
    }

    public long ExecutePart2(string[] lines)
    {
        var batchSums = GetBatchSums(lines);
        return batchSums.OrderByDescending(x => x).Take(3).Sum();
    }

}