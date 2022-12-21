namespace AdventOfCode2022;

public class Day20 : Exercise
{
    private const long DecryptionKey = 811589153L;

    public long ExecutePart1(string[] lines)
    {
        return GetCoordinates(lines).Sum();
    }

    public IReadOnlyList<long> GetCoordinates(string[] lines)
    {
        var sequence = new NumberSequence(lines.Select(x => new Number(int.Parse(x))).ToList());
        sequence.UnMix();
        return sequence.FindNthElementsAfter0(new []{1000, 2000, 3000}).Select(x => x.Value).ToArray();
    }

    public long ExecutePart2(string[] lines)
    {
        return GetCoordinates_Part2(lines).Sum();
    }

    public IEnumerable<long> GetCoordinates_Part2(string[] lines)
    {
        var sequence = new NumberSequence(lines.Select(x => new Number(int.Parse(x))).ToList());
        sequence.MultiplyNumbers(DecryptionKey);
        sequence.UnMix(10);
        return sequence.FindNthElementsAfter0(new []{1000, 2000, 3000}).Select(x => x.Value).ToArray();
    }

    public static NumberSequence ParseNumbers(string commaSeparatedNumbers)
    {
        return new NumberSequence(commaSeparatedNumbers.Split(", ").Select(x => new Number(int.Parse(x))).ToList());
    }


    public class NumberSequence
    {
        private readonly List<Number> _numbers;

        public NumberSequence(List<Number> numbers)
        {
            _numbers = numbers;
        }

        public void ShiftNumberAtIndex(int index)
        {
            var number = _numbers[index];
            long offset = number.Value;

            long newIndex = (index + offset);
            var modulo = _numbers.Count - 1;
            if (newIndex >= 0)
            {
                newIndex %= modulo;
            }
            else
            {
                newIndex = ((newIndex % modulo) + modulo) % modulo;
            }
            _numbers.RemoveAt(index);
            if (newIndex == 0)
            {
                _numbers.Add(number);
            }
            else
            {
                _numbers.Insert((int)newIndex, number);
            }
        }

        public override string ToString()
        {
            return string.Join(", ", _numbers);
        }

        public void UnMix(int iterationCount = 1)
        {
            var order = new List<Number>(_numbers);
            for (int i = 0; i < iterationCount; i++)
            {
                foreach (var number in order)
                {
                    var index = _numbers.IndexOf(number);
                    ShiftNumberAtIndex(index);
                }
            }
        }

        public Number FindNthElementAfter0(int index)
        {
            var zeroIndex = _numbers.FindIndex(x => x.Value == 0);
            return _numbers[(zeroIndex + index) % _numbers.Count];
        }

        public IEnumerable<Number> FindNthElementsAfter0(IEnumerable<int> indices)
        {
            var zeroIndex = _numbers.FindIndex(x => x.Value == 0);
            return indices.Select(i => _numbers[(zeroIndex + i) % _numbers.Count]);
        }

        public void MultiplyNumbers(long factor)
        {
            foreach (var number in _numbers)
            {
                number.Multiply(factor);
            }
        }
    }
    public class Number
    {
        public long Value { get; private set; }

        public Number(int value)
        {
            Value = value;
        }

        public void Multiply(long factor)
        {
            Value *= factor;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

}
