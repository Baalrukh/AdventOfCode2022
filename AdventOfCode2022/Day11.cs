using AdventOfCode2022.Utils;

namespace AdventOfCode2022;

public class Day11 : Exercise
{
    public static readonly IntMathOperation SquareIntOperationInstance = new SquareIntOperation();
    private const string MathOperationBasePrefix = "  Operation: new = old "; 
    private const string AddPrefix = "+ ";
    private const string MultiplyPrefix = "* ";
    private const string SquareText = "* old";
    
    public long ExecutePart1(string[] lines)
    {
        var monkeys = ParseMonkeys(lines);
        for (int i = 0; i < 20; i++)
        {
            ExecuteRound(monkeys);
        }

        return monkeys.OrderByDescending(x => x.InspectionCount).Take(2)
            .Aggregate(1, (score, monkey) => score * monkey.InspectionCount);
    }

    public long ExecutePart2(string[] lines)
    {
        return -2;
    }

    public interface IntMathOperation
    {
        int Execute(int sourceValue);
    }

    public record AddIntOperation(int Amount) : IntMathOperation
    {
        public int Execute(int sourceValue)
        {
            return sourceValue + Amount;
        }
    }

    public record MultiplyIntOperation(int Amount) : IntMathOperation
    {
        public int Execute(int sourceValue)
        {
            return sourceValue * Amount;
        }
    }

    private class SquareIntOperation : IntMathOperation
    {
        public int Execute(int sourceValue)
        {
            return sourceValue * sourceValue;
        }
    }
    
    public class Monkey
    {
        public readonly Queue<int> ItemWorries;

        public readonly IntMathOperation WorryIncreaseOperation;

        public readonly int ModuloTestValue;

        public readonly int DivisibleMonkeyIndex;
        public readonly int NotDivisibleMonkeyIndex;

        public Monkey(IEnumerable<int> itemWorries, IntMathOperation worryIncreaseOperation, int moduloTestValue, int divisibleMonkeyIndex, int notDivisibleMonkeyIndex)
        {
            ItemWorries = new Queue<int>(itemWorries);
            WorryIncreaseOperation = worryIncreaseOperation;
            ModuloTestValue = moduloTestValue;
            DivisibleMonkeyIndex = divisibleMonkeyIndex;
            NotDivisibleMonkeyIndex = notDivisibleMonkeyIndex;
        }

        public int InspectionCount { get; private set; }

        protected bool Equals(Monkey other)
        {
            return ItemWorries.SequenceEqual(other.ItemWorries)
                   && WorryIncreaseOperation.Equals(other.WorryIncreaseOperation)
                   && ModuloTestValue == other.ModuloTestValue
                   && DivisibleMonkeyIndex == other.DivisibleMonkeyIndex
                   && NotDivisibleMonkeyIndex == other.NotDivisibleMonkeyIndex;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Monkey)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ItemWorries, WorryIncreaseOperation, ModuloTestValue, DivisibleMonkeyIndex, NotDivisibleMonkeyIndex);
        }

        public void ExecuteStep(IReadOnlyList<Monkey> monkeys)
        {
            int itemWorry = ItemWorries.Dequeue();
            itemWorry = WorryIncreaseOperation.Execute(itemWorry);
            itemWorry /= 3;
            int nextMonkeyIndex = itemWorry % ModuloTestValue == 0 ? DivisibleMonkeyIndex : NotDivisibleMonkeyIndex;
            monkeys[nextMonkeyIndex].ItemWorries.Enqueue(itemWorry);
            InspectionCount++;
        }

        public void ExecuteRound(IReadOnlyList<Monkey> monkeys)
        {
            while (ItemWorries.Count > 0)
            {
                ExecuteStep(monkeys);
            }
        }
    }

    public static Monkey ParseMonkey(IReadOnlyList<string> lines)
    {
        // "Monkey 0:",
        // "  Starting items: 79, 98",
        // "  Operation: new = old * 19",
        // "  Test: divisible by 23",
        // "    If true: throw to monkey 2",
        // "    If false: throw to monkey 3",

        var items = lines[1]["  Starting items: ".Length..].Split(", ").Select(int.Parse);
        IntMathOperation operation = ParseIntMathOperation(lines[2]);
        int modulo = int.Parse(lines[3]["  Test: divisible by ".Length..]);
        int trueMonkeyIndex = int.Parse(lines[4]["    If true: throw to monkey ".Length..]);
        int falseMonkeyIndex = int.Parse(lines[5]["    If false: throw to monkey ".Length..]);
        return new Monkey(items, operation, modulo, trueMonkeyIndex, falseMonkeyIndex);
    }

    public static IntMathOperation ParseIntMathOperation(string line)
    {
        string text = line[MathOperationBasePrefix.Length..];
        if (text.StartsWith(AddPrefix))
        {
            return new AddIntOperation(int.Parse(text[2..]));
        }

        if (text == SquareText)
        {
            return SquareIntOperationInstance;
        }
        
        if (text.StartsWith(MultiplyPrefix))
        {
            return new MultiplyIntOperation(int.Parse(text[2..]));
        }

        throw new Exception("Unsupported operation : " + line);
    }

    public static Monkey[] ParseMonkeys(string[] lines)
    {
        return lines.Batch(x => x.Length == 0, true).Select(ParseMonkey).ToArray();
    }

    public static void ExecuteRound(Monkey[] monkeys)
    {
        foreach (Monkey monkey in monkeys)
        {
            monkey.ExecuteRound(monkeys);
        }
    }

}
