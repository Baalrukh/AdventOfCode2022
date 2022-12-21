namespace AdventOfCode2022;

public class Day21 : Exercise
{
    public long ExecutePart1(string[] lines)
    {
        MonkeyRepository monkeyRepository = new MonkeyRepository(lines.Select(ParseMonkeyInstruction).ToDictionary(x => x.monkeyName, x => x.instruction));
        return monkeyRepository["root"].Execute(monkeyRepository);
    }

    private (string monkeyName, MonkeyInstruction instruction) ParseMonkeyInstruction(string line)
    {
        string[] tokens = line.Split(": ");
        return (tokens[0], ParseInstruction(tokens[1]));
    }

    private MonkeyInstruction ParseInstruction(string text)
    {
        if (int.TryParse(text, out var intValue))
        {
            return new ValueInstruction(intValue);
        }

        string[] tokens = text.Split(" ");
        return new BinaryOperationInstruction(tokens[0], tokens[2], _intOperations[tokens[1][0]]);
    }

    public long ExecutePart2(string[] lines)
    {
        return -2;
    }

    public class MonkeyRepository
    {
        private Dictionary<string, MonkeyInstruction> _monkeyInstructions;

        public MonkeyRepository(Dictionary<string, MonkeyInstruction> monkeyInstructions)
        {
            _monkeyInstructions = monkeyInstructions;
        }

        public MonkeyInstruction this[string name] => _monkeyInstructions[name];
    }
    
    public interface MonkeyInstruction
    {
        long Execute(MonkeyRepository monkeyRepository);
    }

    public class ValueInstruction : MonkeyInstruction
    {
        public readonly int Value;

        public ValueInstruction(int value)
        {
            Value = value;
        }

        public long Execute(MonkeyRepository monkeyRepository)
        {
            return Value;
        }
    }

    public class BinaryOperationInstruction : MonkeyInstruction
    {
        public readonly string LeftMonkey;
        public readonly string RightMonkey;
        public readonly IntBinaryOperation Operation;

        public BinaryOperationInstruction(string leftMonkey, string rightMonkey, IntBinaryOperation operation)
        {
            LeftMonkey = leftMonkey;
            RightMonkey = rightMonkey;
            Operation = operation;
        }

        public long Execute(MonkeyRepository monkeyRepository)
        {
            return Operation(monkeyRepository[LeftMonkey].Execute(monkeyRepository),
                monkeyRepository[RightMonkey].Execute(monkeyRepository));
        }
    }

    public delegate long IntBinaryOperation(long left, long right);

    public static readonly IntBinaryOperation AdditionOperation = (a, b) => a + b;
    public static readonly IntBinaryOperation SubstractOperation = (a, b) => a - b;
    public static readonly IntBinaryOperation MultiplyOperation = (a, b) => a * b;
    public static readonly IntBinaryOperation DivideOperation = (a, b) => a / b;
    
    private static readonly IReadOnlyDictionary<char, IntBinaryOperation> _intOperations = new Dictionary<char, IntBinaryOperation>()
    {
        {'+', AdditionOperation},
        {'-', SubstractOperation},
        {'*', MultiplyOperation},
        {'/', DivideOperation},
    };

}
