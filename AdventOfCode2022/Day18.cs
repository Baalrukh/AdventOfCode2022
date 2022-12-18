using AdventOfCode2022.Utils;

namespace AdventOfCode2022;

public class Day18 : Exercise
{
    private static IntVector3[] _directions = new IntVector3[]
    {
        new(-1, 0, 0),
        new(1, 0, 0),
        new(0, -1, 0),
        new(0, 1, 0),
        new(0, 0, -1),
        new(0, 0, 1),
    };
    
    public long ExecutePart1(string[] lines)
    {
        var blocks = lines.Select(ParseVector3).ToHashSet();

        return blocks.Sum(x => CountNumberOfFreeFaces(x, blocks));
    }

    private int CountNumberOfFreeFaces(IntVector3 position, IReadOnlySet<IntVector3> blocks)
    {
        return _directions.Count(x => !blocks.Contains(position + x));
    }

    private IntVector3 ParseVector3(string text)
    {
        string[] tokens = text.Split(',');
        return new IntVector3(int.Parse(tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2]));
    }
    

    public long ExecutePart2(string[] lines)
    {
        var blocks = lines.Select(ParseVector3).ToHashSet();
        int maxX = blocks.Max(x => x.X);
        int maxY = blocks.Max(x => x.Y);
        int maxZ = blocks.Max(x => x.Z);

        HashSet<IntVector3> externalAir = new();

        Queue<IntVector3> queue = new();
        queue.Enqueue(new IntVector3(0, 0, 0));
        externalAir.Add(new IntVector3(0, 0, 0));
        
        while (queue.Count > 0)
        {
            IntVector3 pos = queue.Dequeue();
            externalAir.Add(pos);
            var nextPositions = _directions.Select(x => pos + x)
                .Where(p => IsInsideSampleVolume(p, maxX, maxY, maxZ))
                .Where(p => !blocks.Contains(p))
                .Where(p => !externalAir.Contains(p));

            foreach (IntVector3 nextPos in nextPositions)
            {
                externalAir.Add(nextPos);
                queue.Enqueue(nextPos);
            } 
        }

        // var insidePositions = EnumerateAllVolumePositions(maxX, maxY, maxZ).Where(p => !blocks.Contains(p) && !externalAir.Contains(p)).ToList();

        return blocks.Sum(x => CountNumberOfFacesExposedToExternalAir(x, blocks, externalAir, maxX, maxY, maxZ));
    }
    //
    // private IEnumerable<IntVector3> EnumerateAllVolumePositions(int maxX, int maxY, int maxZ)
    // {
    //     for (int x = 0; x < maxX + 1; x++)
    //     {
    //         for (int y = 0; y < maxY + 1; y++)
    //         {
    //             for (int z = 0; z < maxZ + 1; z++)
    //             {
    //                 yield return new IntVector3(x, y, z);
    //             }
    //         }
    //     }
    // }

    private bool IsInsideSampleVolume(IntVector3 position, int maxX, int maxY, int maxZ)
    {
        return ((position.X >= 0) && (position.X <= maxX))
               && ((position.Y >= 0) && (position.Y <= maxY))
               && ((position.Z >= 0) && (position.Z <= maxZ));
    }

    private int CountNumberOfFacesExposedToExternalAir(IntVector3 position, IReadOnlySet<IntVector3> blocks, IReadOnlySet<IntVector3> externalAir, int maxX, int maxY, int maxZ)
    {
        return _directions.Select(x => position + x)
            .Count(x => !blocks.Contains(x) && IsInExternalAir(externalAir, maxX, maxY, maxZ, x));
    }

    private bool IsInExternalAir(IReadOnlySet<IntVector3> externalAir, int maxX, int maxY, int maxZ, IntVector3 x)
    {
        return !IsInsideSampleVolume(x, maxX, maxY, maxZ) || externalAir.Contains(x);
    }
}
