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
        int iterationCount = 20;
        var group = MonkeyGroup.ParseMonkeys(lines, MonkeyGroup.Part1WorryLevelAdjustment);
        for (int i = 0; i < iterationCount; i++)
        {
            group.ExecuteRound();
        }

        return group.Monkeys.OrderByDescending(x => x.InspectionCount).Take(2)
            .Aggregate(1, (score, monkey) => score * monkey.InspectionCount);
    }

    public long ExecutePart2(string[] lines)
    {
        int iterationCount = 10000;
        var group = MonkeyGroup.ParseMonkeys(lines, MonkeyGroup.Part1WorryLevelAdjustment);
        int globalModuloValue = group.Monkeys.Aggregate(1, (i, monkey) => i * monkey.ModuloTestValue);
        group._worryLevelAdjustment = level => level % globalModuloValue;
        for (int i = 0; i < iterationCount; i++)
        {
            group.ExecuteRound();
        }

        return group.Monkeys.OrderByDescending(x => x.InspectionCount).Take(2)
            .Aggregate(1L, (score, monkey) => score * monkey.InspectionCount);
    }

    public interface IntMathOperation
    {
        long Execute(long sourceValue);
    }

    public record AddIntOperation(int Amount) : IntMathOperation
    {
        public long Execute(long sourceValue)
        {
            return sourceValue + Amount;
        }
    }

    public record MultiplyIntOperation(int Amount) : IntMathOperation
    {
        public long Execute(long sourceValue)
        {
            return sourceValue * Amount;
        }
    }

    private class SquareIntOperation : IntMathOperation
    {
        public long Execute(long sourceValue)
        {
            return sourceValue * sourceValue;
        }
    }
    
    public class Monkey
    {
        public readonly Queue<long> ItemWorries;

        public readonly IntMathOperation WorryIncreaseOperation;

        public readonly int ModuloTestValue;

        public readonly int DivisibleMonkeyIndex;
        public readonly int NotDivisibleMonkeyIndex;

        public Monkey(IEnumerable<long> itemWorries, IntMathOperation worryIncreaseOperation, int moduloTestValue, int divisibleMonkeyIndex, int notDivisibleMonkeyIndex)
        {
            ItemWorries = new Queue<long>(itemWorries);
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

        public void ExecuteStep(IReadOnlyList<Monkey> monkeys, MonkeyGroup.WorryLevelAdjustment worryLevelAdjustment)
        {
            long itemWorry = ItemWorries.Dequeue();
            itemWorry = WorryIncreaseOperation.Execute(itemWorry);
            // itemWorry /= 3;
            itemWorry = worryLevelAdjustment(itemWorry);
            int nextMonkeyIndex = itemWorry % ModuloTestValue == 0 ? DivisibleMonkeyIndex : NotDivisibleMonkeyIndex;
            monkeys[nextMonkeyIndex].ItemWorries.Enqueue(itemWorry);
            InspectionCount++;
        }

        public void ExecuteRound(IReadOnlyList<Monkey> monkeys, MonkeyGroup.WorryLevelAdjustment worryLevelAdjustment)
        {
            while (ItemWorries.Count > 0)
            {
                ExecuteStep(monkeys, worryLevelAdjustment);
            }
        }
    }


    public class MonkeyGroup
    {
        public IReadOnlyList<Monkey> Monkeys { get; }
        public delegate long WorryLevelAdjustment(long worryLevel);

        public WorryLevelAdjustment _worryLevelAdjustment { private get; set; }

        public static long Part1WorryLevelAdjustment(long level) => level / 3;
        
        public MonkeyGroup(IReadOnlyList<Monkey> monkeys, WorryLevelAdjustment worryLevelAdjustment)
        {
            Monkeys = monkeys;
            _worryLevelAdjustment = worryLevelAdjustment;
        }

        public Monkey this[int i] => Monkeys[i];
        
        public static Monkey ParseMonkey(IReadOnlyList<string> lines)
        {
            // "Monkey 0:",
            // "  Starting items: 79, 98",
            // "  Operation: new = old * 19",
            // "  Test: divisible by 23",
            // "    If true: throw to monkey 2",
            // "    If false: throw to monkey 3",

            var items = lines[1]["  Starting items: ".Length..].Split(", ").Select(long.Parse);
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

        public static MonkeyGroup ParseMonkeys(string[] lines, WorryLevelAdjustment worryLevelAdjustment)
        {
            return new MonkeyGroup(lines.Batch(x => x.Length == 0, true).Select(ParseMonkey).ToArray(), worryLevelAdjustment);
        }

        public void ExecuteRound()
        {
            foreach (Monkey monkey in Monkeys)
            {
                monkey.ExecuteRound(Monkeys, _worryLevelAdjustment);
            }
        }
    }


}
