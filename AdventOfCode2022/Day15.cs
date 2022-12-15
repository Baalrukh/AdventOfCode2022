using System.Numerics;
using System.Text.RegularExpressions;
using AdventOfCode2022.Utils;

namespace AdventOfCode2022;

public class Day15 : Exercise
{
    private static readonly Regex SensorPattern = new Regex("Sensor at x=(\\d+), y=(\\d+): closest beacon is at x=(-?\\d+), y=(-?\\d+)");

    public long ExecutePart1(string[] lines)
    {
        var testY = 2_000_000;
        return GetNumberOfNoBeaconPositionAtY(lines, testY);
    }

    public static long GetNumberOfNoBeaconPositionAtY(string[] lines, int testY)
    {
        var intervalGroup = lines.Select(ParseSensor)
            .Select(x => x.GetNoBeaconRangeAtY(testY))
            .Aggregate(new IntervalGroup(), (group, interval) => group.Add(interval));
        return intervalGroup.LengthSum;
    }

    public long ExecutePart2(string[] lines)
    {
        return GetTuningFrequency(lines, 4_000_000);
    }

    public static long GetTuningFrequency(string[] lines, int maxCoordinate)
    {
        var (x, y) = FindBeaconPosition(lines, maxCoordinate);
        var tuningFrequency = new BigInteger(x) * new BigInteger(4_000_000) + new BigInteger(y);
        Console.WriteLine(tuningFrequency);
        return x * 4_000_000 + y; // will overflow
    }

    public static (int x, int y) FindBeaconPosition(string[] lines, int maxCoordinate)
    {
        var sensors = lines.Select(ParseSensor).ToList();
        for (int y = 0; y < maxCoordinate; y++)
        {
            var wholeLine = new IntervalGroup().Add(Interval.FromToIncluded(0, maxCoordinate));
            var possibleIntervals = sensors.Aggregate(wholeLine, (group, sensor) => group.Remove(sensor.GetNoBeaconRangeAtY(y, false)));
            if (possibleIntervals.LengthSum != 0)
            {
                return (possibleIntervals.Intervals[0].Start, y);
            }
        }

        return (-1, -1);
    }

    public record Sensor(IntVector2 Position, IntVector2 ClosestBeaconPosition)
    {
        public int Range => (ClosestBeaconPosition - Position).ManhattanDistance;

        public Interval GetNoBeaconRangeAtY(int y, bool excludeBeacons = true)
        {
            var range = Range;
            var deltaY = Math.Abs(y - Position.Y);
            if (deltaY > range)
            {
                return Interval.Empty;
            }

            int width = Range - deltaY;
            var minX = Position.X - width;
            var maxX = Position.X + width;
            if ((y == ClosestBeaconPosition.Y) && excludeBeacons)
            {
                if (ClosestBeaconPosition.X < Position.X)
                {
                    minX++;
                }
                else
                {
                    maxX--;
                }
            }

            return Interval.FromToIncluded(minX, maxX);

        }

    }

    public static Sensor ParseSensor(string line)
    {
        var match = SensorPattern.Match(line);
        var position = new IntVector2(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
        var beaconOffset = new IntVector2(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
        return new Sensor(position, beaconOffset);
    }
}
