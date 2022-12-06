namespace AdventOfCode2022;

public class Day06 : Exercise
{
    public long ExecutePart1(string[] lines)
    {
        string line = lines[0];
        return FindNonRepeatingBlock(line, 4);
    }

    public long ExecutePart2(string[] lines)
    {
        string line = lines[0];
        return FindNonRepeatingBlock(line, 14);
    }

    private static long FindNonRepeatingBlock(string line, int patternLength)
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
}
