namespace AdventOfCode2022;

public class Day21 : Exercise
{
    private const string RootName = "root";
    private const string HumanName = "humn";
    
    public long ExecutePart1(string[] lines)
    {
        MonkeyRepository monkeyRepository = new MonkeyRepository(lines.Select(ParseMonkeyInstruction).ToDictionary(x => x.monkeyName, x => x.instruction));
        var result = monkeyRepository.EvaluateValue(RootName);

        return result;
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
        MonkeyRepository monkeyRepository = new MonkeyRepository(lines.Select(ParseMonkeyInstruction).ToDictionary(x => x.monkeyName, x => x.instruction));
        monkeyRepository.PatchRepository();

        return monkeyRepository.ReverseEvaluate(HumanName);
    }

    public class MonkeyRepository
    {
        private Dictionary<string, MonkeyInstruction> _monkeyInstructions;

        public MonkeyRepository(Dictionary<string, MonkeyInstruction> monkeyInstructions)
        {
            _monkeyInstructions = monkeyInstructions;
        }

        public void PatchRepository()
        {
            PatchRoot();
            _monkeyInstructions.Remove(HumanName);
        }

        public void PatchRoot()
        {
            BinaryOperationInstruction root = (BinaryOperationInstruction) _monkeyInstructions[RootName];
            _monkeyInstructions[RootName] = new EqualityInstruction(root.LeftMonkey, root.RightMonkey);
        }

        public long EvaluateValue(string name)
        {
            return _monkeyInstructions[name].Execute(this);
        }

        public long ReverseEvaluate(string monkeyName)
        {
            KeyValuePair<string, MonkeyInstruction> monkeyInstruction = _monkeyInstructions.FirstOrDefault(x => x.Value.Uses(monkeyName));
            var unknownName = monkeyInstruction.Key;
            var instruction = monkeyInstruction.Value;

            var equalityValue = unknownName == RootName ? 0 : ReverseEvaluate(unknownName);

            return instruction.ReverseEvaluate(equalityValue, this, monkeyName);
        }
    }
    
    public interface MonkeyInstruction
    {
        long Execute(MonkeyRepository monkeyRepository);
        bool Uses(string monkeyName);
        long ReverseEvaluate(long equalityValue, MonkeyRepository monkeyRepository, string operandToInvert);
    }

    public class ValueInstruction : MonkeyInstruction
    {
        public readonly long Value;

        public ValueInstruction(long value)
        {
            Value = value;
        }

        public long Execute(MonkeyRepository monkeyRepository)
        {
            return Value;
        }

        public bool Uses(string monkeyName)
        {
            return false;
        }

        public long ReverseEvaluate(long equalityValue, MonkeyRepository monkeyRepository, string operandToInvert)
        {
            throw new NotImplementedException();
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
            var left = monkeyRepository.EvaluateValue(LeftMonkey);
            var right = monkeyRepository.EvaluateValue(RightMonkey);
            return Operation.Execute(left, right);
        }

        public bool Uses(string monkeyName)
        {
            return (monkeyName == LeftMonkey) || (monkeyName == RightMonkey);
        }

        public long ReverseEvaluate(long equalityValue, MonkeyRepository monkeyRepository, string operandToInvert)
        {
            string otherName = operandToInvert == LeftMonkey ? RightMonkey : LeftMonkey;
            var otherValue = monkeyRepository.EvaluateValue(otherName);

            if (Operation == AdditionOperation)
            {
                return equalityValue - otherValue;
            }

            if (Operation == SubstractOperation)
            {
                // eq = left - right
                if (operandToInvert == LeftMonkey)
                {
                    return equalityValue + otherValue;
                }

                return otherValue - equalityValue;
            }

            if (Operation == MultiplyOperation)
            {
                // eq = left * right
                return equalityValue / otherValue;
            }

            if (Operation == DivideOperation)
            {
                // eq = left / right
                if (operandToInvert == LeftMonkey)
                {
                    return equalityValue * otherValue;
                }

                return otherValue / equalityValue;
            }

            throw new Exception();
        }

        public override string ToString()
        {
            return $"{LeftMonkey} {_intOperationsText[Operation]} {RightMonkey}";
        }
    }

    public class EqualityInstruction : MonkeyInstruction
    {
        public readonly string LeftMonkey;
        public readonly string RightMonkey;

        public EqualityInstruction(string leftMonkey, string rightMonkey)
        {
            LeftMonkey = leftMonkey;
            RightMonkey = rightMonkey;
        }

        public long Execute(MonkeyRepository monkeyRepository)
        {
            var left = monkeyRepository.EvaluateValue(LeftMonkey);
            var right = monkeyRepository.EvaluateValue(RightMonkey);
            return left.CompareTo(right);
        }

        public bool Uses(string monkeyName)
        {
            return (monkeyName == LeftMonkey) || (monkeyName == RightMonkey);
        }

        public long ReverseEvaluate(long equalityValue, MonkeyRepository monkeyRepository, string operandToInvert)
        {
            // left == right
            string otherName = operandToInvert == LeftMonkey ? RightMonkey : LeftMonkey;
            var otherValue = monkeyRepository.EvaluateValue(otherName);
            return otherValue;
        }
    }

    public interface IntBinaryOperation
    {
        long Execute(long left, long right);
    }

    public class SingleResultBinaryOperation : IntBinaryOperation
    {
        private readonly Func<long, long, long> _operation;
        public char Symbol { get; }

        public SingleResultBinaryOperation(char symbol, Func<long, long, long> operation)
        {
            _operation = operation;
            Symbol = symbol;
        }

        public long Execute(long left, long right)
        {
            return _operation(left, right);
        }

        public override string ToString()
        {
            return Symbol.ToString();
        }
    }

    public static readonly IntBinaryOperation AdditionOperation = new SingleResultBinaryOperation('+', (a, b) => a + b);
    public static readonly IntBinaryOperation SubstractOperation = new SingleResultBinaryOperation('-', (a, b) => a - b);
    public static readonly IntBinaryOperation MultiplyOperation = new SingleResultBinaryOperation('*', (a, b) => a * b);
    public static readonly IntBinaryOperation DivideOperation = new SingleResultBinaryOperation('/', (a, b) => a / b);

    private static readonly IReadOnlyDictionary<char, IntBinaryOperation> _intOperations = new Dictionary<char, IntBinaryOperation>()
    {
        {'+', AdditionOperation},
        {'-', SubstractOperation},
        {'*', MultiplyOperation},
        {'/', DivideOperation},
    };

    private static readonly IReadOnlyDictionary<IntBinaryOperation, char> _intOperationsText = new Dictionary<IntBinaryOperation, char>()
    {
        { AdditionOperation, '+' },
        { SubstractOperation, '-' },
        { MultiplyOperation, '*' },
        { DivideOperation, '/' },
    };
}
