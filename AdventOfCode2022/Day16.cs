using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day16 : Exercise
{
    private static readonly Regex ValvePattern =
        new Regex("Valve (\\w\\w) has flow rate=(\\d+); tunnels lead to valves (.*)"); 
    public long ExecutePart1(string[] lines)
    {
        return -1;
    }

    public long ExecutePart2(string[] lines)
    {
        return -2;
    }

    public static Valve Parse(string line)
    {
        Match match = ValvePattern.Match(line);
        string name = match.Groups[1].Value;
        int flowRate = int.Parse(match.Groups[2].Value);
        string rawAdjacents = match.Groups[3].Value;
        string[] tokens = rawAdjacents.Split(", ");
        int[] adjacentValves = tokens.Select(x => x[0] - 'A').ToArray();
        return new Valve(name, flowRate, adjacentValves);
    }

    public record Valve(string Name, int FlowRate, int[] AdjacentValveIndices)
    {
        public virtual bool Equals(Valve? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && FlowRate == other.FlowRate && AdjacentValveIndices.SequenceEqual(other.AdjacentValveIndices);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, FlowRate, AdjacentValveIndices);
        }
    }
}
