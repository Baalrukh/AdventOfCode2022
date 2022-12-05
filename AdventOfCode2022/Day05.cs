using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode2022.Utils;

namespace AdventOfCode2022;

public class Day05 : Exercise
{
    public long ExecutePart1(string[] lines)
    {
        Console.WriteLine(ExecutePart1Text(lines));
        return -1;
    }

    public long ExecutePart2(string[] lines)
    {
        Console.WriteLine(ExecutePart2Text(lines));
        return -2;
    }

    public delegate void ExecuteStackCommand(StackCommand stackCommand, IReadOnlyList<CrateStack> stacks);

    public string ExecutePart1Text(string[] lines)
    {
        return ExecutePartText(lines, (command, stacks) => command.Execute(stacks));
    }

    public string ExecutePart2Text(string[] lines)
    {
        return ExecutePartText(lines, (command, stacks) => command.ExecuteKeepOrder(stacks));
    }

    public string ExecutePartText(string[] lines, ExecuteStackCommand executeStackCommand)
    {
        var separatorLineIndex = lines.IndexOf("");
        var stacks = ParseStacks(lines.Take(separatorLineIndex - 1).ToArray());
        var commands = lines.Skip(separatorLineIndex + 1).Select(ParseCommand);
        foreach (var stackCommand in commands)
        {
            executeStackCommand(stackCommand, stacks);
        }

        return stacks.Aggregate("", (txt, x) => txt + x.Crates.Peek());
    }

    public static IReadOnlyList<CrateStack> ParseStacks(string[] lines)
    {
        int stackCount = (lines[0].Length + 1) / 4;
        List<List<char>> stacks = Enumerable.Range(0, stackCount).Select(x => new List<char>()).ToList();

        foreach (var line in lines)
        {
            for (int i = 0; i < stackCount; i++)
            {
                var c = line[i * 4 + 1];
                if (c != ' ')
                {
                    stacks[i].Add(c);
                }
            }
        }

        foreach (var stack in stacks)
        {
            stack.Reverse();
        }

        return stacks.Select(x => new CrateStack(new Stack<char>(x))).ToList();
    }

    private static readonly Regex CommandRegex = new Regex("move (\\d+) from (\\d+) to (\\d+)");

    public static StackCommand ParseCommand(string line)
    {
        var match = CommandRegex.Match(line);
        return new StackCommand(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value) - 1, int.Parse(match.Groups[3].Value) - 1);
    }

    public record StackCommand(int Count, int FromStackIndex, int ToStackIndex)
    {
        public void Execute(IReadOnlyList<CrateStack> stacks)
        {
            for (int i = 0; i < Count; i++)
            {
                var crate = stacks[FromStackIndex].Crates.Pop();
                stacks[ToStackIndex].Crates.Push(crate);
            }
        }

        public void ExecuteKeepOrder(IReadOnlyList<CrateStack> stacks)
        {
            Stack<char> tmp = new Stack<char>();
            for (int i = 0; i < Count; i++)
            {
                var crate = stacks[FromStackIndex].Crates.Pop();
                tmp.Push(crate);
            }

            var stack = stacks[ToStackIndex].Crates;
            foreach (var crate in tmp)
            {
                stack.Push(crate);
            }
        }
    }

    public class CrateStack
    {
        public readonly Stack<char> Crates;

        public CrateStack(Stack<char> crates)
        {
            Crates = crates;
        }
    }

}
