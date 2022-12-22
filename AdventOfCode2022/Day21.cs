namespace AdventOfCode2022;

public class Day21 : Exercise
{
    private const string RootName = "root";
    private const string HumanName = "humn";
    
    public long ExecutePart1(string[] lines)
    {
        MonkeyRepository monkeyRepository = new MonkeyRepository(lines.Select(ParseMonkeyInstruction).ToDictionary(x => x.monkeyName, x => x.instruction));
        return monkeyRepository.EvaluateValue(RootName);
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

        monkeyRepository.Invert(HumanName);
        
        return monkeyRepository.EvaluateValue(HumanName);
    }

    public class MonkeyRepository
    {
        private Dictionary<string, MonkeyInstruction> _monkeyInstructions;

        public MonkeyRepository(Dictionary<string, MonkeyInstruction> monkeyInstructions)
        {
            _monkeyInstructions = monkeyInstructions;
        }

        public MonkeyInstruction this[string name] => _monkeyInstructions[name];

        public void PatchRepository()
        {
            BinaryOperationInstruction root = (BinaryOperationInstruction)_monkeyInstructions[RootName];
            _monkeyInstructions[RootName] = new EqualityInstruction(root.LeftMonkey, root.RightMonkey);
            _monkeyInstructions.Remove(HumanName);
        }

        public long EvaluateValue(string name)
        {
            return _monkeyInstructions[name].Execute(this);
        }

        public void Invert(string monkeyName)
        {
            Dictionary<string, MonkeyInstruction> newInstructions = new Dictionary<string, MonkeyInstruction>();
            while (monkeyName != string.Empty)
            {
                KeyValuePair<string, MonkeyInstruction> monkeyInstruction = _monkeyInstructions.FirstOrDefault(x => x.Value.Uses(monkeyName));
                if (!default(KeyValuePair<string, MonkeyInstruction>).Equals(monkeyInstruction))
                {
                    newInstructions[monkeyName] = monkeyInstruction.Value.Invert(monkeyInstruction.Key, monkeyName);
                    _monkeyInstructions.Remove(monkeyInstruction.Key);
                    monkeyName = monkeyInstruction.Key;
                }
                else
                {
                    monkeyName = string.Empty;
                }
            }

            foreach (var pair in newInstructions)
            {
                _monkeyInstructions.Add(pair.Key, pair.Value);
            }
        }
    }
    
    public interface MonkeyInstruction
    {
        long Execute(MonkeyRepository monkeyRepository);
        bool Uses(string monkeyName);
        MonkeyInstruction Invert(string currentName, string operandToInvert);
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

        public bool Uses(string monkeyName)
        {
            return false;
        }

        public MonkeyInstruction Invert(string currentName, string operandToInvert)
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
            return Operation(monkeyRepository.EvaluateValue(LeftMonkey),
                monkeyRepository.EvaluateValue(RightMonkey));
        }

        public bool Uses(string monkeyName)
        {
            return (monkeyName == LeftMonkey) || (monkeyName == RightMonkey);
        }

        public MonkeyInstruction Invert(string currentName, string operandToInvert)
        {
            IntBinaryOperation oppositeIntOperation = _oppositeIntOperations[Operation];

            string otherName = operandToInvert == LeftMonkey ? RightMonkey : LeftMonkey;
            
            if ((Operation == AdditionOperation) || (Operation == MultiplyOperation))
            {
                return new BinaryOperationInstruction(currentName, otherName, oppositeIntOperation);
            }
            
            if (operandToInvert == LeftMonkey)
            {
                return new BinaryOperationInstruction(currentName, RightMonkey, oppositeIntOperation);
            }

            return new BinaryOperationInstruction(LeftMonkey, currentName, oppositeIntOperation);
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
            return monkeyRepository.EvaluateValue(LeftMonkey) == monkeyRepository.EvaluateValue(RightMonkey) ? 1 : 0;
        }

        public bool Uses(string monkeyName)
        {
            return (monkeyName == LeftMonkey) || (monkeyName == RightMonkey);
        }

        public MonkeyInstruction Invert(string currentName, string operandToInvert)
        {
            string otherName = operandToInvert == LeftMonkey ? RightMonkey : LeftMonkey;
            return new ReferenceInstruction(otherName);
        }
    }


    public class ReferenceInstruction : MonkeyInstruction
    {
        public readonly string MonkeyName;

        public ReferenceInstruction(string monkeyName)
        {
            MonkeyName = monkeyName;
        }

        public long Execute(MonkeyRepository monkeyRepository)
        {
            return monkeyRepository.EvaluateValue(MonkeyName);
        }

        public bool Uses(string monkeyName)
        {
            throw new NotImplementedException();
        }

        public MonkeyInstruction Invert(string currentName, string operandToInvert)
        {
            throw new NotImplementedException();
        }
    }
    
    public delegate long IntBinaryOperation(long left, long right);

    public static readonly IntBinaryOperation AdditionOperation = (a, b) => a + b;
    public static readonly IntBinaryOperation SubstractOperation = (a, b) => a - b;
    public static readonly IntBinaryOperation MultiplyOperation = (a, b) => a * b;
    public static readonly IntBinaryOperation DivideOperation = (a, b) =>
    {
        if (a % b != 0)
        {
            throw new Exception("TODO");
        } 
        return a / b;
    };
    
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
    
    private static readonly IReadOnlyDictionary<IntBinaryOperation, IntBinaryOperation> _oppositeIntOperations = new Dictionary<IntBinaryOperation, IntBinaryOperation>()
    {
        {AdditionOperation, SubstractOperation},
        {SubstractOperation, AdditionOperation},
        {MultiplyOperation, DivideOperation},
        {DivideOperation, MultiplyOperation},
    };
    
    

}
