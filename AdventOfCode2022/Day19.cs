using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day19 : Exercise
{
    public const int OreIndex = 0;
    public const int ClayIndex = 1;
    public const int ObsidianIndex = 2;
    public const int GeodeIndex = 3;

    private static readonly Regex BlueprintPattern = new (
        "Blueprint (\\d+): Each ore robot costs (\\d+) ore. Each clay robot costs (\\d+) ore. Each obsidian robot costs (\\d+) ore and (\\d+) clay. Each geode robot costs (\\d+) ore and (\\d+) obsidian.");


    // 1327 too low
    public long ExecutePart1(string[] lines)
    {
        
        // pas 1678, ni 1415
        var list = lines.Select(ParseBlueprint).Select(x => x.GetCollectedGeodeCount(24)).ToList();
        Console.WriteLine(string.Join("\n", list));
        return list.Select((x, i) => x * (i + 1)).Sum();
    }

    public long ExecutePart2(string[] lines)
    {
        return -2;
    }

    public static Blueprint ParseBlueprint(string line)
    {
        var match = BlueprintPattern.Match(line);
        int id = ParseInt(match, 1);
        Cost oreCost = new Cost(ParseInt(match, 2), 0, 0);
        Cost clayCost = new Cost(ParseInt(match, 3), 0, 0);
        Cost obsidianCost = new Cost(ParseInt(match, 4), ParseInt(match, 5), 0);
        Cost geodeCost = new Cost(ParseInt(match, 6), 0, ParseInt(match, 7));

        return new Blueprint(id, oreCost, clayCost, obsidianCost, geodeCost);
    }

    private static int ParseInt(Match match, int index)
    {
        return int.Parse(match.Groups[index].Value);
    }

    public record Cost(int Ore, int Clay, int Obsidian)
    {
        public bool TrySpend(List<int> inventory)
        {
            if ((inventory[OreIndex] >= Ore)
                && (inventory[ClayIndex] >= Clay)
                && (inventory[ObsidianIndex] >= Obsidian))
            {
                inventory[OreIndex] -= Ore;
                inventory[ClayIndex] -= Clay;
                inventory[ObsidianIndex] -= Obsidian;

                return true;
            }

            return false;
        }

        public bool WillBeAccessible(IReadOnlyList<int> machineCounts, IReadOnlyList<int> inventory, int remainingTime)
        {
            return ((Ore == 0) || (inventory[OreIndex] + machineCounts[OreIndex] * remainingTime >= Ore))
                   && ((Clay == 0) || (inventory[ClayIndex] + machineCounts[ClayIndex] * remainingTime >= Clay))
                   && ((Obsidian == 0) || (inventory[ObsidianIndex] + machineCounts[ObsidianIndex] * remainingTime >= Obsidian));
        }
    }

    public record Blueprint(int ID, Cost OreCost, Cost ClayCost, Cost ObsidianCost, Cost GeodeCost)
    {
        public Cost[] MachineCosts => new[] {OreCost, ClayCost, ObsidianCost, GeodeCost};

        public int GetCollectedGeodeCount(int time)
        {
            int[] inventory = new int[4];
            int[] machineCounts = new int[4];

            machineCounts[OreIndex] = 1;

            List<string> actions = new();
            int geodeCount = TryBuyNextMachine(time, machineCounts, inventory, actions);
            Console.WriteLine($"{ID} -> Geodes {geodeCount}");
            return geodeCount;
        }

        private int TryBuyNextMachine(int time, IReadOnlyList<int> machineCounts, IReadOnlyList<int> inventory, List<string> actions)
        {
            int maxGeodeCount = 0;
            bool first = true;
            
            for (int nextMachineToBuyIndex = MachineCosts.Length - 1; nextMachineToBuyIndex >= 0; nextMachineToBuyIndex--)
            {
                if (!MachineCosts[nextMachineToBuyIndex].WillBeAccessible(machineCounts, inventory, time))
                {
                    continue;
                }
                
                if (!first)
                {
                    LogDebug($"Backtrack to {time}");
                }
                first = false;
                var currentInventory = new List<int>(inventory);
                var currentMachineCounts = new List<int>(machineCounts);

                int geodeCount =
                    GetCollectedGeodeCountWithNextMachine(nextMachineToBuyIndex, time, currentInventory, currentMachineCounts, new List<string>(actions));
                if (maxGeodeCount < geodeCount)
                {
                    maxGeodeCount = geodeCount;
                }
            }
            // foreach (var nextMachineToBuyIndex in EnumerateAccessibleMachines(machineCounts, inventory, time))
            // {
            //     if (!first)
            //     {
            //         LogDebug($"Backtrack to {time}");
            //     }
            //     first = false;
            //     var currentInventory = new List<int>(inventory);
            //     var currentMachineCounts = new List<int>(machineCounts);
            //
            //     int geodeCount =
            //         GetCollectedGeodeCountWithNextMachine(nextMachineToBuyIndex, time, currentInventory, currentMachineCounts, new List<string>(actions));
            //     if (maxGeodeCount < geodeCount)
            //     {
            //         maxGeodeCount = geodeCount;
            //     }
            // }

            return maxGeodeCount;
        }

        private int GetCollectedGeodeCountWithNextMachine(int nextMachineToBuyIndex, int time, List<int> inventory, List<int> machineCounts,
            List<string> actions)
        {
            Cost machineCost = MachineCosts[nextMachineToBuyIndex];
            while (time > 0)
            {
                // if (time == MachineCosts[GeodeIndex].Obsidian)
                // {
                //     if (inventory[ObsidianIndex] + machineCounts[ObsidianIndex] * time < MachineCosts[GeodeIndex].Obsidian)
                //     {
                //         LogDebug("Early break, won't have enough obsidian");
                //         int totalGeode = inventory[ObsidianIndex] + machineCounts[ObsidianIndex] * time;
                //         // Console.WriteLine("Early break, won't have enough obsidian => " + totalGeode);
                //         return totalGeode;
                //     }
                //
                // }
                bool machineBought = machineCost.TrySpend(inventory);
                ExtractResources(machineCounts, inventory);

                time--;

                if (machineBought && (time > 0))
                {
                    if (LogEnabled)
                    {
                        actions.Add($"Bought Machine {nextMachineToBuyIndex} at {time}");
                        actions.Add($"Resources at {time}: [{string.Join(",", inventory)}]");
                    }

                    machineCounts[nextMachineToBuyIndex]++;
                    return TryBuyNextMachine(time, machineCounts, inventory, actions);
                }

                if (LogEnabled)
                {
                    actions.Add($"Resources at {time}: [{string.Join(",", inventory)}]");
                }
            }

            if (LogEnabled)
            {
                LogDebug("-------------");
                LogDebug($"End path with {inventory[GeodeIndex]} geodes");
                LogDebug(string.Join("\n", actions));
            }

            return inventory[GeodeIndex];
        }

        private IEnumerable<int> EnumerateAccessibleMachines(IReadOnlyList<int> machineCount, IReadOnlyList<int> inventory, int remainingTime)
        {
            for (int i = MachineCosts.Length - 1; i >= 0; i--)
            {
                if (MachineCosts[i].WillBeAccessible(machineCount, inventory, remainingTime))
                {
                    yield return i;
                }
            }
        }

        private static void ExtractResources(List<int> machineCount, List<int> inventory)
        {
            for (int i = 0; i < machineCount.Count; i++)
            {
                inventory[i] += machineCount[i];
            }
        }

        // private static int BuyMachineIfPossible(int[] machineCount, Cost[] machineCosts, int[] inventory)
        // {
        //     for (int i = machineCount.Length - 1; i >= 0; i--)
        //     {
        //         if (machineCosts[i].TrySpend(inventory))
        //         {
        //             return i;
        //         }
        //     }
        //
        //     return -1;
        // }
    }

    public static bool LogEnabled = false;
    public static void LogDebug(string text)
    {
        if (LogEnabled)
        {
            Debug.WriteLine(text);
        }
    }
}
