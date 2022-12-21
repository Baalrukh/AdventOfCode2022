namespace AdventOfCode2022;

public class Day20 : Exercise
{
    public long ExecutePart1(string[] lines)
    {
        return GetCoordinates(lines).Sum();
    }

    public IReadOnlyList<int> GetCoordinates(string[] lines)
    {
        var sequence = new NumberSequence(lines.Select(x => new Number(int.Parse(x))).ToList());
        sequence.UnMix();
        return sequence.FindNthElementsAfter0(new []{1000, 2000, 3000}).Select(x => x.Value).ToArray();

    }

    public long ExecutePart2(string[] lines)
    {
        return -2;
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
            int offset = number.Value;

            int newIndex = (index + offset);
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
                _numbers.Insert(newIndex, number);
            }
        }

        public override string ToString()
        {
            return string.Join(", ", _numbers);
        }

        public void UnMix()
        {
            var order = new List<Number>(_numbers);
            foreach (var number in order)
            {
                var index = _numbers.IndexOf(number);
                ShiftNumberAtIndex(index);
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

    }
    public class Number
    {
        public readonly int Value;

        public Number(int value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

}
