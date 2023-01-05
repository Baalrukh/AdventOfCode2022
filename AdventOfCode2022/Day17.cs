using System.Collections;
using System.Text;
using AdventOfCode2022.Utils;

namespace AdventOfCode2022;

public class Day17 : Exercise
{
    public static readonly IReadOnlyList<IntVector2>[] PIECES = {
        new IntVector2[] { new(0, 0), new(1,  0), new(2,  0), new(3,  0) }, // H line
        new IntVector2[] { new(1, 0), new(1, -1), new(0, -1), new(2, -1), new(1, -2) }, // cross
        new IntVector2[] { new(2, 0), new(2, -1), new(2, -2), new(1, -2), new(0, -2)  }, // L
        new IntVector2[] { new(0, 0), new(0, -1), new(0, -2), new(0, -3) }, // V line
        new IntVector2[] { new(0, 0), new(0, -1), new(1,  0), new(1, -1) }, // square
    };

    public static readonly IReadOnlyList<IntVector2> PIECES_SIZE = new IntVector2[]
    {
        new(4, 1),
        new(3, 3),
        new(3, 3),
        new(1, 4),
        new(2, 2)
    };
    
    public long ExecutePart1(string[] lines)
    {
        const int rockCount = 2022;
        TetrisWorld world = new TetrisWorld(7);
        var jetsEnumerator = new LoopingEnumerator<char>(lines[0].ToList());
        for (int i = 0; i < rockCount; i++)
        {
            int pieceIndex = i % PIECES.Length;
            world.MakeBlockFall(PIECES[pieceIndex], PIECES_SIZE[pieceIndex], jetsEnumerator);
        }

        return world.MaxHeight;
    }

    public struct IterationDesc
    {
        public readonly int PieceIndex;
        public readonly int JetIndex;
        public readonly int LastLineHash;

        public IterationDesc(int pieceIndex, int jetIndex, int lastLineHash)
        {
            PieceIndex = pieceIndex;
            JetIndex = jetIndex;
            LastLineHash = lastLineHash;
        }

        public bool Equals(IterationDesc other)
        {
            return PieceIndex == other.PieceIndex && JetIndex == other.JetIndex && LastLineHash == other.LastLineHash;
        }

