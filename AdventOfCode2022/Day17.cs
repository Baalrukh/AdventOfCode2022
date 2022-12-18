using System.Diagnostics;
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

    public long ExecutePart2(string[] lines)
    {
        return -2;
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

        private void PrintMap()
        {
            int max = Blocks.Max(x => x.Y);
            StringBuilder builder = new StringBuilder((max + 1) * _width);
            for (int i = 0; i <= max; i++)
            {
                builder.Append("|.......|\n");
            }

            const int lineWidth = 7 + 3;
            foreach (IntVector2 block in Blocks)
            {
                builder[(max - block.Y) * lineWidth + block.X + 1] = '#';
            }
            
            Debug.WriteLine("-----------------");
            Debug.WriteLine("");
            Debug.WriteLine(builder);
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
}