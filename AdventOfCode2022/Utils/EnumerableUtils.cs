namespace AdventOfCode2022.Utils;

public static class EnumerableUtils
{

    public static IEnumerable<List<T>> Batch<T>(this IEnumerable<T> sequence, int batchSize)
    {
        using (var enumerator = sequence.GetEnumerator())
        {
            while (true)
            {
                var list = new List<T>();
                for (int i = 0; i < batchSize; i++)
                {
                    if (enumerator.MoveNext())
                    {
                        list.Add(enumerator.Current);
                    }
                    else
                    {
                        if (list.Count > 0)
                        {
                            yield return list;
                        }

                        yield break;
                    }
                }

                yield return list;
            }
        }
    }

    public static IEnumerable<T> TakeByColumn<T>(this IEnumerable<T> sequence, int count, int stride)
    {
        using (var enumerator = sequence.GetEnumerator())
        {
            for (int i = 0; i < count; i++)
            {
                if (!enumerator.MoveNext())
                {
                    yield break;
                }

                yield return enumerator.Current;
                for (int j = 0; j < stride - 1; j++)
                {
                    enumerator.MoveNext();
                }
            }
        }
    }


    public static int GetCollectionHashCode<T>(this ICollection<T> collection)
    {
        return collection.Aggregate(0, (current, element) => (current * 397) ^ element.GetHashCode());
    }


    public static int IndexOf<T>(this IReadOnlyList<T> collection, T value)
    {
        int i = 0;
        foreach (var element in collection)
        {
            if (Equals(element, value))
            {
                return i;
            }
            i++;
        }

        return -1;
    }

    public static IEnumerable<List<string>> Batch(this IEnumerable<string> lines, Predicate<string> splitCriteria, bool skipSplitLine = false)
    {
        List<string> batch = new List<string>();

        foreach (var line in lines)
        {
            if (splitCriteria(line))
            {
                if (batch.Count > 0)
                {
                    yield return batch;
                }

                batch = new List<string>();
                if (!skipSplitLine)
                {
                    batch.Add(line);
                }
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