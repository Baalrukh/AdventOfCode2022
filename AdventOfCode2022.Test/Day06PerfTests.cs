using System.Diagnostics;
using System.Text;

namespace AdventOfCode2022.Test;

[TestFixture]
public class Day06PerfTests
{
    private delegate long FindNonRepeatingBlock(string line, int patternLength);

    private FindNonRepeatingBlock[] _testMethods = new []
    {
        (FindNonRepeatingBlock)Day06Perf.FindNonRepeatingBlockDoubleFor,
        Day06Perf.FindNonRepeatingBlockLinq_Range,
        Day06Perf.FindNonRepeatingBlockLinq_SubString,
        // Day06Perf.FindNonRepeatingBlockLinq_Skip, // disabled, far too slow
        Day06Perf.FindNonRepeatingBlockHashSet,
        Day06Perf.FindNonRepeatingBlockHashSet_ReUse,
        Day06Perf.FindNonRepeatingBlock_MySolution,
        Day06Perf.FindNonRepeatingBlockOpti_MySolution_NoSkip,
    };

    [Test]
    public void TestSimpleCases()
    {
        foreach (var testMethod in _testMethods)
        {
            try
            {
                TestSimpleCaseWithTestMethod(testMethod);
            }
            catch (Exception e)
            {
                throw new Exception(testMethod.Method.Name, e);
            }
        }
    }

    private void TestSimpleCaseWithTestMethod(FindNonRepeatingBlock testMethod)
    {
        Assert.AreEqual(7, testMethod("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 4));
        Assert.AreEqual(5, testMethod("bvwbjplbgvbhsrlpgdmjqwftvncz", 4));
        Assert.AreEqual(6, testMethod("nppdvjthqldpwncqszvftbrmjlhg", 4));
        Assert.AreEqual(10, testMethod("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 4));
        Assert.AreEqual(11, testMethod("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 4));
    }


    [Test]
    public void TestAllLongAlwaysRepeating()
    {
        const int patternLength = 26;
        const int testStringLength = 100_000;
        var textPattern = GenerateTestString(patternLength, testStringLength);

        foreach (var testMethod in _testMethods)
        {
            Assert.AreEqual(-1, testMethod(textPattern, patternLength));
        }
    }


    [Test]
    public void BenchLongTextAlwaysRepeating()
    {
        const int patternLength = 26;
        const int testStringLength = 100_000;
        const int repeatCount = 10;
        var textPattern = GenerateTestString(patternLength, testStringLength);

        foreach (var testMethod in _testMethods)
        {
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < repeatCount; i++)
            {
                testMethod(textPattern, patternLength);
            }

            var durationMs = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"{testMethod.Method.Name}: {durationMs}ms");
        }
        Assert.AreEqual(-1, Day06Perf.FindNonRepeatingBlock_MySolution(textPattern, patternLength));
    }

    private static string GenerateTestString(int patternLength, int testStringLength)
    {
        var textPattern = Enumerable.Range(0, patternLength - 1).Aggregate("", (s, i) => s + (char) (i + 'a'));
        StringBuilder builder = new StringBuilder();
        while (builder.Length < testStringLength)
        {
            builder.Append(textPattern);
        }

        return builder.ToString();
    }

}