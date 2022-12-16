using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day16 : Exercise
{
    private static readonly Regex ValvePattern = new("Valve (\\w\\w) has flow rate=(\\d+); tunnels? leads? to valves? (.*)");
    public long ExecutePart1(string[] lines)
    {
        var valves = ParseAllValves(lines);
        var valvesDictionary = valves.ToDictionary(x => x.Name, x => x);
        foreach (var valve in valves)
        {
            valve.ComputeAllTravelTimes(valvesDictionary);
        }

        int time = 30;
        Dictionary<Valve, bool> openValves = valves.ToDictionary(x => x, x => false);
        var firstValve = valvesDictionary["AA"];
        openValves[firstValve] = true;
        var result = firstValve.FindBestFlow(valves, 30, 0, openValves);

        return result.totalFlow;
    }



    public long ExecutePart2(string[] lines)
    {
        return -2;
    }

    public static Valve[] ParseAllValves(string[] lines)
    {
        return lines.Select(Parse).ToArray();
    }

    public static Valve Parse(string line)
    {
        Match match = ValvePattern.Match(line);
        string name = match.Groups[1].Value;
        int flowRate = int.Parse(match.Groups[2].Value);
        string rawAdjacents = match.Groups[3].Value;
        string[] adjacentValves = rawAdjacents.Split(", ");
        return new Valve(name, flowRate, adjacentValves);
    }

    public record Valve(string Name, int FlowRate, string[] AdjacentValveIndices)
    {
        private Dictionary<string, int> _travelTimes;

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

        public void ComputeAllTravelTimes(Dictionary<string, Valve> valves)
        {
            Dictionary<string, int> travelTimes = valves.ToDictionary(x => x.Key, x => 99);

            Queue<(Valve valve, int cost)> valveQueue = new ();
            valveQueue.Enqueue((this, 0));

            while (valveQueue.Count > 0)
            {
                var (valve, cost) = valveQueue.Dequeue();
                cost++;
                foreach (var nextValveIndex in valve.AdjacentValveIndices)
                {
                    var nextValve = valves[nextValveIndex];

                    if ((nextValve == this) || (travelTimes[nextValveIndex] <= cost))
                    {
                        continue;
                    }

                    travelTimes[nextValveIndex] = cost;
                    valveQueue.Enqueue((nextValve, cost));
                }
            }

            _travelTimes = travelTimes;
        }


        public (int totalFlow, List<Valve> visitedValves) FindBestFlow(Valve[] valves, int remainingTime, int currentFlow, Dictionary<Valve, bool> openValves)
        {
            if ((FlowRate != 0) && (remainingTime >= 1))
            {
                // open valve
                remainingTime--;
                currentFlow += remainingTime * FlowRate;
            }

            if (remainingTime == 0)
            {
                return (currentFlow, new List<Valve>() {this});
            }


            var bestFlow = currentFlow;
            var bestPath = new List<Valve>();

            foreach (var nextValve in valves.Where(x => !openValves[x] && (x.FlowRate > 0)))
            {
                int nextRemainingTime = remainingTime - _travelTimes[nextValve.Name];
                if (nextRemainingTime < 0)
                {
                    continue;
                }

                openValves[nextValve] = true;
                var (totalFlow, visitedValves) = nextValve.FindBestFlow(valves, nextRemainingTime, currentFlow, openValves);
                openValves[nextValve] = false;

                if (totalFlow > bestFlow)
                {
                    bestFlow = totalFlow;
                    bestPath = visitedValves;
                }
            }

            bestPath.Add(this);
            return (bestFlow, bestPath);
        }

    }
}
