namespace AdventOfCode2022.Test;

[TestFixture]
public class Day07Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "$ cd /",
        "$ ls",
        "dir a",
        "14848514 b.txt",
        "8504156 c.dat",
        "dir d",
        "$ cd a",
        "$ ls",
        "dir e",
        "29116 f",
        "2557 g",
        "62596 h.lst",
        "$ cd e",
        "$ ls",
        "584 i",
        "$ cd ..",
        "$ cd ..",
        "$ cd d",
        "$ ls",
        "4060174 j",
        "8033020 d.log",
        "5626152 d.ext",
        "7214296 k"
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(95437, new Day07().ExecutePart1(_sampleLines));
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(24933642, new Day07().ExecutePart2(_sampleLines));
    }

    [Test]
    public void TestSplitByCommand()
    {
        CollectionAssert.AreEqual(new[]
        {
            new []{ "$ cd /"},
            new []{"$ ls",
                "dir a",
                "14848514 b.txt",
                "8504156 c.dat",
                "dir d"},
                new []{"$ cd a"}
        }, Day07.SplitByCommand(_sampleLines.Take(7)));
    }

    [Test]
    public void TestParseCommand_Ls()
    {
        var command = Day07.ParseCommand(new[]
        {
            "$ ls",
            "dir a",
            "14848514 b.txt",
            "8504156 c.dat",
            "dir d"
        });
        Assert.AreEqual(new Day07.LsCommand(new List<string>{"dir a",
            "14848514 b.txt",
            "8504156 c.dat",
            "dir d"}), command);
    }

    [Test]
    public void TestParseCommand_Cd()
    {
        var command = Day07.ParseCommand(new[]
        {
            "$ cd .."
        });
        Assert.AreEqual(new Day07.CdCommand(".."), command);
    }

    [Test]
    public void TestExecuteCDCommandParent()
    {
        var root = new Day07.Directory("/", null);
        var child = new Day07.Directory("Child", root);
        Assert.AreEqual(root, new Day07.CdCommand("..").Execute(child));
    }

    [Test]
    public void TestExecuteCDCommandParentWhenAtRoot()
    {
        var root = new Day07.Directory("/", null);
        Assert.AreEqual(root, new Day07.CdCommand("..").Execute(root));
    }

    [Test]
    public void TestExecuteCDCommandRoot()
    {
        var root = new Day07.Directory("/", null);
        var child = new Day07.Directory("Child", root);
        var child2 = new Day07.Directory("Child", child);
        Assert.AreEqual(root, new Day07.CdCommand("/").Execute(child2));
    }

    [Test]
    public void TestExecuteCDCommandChild()
    {
        var root = new Day07.Directory("/", null);
        var child = new Day07.Directory("Child", root);
        var child2 = new Day07.Directory("Child2", root);
        root.Directories.Add(child);
        root.Directories.Add(child2);
        Assert.AreEqual(child2, new Day07.CdCommand("Child2").Execute(root));
    }

    [Test]
    public void TestExecuteCDCommandChildInvalidName()
    {
        var root = new Day07.Directory("/", null);
        Assert.AreEqual(root, new Day07.CdCommand("Child2").Execute(root));
    }

    [Test]
    public void TestExecuteLsCommandCreatesDirectories()
    {
        var root = new Day07.Directory("/", null);
        var command = new Day07.LsCommand(new[] {"dir Child", "dir Child2"});
        Assert.AreEqual(root, command.Execute(root));
        Assert.AreEqual(new [] {new Day07.Directory("Child", root), new Day07.Directory("Child2", root)}, root.Directories);
    }

    [Test]
    public void TestExecuteLsCommandCreatesFiles()
    {
        var root = new Day07.Directory("/", null);
        var command = new Day07.LsCommand(new[] {"29116 f", "2557 g"});
        Assert.AreEqual(root, command.Execute(root));
        Assert.AreEqual(new [] {new Day07.File("f", 29116, root), new Day07.File("g", 2557, root)}, root.Files);
    }
}
