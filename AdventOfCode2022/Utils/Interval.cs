namespace AdventOfCode2022.Utils {
    public readonly struct Interval
    {
        public readonly int Start;
        public readonly int End;

        public Interval(int start, int end)
        {
            Start = start;
            End = end;
        }

        public int Length => End - Start + 1;

        public bool IsInside(int value)
        {
            return (Start <= value) && (value <= End);
        }

        public bool Intersects(Interval other)
        {
            return (other.Start <= End) && (other.End >= Start);
        }

        public bool IsInside(Interval other)
        {
            return (other.Start >= Start) && (other.End <= End);
        }

        public override string ToString()
        {
            return $"[{Start}/{End}]";
        }

        public bool IsWithin(int min, int max) {
            return (Start >= min) && (End <= max);
        }
    }
}