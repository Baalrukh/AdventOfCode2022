using System.Text.RegularExpressions;
using AdventOfCode2022.Utils;

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
        var result = firstValve.FindBestFlow(valves, time, 0, openValves);

        var result2 = FindBestFlowParallel(valves, openValves, new Actor[] {new Actor("Human", time, firstValve)}, 0);

        // return result.totalFlow;
        return result2.Flow;
    }



    public long ExecutePart2(string[] lines)
    {
        var valves = ParseAllValves(lines);
        var valvesDictionary = valves.ToDictionary(x => x.Name, x => x);
        foreach (var valve in valves)
        {
            valve.ComputeAllTravelTimes(valvesDictionary);
        }

        int time = 26;
        Dictionary<Valve, bool> openValves = valves.ToDictionary(x => x, x => false);
        var firstValve = valvesDictionary["AA"];
        openValves[firstValve] = true;

        var result = FindBestFlowParallel(valves, openValves, new Actor[] {new Actor("Human", time, firstValve), new Actor("Elephant", time, firstValve)}, 0);

        return result.Flow;
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

    public class Actor
    {
        public readonly string Name;
        public int RemainingTime;
        public Valve CurrentPosition;

        public Actor(string name, int remainingTime, Valve currentPosition)
        {
            Name = name;
            RemainingTime = remainingTime;
            CurrentPosition = currentPosition;
        }
    }

    public class PathResult
    {
        public int Flow;
        public List<(int, Valve)>[] VisitedValvesByActor;

        public PathResult(int flow)
            : this(flow, new[] {new List<(int, Valve)>(), new List<(int, Valve)>()})
        {
        }

        public PathResult(int flow, List<(int, Valve)>[] visitedValvesByActor)
        {
            Flow = flow;
            VisitedValvesByActor = visitedValvesByActor;
        }
    }

    public static PathResult FindBestFlowParallel(Valve[] valves, Dictionary<Valve, bool> openValves, Actor[] actors, int currentFlow)
    {
        var actorToMove = actors.MaxBy(x => x.RemainingTime);

        var currentValve = actorToMove.CurrentPosition;

        PathResult bestPathResult = new PathResult(currentFlow);

        var remainingTime = actorToMove.RemainingTime;
        foreach (var nextValve in valves.Where(x => !openValves[x] && (x.FlowRate > 0)))
        {
            int nextRemainingTime = remainingTime - currentValve.TravelTimes[nextValve.Name] - 1;
            if (nextRemainingTime < 0)
            {
                continue;
            }
            int flow = currentFlow + nextValve.FlowRate * nextRemainingTime;

            openValves[nextValve] = true;
            actorToMove.CurrentPosition = nextValve;
            actorToMove.RemainingTime = nextRemainingTime;
            PathResult pathResult = FindBestFlowParallel(valves, openValves, actors, flow);
            openValves[nextValve] = false;

            if (pathResult.Flow > bestPathResult.Flow)
            {
                bestPathResult = pathResult;
            }
        }

        var indexOf = actors.IndexOf(actorToMove);
        actorToMove.RemainingTime = remainingTime;
        actorToMove.CurrentPosition = currentValve;
        bestPathResult.VisitedValvesByActor[indexOf].Add((26 - actorToMove.RemainingTime, currentValve));
        return bestPathResult;
    }

    public record Valve(string Name, int FlowRate, string[] AdjacentValveIndices)
    {
        private Dictionary<string, int> _travelTimes;

        public Dictionary<string, int> TravelTimes => _travelTimes;

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
