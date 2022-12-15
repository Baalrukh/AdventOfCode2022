 using AdventOfCode2022.Utils;

 namespace AdventOfCode2022.Test;

[TestFixture]
public class Day15Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "Sensor at x=2, y=18: closest beacon is at x=-2, y=15",
        "Sensor at x=9, y=16: closest beacon is at x=10, y=16",
        "Sensor at x=13, y=2: closest beacon is at x=15, y=3",
        "Sensor at x=12, y=14: closest beacon is at x=10, y=16",
        "Sensor at x=10, y=20: closest beacon is at x=10, y=16",
        "Sensor at x=14, y=17: closest beacon is at x=10, y=16",
        "Sensor at x=8, y=7: closest beacon is at x=2, y=10",
        "Sensor at x=2, y=0: closest beacon is at x=2, y=10",
        "Sensor at x=0, y=11: closest beacon is at x=2, y=10",
        "Sensor at x=20, y=14: closest beacon is at x=25, y=17",
        "Sensor at x=17, y=20: closest beacon is at x=21, y=22",
        "Sensor at x=16, y=7: closest beacon is at x=15, y=3",
        "Sensor at x=14, y=3: closest beacon is at x=15, y=3",
        "Sensor at x=20, y=1: closest beacon is at x=15, y=3"
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(26, Day15.GetNumberOfNoBeaconPositionAtY(_sampleLines, 10));
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(56000011, Day15.GetTuningFrequency(_sampleLines, 20));
    }

    [Test]
    public void TestFindBeaconPosition()
    {
        Assert.AreEqual((14, 11), Day15.FindBeaconPosition(_sampleLines, 20));
    }

    [Test]
    public void TestParseSensor()
    {
        Assert.AreEqual(new Day15.Sensor(new (2, 18), new (-2, 15)), Day15.ParseSensor(_sampleLines[0]));
    }

    [Test]
    public void TestSensorGetNoBeaconRangeAtY_outOfRange()
    {
        var sensor = new Day15.Sensor(new (10, 15), new (2, 3));
        Assert.IsTrue(sensor.GetNoBeaconRangeAtY(100).IsEmpty);
    }

    [Test]
    public void TestSensorGetNoBeaconRangeAtY_std()
    {
        var sensor = new Day15.Sensor(new (8, 7), new (2, 10));
        Assert.AreEqual(Interval.FromToIncluded(4, 12), sensor.GetNoBeaconRangeAtY(12));
    }

    [Test]
    public void TestSensorGetNoBeaconRangeAtY_atBeaconY()
    {
        var sensor = new Day15.Sensor(new (8, 7), new (2, 10));
        Assert.AreEqual(Interval.FromToIncluded(3, 14), sensor.GetNoBeaconRangeAtY(10));
    }
}
