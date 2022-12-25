using System.Text;

namespace AdventOfCode2022;

public class Day25 : Exercise
{
    public long ExecutePart1(string[] lines)
    {
        Console.WriteLine(GetSNAFUSum(lines));
        return -1;
    }

    public long ExecutePart2(string[] lines)
    {
        return -2;
    }

    public static string GetSNAFUSum(string[] lines)
    {
        long sum = lines.Select(x => new SNAFUNumber(x)).Select(x => x.LongValue).Sum();
        return SNAFUNumber.FromLong(sum).TextValue;
    }

    public class SNAFUNumber
    {
        private string _text;

        public string TextValue => _text;

        public SNAFUNumber(string text)
        {
            _text = text;
        }

        public long LongValue
        {
            get
            {
                long val = 0;
                foreach (char c in _text)
                {
                    int digit;
                    if (c == '-') digit = -1;
                    else if (c == '=') digit = -2;
                    else digit = c - '0';

                    val = val * 5 + digit;
                }

                return val;
            }
        }

        public static SNAFUNumber FromLong(long value)
        {
            StringBuilder builder = new();
            while (value > 0)
            {
                long digit = value % 5;
                long remaining = value / 5;
                if (digit == 4)
                {
                    builder.Insert(0, '-');
                    remaining++;
                }
                else if (digit == 3)
                {
                    builder.Insert(0, '=');
                    remaining++;
                } 
                else
                {
                    builder.Insert(0, (char)(digit + '0'));
                }

                value = remaining;
            }

            return new SNAFUNumber(builder.ToString());
        }
    }
}
