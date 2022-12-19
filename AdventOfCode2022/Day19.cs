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
        return -1;
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
        public bool TrySpend(int[] inventory)
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
    }

    public record Blueprint(int ID, Cost OreCost, Cost ClayCost, Cost ObsidianCost, Cost GeodeCost)
    {
        public Cost[] MachineCosts => new[] {OreCost, ClayCost, ObsidianCost, GeodeCost};

        public int GetCollectedGeodeCount(int time)
        {
            int[] inventory = new int[4];
            int[] machineCount = new int[4];

            machineCount[OreIndex] = 1;

            var machineCosts = MachineCosts;
            for (int i = 1; i < time + 1; i++)
            {
                var boughtType = BuyMachineIfPossible(machineCount, machineCosts, inventory);

                ExtractResources(machineCount, inventory);

                if (boughtType != -1)
                {
                    machineCount[boughtType]++;
                }
            }

            return inventory[GeodeIndex];
        }

        private static void ExtractResources(int[] machineCount, int[] inventory)
        {
            for (int i = 0; i < machineCount.Length; i++)
            {
                inventory[i] += machineCount[i];
            }
        }

        private static int BuyMachineIfPossible(int[] machineCount, Cost[] machineCosts, int[] inventory)
        {
            for (int i = machineCount.Length - 1; i >= 0; i--)
            {
                if (machineCosts[i].TrySpend(inventory))
                {
                    return i;
                }
            }

            return -1;
        }
    }

}