        public override bool Equals(object? obj)
        {
            return obj is IterationDesc other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = PieceIndex;
                hashCode = (hashCode * 397) ^ JetIndex;
                hashCode = (hashCode * 397) ^ LastLineHash;
                return hashCode;
            }
        }
    }

    // more elegant, but does not return the expected value
    public long ExecutePart2_NotWorking(string[] lines)
    {
        const long rockCount = 1_000_000_000_000;

        TetrisWorld world = new TetrisWorld(7);
        var jetsEnumerator = new CountingLoopingEnumerator<char>(lines[0].ToList());

        Dictionary<IterationDesc, long> rockCountByPattern = new Dictionary<IterationDesc, long>();
        Dictionary<IterationDesc, long> maxHeightByPattern = new Dictionary<IterationDesc, long>();

        long additionalHeight = 0;

        long i = 0;
        while (i < rockCount)
        {
            int pieceIndex = (int) (i % PIECES.Length);
            world.MakeBlockFall(PIECES[pieceIndex], PIECES_SIZE[pieceIndex], jetsEnumerator);

            var iterationDesc = new IterationDesc(pieceIndex, jetsEnumerator.Index, world.GetLastLineHash());
            if (rockCountByPattern.TryGetValue(iterationDesc, out var prevRockCount))
            {
                int asddas = 0;
                var prevHeight = maxHeightByPattern[iterationDesc];
                long cycleLength = i - prevRockCount;
                long patternHeight = world.MaxHeight - prevHeight;

                long cycleCount = (rockCount - i - 1) / cycleLength;
                i += cycleCount * cycleLength + 1;
                additionalHeight = patternHeight * cycleCount;
                break;
            }

            rockCountByPattern.Add(iterationDesc, i);
            maxHeightByPattern.Add(iterationDesc, world.MaxHeight);
            i++;
        }

        while (i < rockCount)
        {
            int pieceIndex = (int) (i % PIECES.Length);
            world.MakeBlockFall(PIECES[pieceIndex], PIECES_SIZE[pieceIndex], jetsEnumerator);
            i++;
        }
        return world.MaxHeight + additionalHeight - 1;
// 1551319648089
    }


    public long ExecutePart2(string[] lines)
    {
        const long rockCount = 1_000_000_000_000;
        const long patternDetectionRockCount = 50_000;
        TetrisWorld world = new TetrisWorld(7);
        var jetsEnumerator = new CountingLoopingEnumerator<char>(lines[0].ToList());

        for (long i = 0; i < patternDetectionRockCount; i++)
        {
            int pieceIndex = (int) (i % PIECES.Length);
            world.MakeBlockFall(PIECES[pieceIndex], PIECES_SIZE[pieceIndex], jetsEnumerator);

            if (jetsEnumerator.Index == jetsEnumerator.Count - 1)
            {
                int asd = 0;
            }
        }

        var map = world.GetBinaryMap();

        var (patternStart, patternLength) = FindPattern(map);

        // reset
        world = new TetrisWorld(7);
        jetsEnumerator = new CountingLoopingEnumerator<char>(lines[0].ToList());

        long rockCountForStart = -1;
        long heightForPatternStart = -1;
        long additionnalLineCount = -1;


        long r = 0;
        while (r < rockCount)
        {
            int pieceIndex = (int) (r % PIECES.Length);
            world.MakeBlockFall(PIECES[pieceIndex], PIECES_SIZE[pieceIndex], jetsEnumerator);

            if (world.MaxHeight == patternStart + patternLength)
            {
                rockCountForStart = r;
                heightForPatternStart = world.MaxHeight;
                r++;
                break;
            }

            r++;
        }

        long prevHeight = world.MaxHeight;
        long prevR = r;
        int cyclePiece = (int) ((r + 4) % PIECES.Length);
        int cycleCommand = jetsEnumerator.Index;

        while (r < rockCount)
        {
            int pieceIndex = (int) (r % PIECES.Length);
            world.MakeBlockFall(PIECES[pieceIndex], PIECES_SIZE[pieceIndex], jetsEnumerator);
            //
            if (((world.MaxHeight - patternStart) % patternLength == 0) && (world.MaxHeight != prevHeight))
            {
                int sdf = 0;
                long lineCount = world.MaxHeight - prevHeight;
                long rockDeltaCount = r - prevR;
                bool pieceOK = (int) (r % PIECES.Length) == cyclePiece;
                bool commandOK = jetsEnumerator.Index == cycleCommand;
                prevHeight = world.MaxHeight;
                prevR = r;
            }
            if (world.MaxHeight == patternStart + 2 * patternLength)
            {
                long rockCountForCycle = r - rockCountForStart;

                var cycleCount = (rockCount - r - 1) / rockCountForCycle;

                additionnalLineCount = cycleCount * patternLength;

                r += cycleCount * rockCountForCycle + 1;
                break;
            }

            r++;
        }

        while (r < rockCount)
        {
            int pieceIndex = (int) (r % PIECES.Length);
            world.MakeBlockFall(PIECES[pieceIndex], PIECES_SIZE[pieceIndex], jetsEnumerator);
            r++;
        }

        return world.MaxHeight + additionnalLineCount;
    }

    private (int start, int lenght) FindPattern_2(string map)
    {
        var lines = map.Split('\n').ToList();
        lines.RemoveRange(lines.Count - 200, 200);

        Dictionary<string, List<int>> repetitions = new Dictionary<string, List<int>>();

        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (!repetitions.TryGetValue(line, out var list))
            {
                list = new List<int>();
                repetitions.Add(line, list);
            }

            list.Add(i);
        }

        return (-1, -1);
    }

    private (int start, int lenght) FindPattern(List<int> lines)
    {
        lines.RemoveRange(lines.Count - 200, 200);

        for (int start = 0; start < lines.Count; start++)
        {
            var length = TryFindPattern(start, lines);
            if (length > 0)
            {
                return (start, length);
            }
        }

        return (-1, -1);
    }

    private int TryFindPattern(int start, List<int> lines)
    {
        int length = 10;
        while (length < (lines.Count - start) / 5)
        {
            if (lines[start + length] == lines[start])
            {
                if (TryPattern(start, length, lines))
                {
                    return length;
                }
            }

            length++;
        }

        return -1;
    }

    private bool TryPattern(int start, int patternLength, List<int> lines)
    {
        for (int i = 0; i < patternLength; i++)
        {
            int l = start + i + patternLength;
            while (l < lines.Count)
            {
                if (lines[l] != lines[start + i])
                {
                    return false;
                }

                l += patternLength;
            }
        }

        return true;
    }

    public class TetrisWorld
    {
        public readonly HashSet<IntVector2> Blocks = new();
        public int MaxHeight = 0;
        private readonly int _width;

        public TetrisWorld(int width)
        {
            _width = width;
        }

        public void MakeBlockFall(IReadOnlyList<IntVector2> piece, IntVector2 pieceSize, IEnumerator<char> jetsEnumerator)
        {
            IntVector2 position = GetSpawnPosition(pieceSize.Y);
            while (true)
            {
                jetsEnumerator.MoveNext();
                if (CanPush(position, piece, pieceSize.X, jetsEnumerator.Current, out int xOffset))
                {
                    position += new IntVector2(xOffset, 0);
                }

                if (!CanFall(position, piece, pieceSize.Y))
                {
                    break;
                }
                
                position += new IntVector2(0, -1);
            }

            if (MaxHeight < position.Y + 1)
            {
                MaxHeight = position.Y + 1;
            }

            foreach (IntVector2 block in piece)
            {
                Blocks.Add(block + position);
            }

            // PrintMap();
        }

        public List<int> GetBinaryMap()
        {
            int max = Blocks.Max(x => x.Y);
            int[] lines = new int[max + 1];

            foreach (IntVector2 block in Blocks)
            {
                lines[block.Y] |= 1 << block.X;
            }

            return new List<int>(lines);
        }

        public int GetLastLineHash()
        {
            int hash = 0;
            foreach (IntVector2 block in Blocks.Where(p => p.Y == MaxHeight - 1))
            {
                hash |= 1 << block.X;
            }

            return hash;
        }

        public string PrintMap()
        {
            int max = Blocks.Max(x => x.Y);
            StringBuilder builder = new StringBuilder((max + 1) * _width);
            for (int i = 0; i <= max; i++)
            {
                // builder.Append("|.......|\n");
                builder.Append("|.......|\n");
            }

            const int lineWidth = 7 + 3;
            foreach (IntVector2 block in Blocks)
            {
                builder[(max - block.Y) * lineWidth + block.X + 1] = '#';
            }
            
            return builder.ToString();
        }

        public IntVector2 GetSpawnPosition(int pieceHeight)
        {
            return new IntVector2(2, MaxHeight + 3 + pieceHeight - 1);
        }

        public bool CanPush(IntVector2 position, IReadOnlyList<IntVector2> piece, int pieceWidth, char direction, out int xOffset)
        {
            if (direction == '<')
            {
                if (position.X < 1)
                {
                    xOffset = 0;
                    return false;
                }

                xOffset = -1;
            }
            else
            {
                if (position.X + pieceWidth >= _width)
                {
                    xOffset = 0;
                    return false;
                }

                xOffset = 1;
            }

            return IsOverlappingBlocks(piece, new IntVector2(position.X + xOffset, position.Y));
        }

        
        public bool CanFall(IntVector2 position, IReadOnlyList<IntVector2> piece, int pieceHeight)
        {
            // if (position.Y - pieceHeight > MaxHeight - 1)
            // {
            //     return true;
            // }

            if (position.Y - pieceHeight < 0)
            {
                return false;
            }
            
            return IsOverlappingBlocks(piece, new IntVector2(position.X, position.Y - 1));
        }

        private bool IsOverlappingBlocks(IReadOnlyList<IntVector2> piece, IntVector2 pos)
        {
            return piece.Select(x => x + pos).All(x => !Blocks.Contains(x));
        }

    }

    public class CountingLoopingEnumerator<T> : IEnumerator<T>
    {
        private readonly IReadOnlyList<T> _list;
        public int Index { get; private set; } = -1;

        public CountingLoopingEnumerator(IReadOnlyList<T> list)
        {
            _list = list;
        }

        public bool MoveNext()
        {
            Index = (Index + 1) % _list.Count;
            return true;
        }

        public void Reset()
        {
            Index = -1;
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public T Current => _list[Index];

        public int Count => _list.Count;
    }
}