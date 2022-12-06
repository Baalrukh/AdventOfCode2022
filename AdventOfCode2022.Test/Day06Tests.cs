namespace AdventOfCode2022.Test;

[TestFixture]
public class Day06Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        ""
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(7, new Day06().ExecutePart1(new [] {"mjqjpqmgbljsphdztnvjfqwrcgsmlb"}));
        Assert.AreEqual(5, new Day06().ExecutePart1(new [] {"bvwbjplbgvbhsrlpgdmjqwftvncz"}));
        Assert.AreEqual(6, new Day06().ExecutePart1(new [] {"nppdvjthqldpwncqszvftbrmjlhg"}));
        Assert.AreEqual(10, new Day06().ExecutePart1(new [] {"nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg"}));
        Assert.AreEqual(11, new Day06().ExecutePart1(new [] {"zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw"}));
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(19, new Day06().ExecutePart2(new [] {"mjqjpqmgbljsphdztnvjfqwrcgsmlb"}));
        Assert.AreEqual(23, new Day06().ExecutePart2(new [] {"bvwbjplbgvbhsrlpgdmjqwftvncz"}));
        Assert.AreEqual(23, new Day06().ExecutePart2(new [] {"nppdvjthqldpwncqszvftbrmjlhg"}));
        Assert.AreEqual(29, new Day06().ExecutePart2(new [] {"nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg"}));
        Assert.AreEqual(26, new Day06().ExecutePart2(new [] {"zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw"}));
    }
}
