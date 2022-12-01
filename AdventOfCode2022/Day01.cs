namespace AdventOfCode2022;

public class Day01 : Exercise
{
    public long ExecutePart1(string[] lines)
    {
        return GetBatchSums(lines).Max();
    }

    private List<int> GetBatchSums(string[] lines)
    {
        var batches = BatchLinesSeparatedByEmptyLines(lines);
        return batches.Select(b => b.Sum(int.Parse)).ToList();
    }

    public long ExecutePart2(string[] lines)
    {
        var batchSums = GetBatchSums(lines);
        return batchSums.OrderByDescending(x => x).Take(3).Sum();
    }

    private IEnumerable<List<string>> BatchLinesSeparatedByEmptyLines(IEnumerable<string> lines)
    {
        List<string> batch = new List<string>();

        foreach (var line in lines)
        {
            if (line.Length == 0)
            {
                if (batch.Count > 0)
                {
                    yield return batch;
                }

                batch = new List<string>();
            }
            else
            {
                batch.Add(line);
            }
        }

        if (batch.Count > 0)
        {
            yield return batch;
        }
    }
}