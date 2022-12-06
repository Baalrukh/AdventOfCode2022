namespace AdventOfCode2022;

public class Day06Perf
{

    public static long FindNonRepeatingBlockDoubleFor(string line, int patternLength)
    {
        for (int i = 0; i < line.Length - patternLength; i++)
        {
            if (FindNonRepeatingBlockDoubleForAtPos(line, patternLength, i))
            {
                return i + patternLength;
            }
        }

        return -1;
    }

    private static bool FindNonRepeatingBlockDoubleForAtPos(string line, int patternLength, int pos)
    {
        for (int i = 0; i < patternLength; i++)
        {
            for (int j = i + 1; j < patternLength; j++)
            {
                var index1 = pos + i;
                var index2 = pos + j;
                if (line[index1] == line[index2])
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static long FindNonRepeatingBlockLinq_Range(string line, int patternLength)
    {
        for (int i = 0; i < line.Length - patternLength; i++)
        {
            if (CheckValidity(line[i..(i + patternLength)])) return i + patternLength;
        }

        return -1;
    }

    public static long FindNonRepeatingBlockLinq_SubString(string line, int patternLength)
    {
        for (int i = 0; i < line.Length - patternLength; i++)
        {
            if (CheckValidity(line.Substring(i, patternLength))) return i + patternLength;
        }

        return -1;
    }

    private static bool CheckValidity(string line)
    {
        return line.Distinct().Count() == line.Length;
    }


    public static long FindNonRepeatingBlockLinq_Skip(string line, int patternLength)
    {
        for (int i = 0; i < line.Length - patternLength; i++)
        {
            if (line.Skip(i).Take(patternLength).Distinct().Count() == patternLength)
                return i + patternLength;
        }

        return -1;
    }


    public static long FindNonRepeatingBlock_MySolution(string line, int patternLength)
    {
        RepetitionFinder repetitionFinder = new();
        int i = 0;
        while (i < line.Length - patternLength)
        {
            if (repetitionFinder.HasRepetition(line, i, patternLength, out int advance))
            {
                return i + patternLength;
            }

            i += advance;
        }

        return -1;
    }

    public static long FindNonRepeatingBlockOpti_MySolution_NoSkip(string line, int patternLength)
    {
        RepetitionFinder repetitionFinder = new();
        int i = 0;
        while (i < line.Length - patternLength)
        {
            if (repetitionFinder.HasRepetition(line, i, patternLength, out int advance))
            {
                return i + patternLength;
            }

            i++;
        }

        return -1;
    }

    private class RepetitionFinder
    {
        private const int NotFound = -1;
        private int[] _foundCharacterCache = Enumerable.Repeat(NotFound, 26).ToArray();

        public bool HasRepetition(string line, int startIndex, int patternLength, out int advance)
        {
            for (int i = 0; i < patternLength; i++)
            {
                int c = line[startIndex + i] - 'a';
                if (_foundCharacterCache[c] != NotFound)
                {
                    advance = _foundCharacterCache[c] + 1;
                    CleanupCache(line, startIndex, i);
                    return false;
                }

                _foundCharacterCache[c] = i;
            }

            advance = 0;
            return true;
        }

        private void CleanupCache(string line, int startIndex, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int c = line[startIndex + i] - 'a';
                _foundCharacterCache[c] = NotFound;
            }
        }
    }

    public static long FindNonRepeatingBlockHashSet(string line, int patternLength)
    {
        for (int i = 0; i < line.Length - patternLength; i++)
        {
            if (IsNonRepeatingBlockHashSet_AtPos(line, patternLength, i))
            {
                return i + patternLength;
            }
        }

        return -1;
    }

    private static bool IsNonRepeatingBlockHashSet_AtPos(string line, int patternLength, int pos)
    {
        HashSet<char> set = new HashSet<char>();
        for (int j = 0; j < patternLength; j++)
        {
            var c = line[pos + j];
            if (set.Contains(c))
            {
                return false;
            }

            set.Add(c);
        }

        return true;
    }

    public static long FindNonRepeatingBlockHashSet_ReUse(string line, int patternLength)
    {
        HashSet<char> set = new ();
        for (int i = 0; i < line.Length - patternLength; i++)
        {
            if (IsNonRepeatingBlockHashSet_ReUse_AtPos(line, patternLength, i, set))
            {
                return i + patternLength;
            }
        }

        return -1;
    }

    private static bool IsNonRepeatingBlockHashSet_ReUse_AtPos(string line, int patternLength, int pos, HashSet<char> set)
    {
        for (int j = 0; j < patternLength; j++)
        {
            var c = line[pos + j];
            if (set.Contains(c))
            {
                set.Clear();
                return false;
            }

            set.Add(c);
        }

        return true;
    }

}