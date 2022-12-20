using System.Collections;
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

    public long ExecutePart1(string[] lines)
    {
        var list = lines.Select(ParseBlueprint).Select(x => x.GetCollectedGeodeCount(24)).ToList();

        return list.Select((x, i) => x * i).Sum();
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

        public bool WillBeAccessible(IReadOnlyList<int> machineCounts)
        {
            return ((Ore == 0) || (machineCounts[OreIndex] > 0))
                   && ((Clay == 0) || (machineCounts[ClayIndex] > 0))
                   && ((Obsidian == 0) || (machineCounts[ObsidianIndex] > 0));
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

            return TryBuyNextMachine(time, machineCounts, inventory);
        }

        private int TryBuyNextMachine(int time, IReadOnlyList<int> machineCounts, IReadOnlyList<int> inventory)
        {
            int maxGeodeCount = 0;
            foreach (var nextMachineToBuyIndex in EnumerateAccessibleMachines(machineCounts))
            {
                var currentInventory = new List<int>(inventory);
                var currentMachineCounts = new List<int>(machineCounts);

                int geodeCount =
                    GetCollectedGeodeCountWithNextMachine(nextMachineToBuyIndex, time, currentInventory, currentMachineCounts);
                if (maxGeodeCount < geodeCount)
                {
                    maxGeodeCount = geodeCount;
                }
            }

            return maxGeodeCount;
        }

        private int GetCollectedGeodeCountWithNextMachine(int nextMachineToBuyIndex, int time, List<int> inventory, List<int> machineCounts)
        {
            Cost machineCost = MachineCosts[nextMachineToBuyIndex];
            while (time > 0)
            {
                bool machineBought = machineCost.TrySpend(inventory);
                ExtractResources(machineCounts, inventory);

                time--;
                
                if (machineBought && (time > 0))
                {
                    machineCounts[nextMachineToBuyIndex]++;
                    return TryBuyNextMachine(time, machineCounts, inventory);
                }
            }

            return inventory[GeodeIndex];
        }

        private IEnumerable<int> EnumerateAccessibleMachines(IReadOnlyList<int> machineCount)
        {
            for (int i = MachineCosts.Length - 1; i >= 0; i--)
            {
                if (MachineCosts[i].WillBeAccessible(machineCount))
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

}
