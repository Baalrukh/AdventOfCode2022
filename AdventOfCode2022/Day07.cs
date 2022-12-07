using AdventOfCode2022.Utils;

namespace AdventOfCode2022;

public class Day07 : Exercise
{
    public long ExecutePart1(string[] lines)
    {
        Directory root = new Directory("/", null);
        SplitByCommand(lines).Select(ParseCommand).Aggregate(root, (dir, command) => command.Execute(dir));

        const long maxSize = 100_000;
        return root.EnumerateAllDirs().Where(x => x.RecursiveSize <= maxSize).Sum(x => x.RecursiveSize);
    }

    public long ExecutePart2(string[] lines)
    {
        Directory root = new Directory("/", null);
        SplitByCommand(lines).Select(ParseCommand).Aggregate(root, (dir, command) => command.Execute(dir));

        const long totalDeviceSize = 70_000_000;
        const long requiredSize = 30_000_000;

        long freeSpace = totalDeviceSize - root.RecursiveSize;
        long minSpaceToFree = requiredSize - freeSpace;

        return root.EnumerateAllDirs().OrderBy(x => x.RecursiveSize).First(x => x.RecursiveSize > minSpaceToFree).RecursiveSize;
    }

    public static IEnumerable<IReadOnlyList<string>> SplitByCommand(IEnumerable<string> lines)
    {
        return lines.Batch(x => x[0] == '$');
    }


    public record File
    {
        public readonly string Name;
        public readonly long Size;
        public Directory Parent { get; set; }

        public File(string name, long size, Directory parent)
        {
            Name = name;
            Size = size;
            Parent = parent;
        }
    }

    public class Directory
    {
        public readonly string Name;
        public Directory? Parent { get; set; }
        public List<Directory> Directories { get; } = new();
        public List<File> Files { get; } = new();
        private long? _recursiveSizeCache;

        public string Path
        {
            get
            {
                if (Parent != null)
                {
                    return $"{Parent.Path}/{Name}";
                }

                return Name;
            }
        }

        public long RecursiveSize
        {
            get
            {
                if (_recursiveSizeCache == null)
                {
                    _recursiveSizeCache = Files.Sum(x => x.Size) + Directories.Sum(x => x.RecursiveSize);
                }

                return _recursiveSizeCache.Value;
            }
        }

        public Directory(string name, Directory? parent)
        {
            Name = name;
            Parent = parent;
        }

        protected bool Equals(Directory other)
        {
            return Name == other.Name && Equals(Parent, other.Parent);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Directory) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name.GetHashCode() * 397) ^ (Parent != null ? Parent.GetHashCode() : 0);
            }
        }

        public IEnumerable<Directory> EnumerateAllDirs()
        {
            yield return this;
            foreach (var directory in Directories)
            {
                foreach (var dir in directory.EnumerateAllDirs())
                {
                    yield return dir;
                }
            }
        }
    }

    public interface FileSystemCommand
    {
        Directory Execute(Directory activeDirectory);
    }

    public record LsCommand(IReadOnlyList<string> Results) : FileSystemCommand
    {
        public virtual bool Equals(LsCommand? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Results.SequenceEqual(other.Results);
        }

        public override int GetHashCode()
        {
            return Results.GetHashCode();
        }

        public Directory Execute(Directory activeDirectory)
        {
            foreach (var result in Results)
            {
                var tokens = result.Split(' ');
                switch (tokens[0])
                {
                    case "dir":
                        activeDirectory.Directories.Add(new Directory(tokens[1], activeDirectory));
                        break;
                    default:
                        activeDirectory.Files.Add(new File(tokens[1], long.Parse(tokens[0]), activeDirectory));
                        break;
                }
            }

            return activeDirectory;
        }
    }

    public record CdCommand(string Path) : FileSystemCommand
    {
        public Directory Execute(Directory activeDirectory)
        {
            switch (Path)
            {
                case "..":
                    if (activeDirectory.Parent != null)
                    {
                        return activeDirectory.Parent;
                    }

                    return activeDirectory;
                case "/":
                    while (activeDirectory.Parent != null)
                    {
                        activeDirectory = activeDirectory.Parent;
                    }

                    return activeDirectory;
                default:
                    return activeDirectory.Directories.FirstOrDefault(x => x.Name == Path) ?? activeDirectory;
            }

        }
    }

    public static FileSystemCommand ParseCommand(IReadOnlyList<string> lines)
    {
        var tokens = lines[0].Split(' ');
        switch (tokens[1])
        {
            case "ls":
                return new LsCommand(lines.Skip(1).ToList());
            case "cd":
                return new CdCommand(tokens[2]);
            default:
                throw new Exception("Unsupported command : " + tokens[1]);
        }
    }
}
