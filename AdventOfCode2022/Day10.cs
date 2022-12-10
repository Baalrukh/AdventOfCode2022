using System.Text;

namespace AdventOfCode2022;

public class Day10 : Exercise
{
    public static readonly NoopInstuction NoopInstructionInstance = new NoopInstuction();
    private const string AddXCommandName = "addx";
    private const string NoopCommandName = "noop";
    
    public long ExecutePart1(string[] lines)
    {
        var allCycles = EnumerateAllCycleChanges(lines);
        var enumerator = allCycles.GetEnumerator();

        int signalStrength = 0;
        int[] sampleCycles = { 20, 60, 100, 140, 180, 220 };
        int currentCycle = 0;
        
        int registerValue = 1;
        foreach (int sampleCycle in sampleCycles)
        {
            registerValue = ExecuteCycles(sampleCycle - 1 - currentCycle, enumerator, registerValue);
            currentCycle = sampleCycle - 1;
            signalStrength += (currentCycle + 1) * registerValue;
        }

        return signalStrength;
    }

    public static IEnumerable<int> EnumerateAllCycleChanges(string[] lines)
    {
        return lines.Select(Parse).SelectMany(x => x.GetXChangesPerCycle());
    }

    public long ExecutePart2(string[] lines)
    {
        foreach (string s in DrawOutput(lines))
        {
            Console.WriteLine(s);
        }
        return -2;
    }

    public static Instruction Parse(string text)
    {
        if (text == NoopCommandName)
        {
            return NoopInstructionInstance;
        }

        if (text.StartsWith(AddXCommandName))
        {
            return new AddInstruction(int.Parse(text.Substring(AddXCommandName.Length + 1)));
        }

        throw new Exception("Unsupported instruction " + text);
    }
    
    public interface Instruction
    {
        IEnumerable<int> GetXChangesPerCycle();
    }

    public record AddInstruction(int Amount) : Instruction
    {
        public IEnumerable<int> GetXChangesPerCycle()
        {
            yield return 0;
            yield return Amount;
        }
    }

    public class NoopInstuction : Instruction
    {
        public IEnumerable<int> GetXChangesPerCycle()
        {
            yield return 0;
        }
    }

    public static int ExecuteCycles(int cycleCount, IEnumerator<int> cycleChangeEnumerator, int registerValue)
    {
        for (int i = 0; i < cycleCount; i++)
        {
            cycleChangeEnumerator.MoveNext();
            registerValue += cycleChangeEnumerator.Current;
        }

        return registerValue;
    }

    public static IEnumerable<string> DrawOutput(string[] lines)
    {
        var allCycles = EnumerateAllCycleChanges(lines);
        int registerValue = 1;

        int crtX = 0;
        StringBuilder stringBuilder = new();
        foreach (int cycle in allCycles)
        {
            stringBuilder.Append((crtX >= registerValue - 1) && (crtX <= registerValue + 1) ? '#' : ".");
            registerValue += cycle;
            crtX++;
            if (crtX == 40)
            {
                crtX = 0;
                yield return stringBuilder.ToString();
                stringBuilder.Clear();
            }
        }
    }
}
