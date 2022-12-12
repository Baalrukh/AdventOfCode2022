using AdventOfCode2022.Utils;

namespace AdventOfCode2022;

public class Day12 : Exercise
{
    private static readonly IntVector2 _Up = new IntVector2(0, -1);
    private static readonly IntVector2 _Right = new IntVector2(1, 0);
    private static readonly IntVector2 _Down = new IntVector2(0, 1);
    private static readonly IntVector2 _Left = new IntVector2(-1, 0);
    private static readonly IntVector2[] _Directions = new[] { _Up, _Right, _Down, _Left };

    public long ExecutePart1(string[] lines)
    {
        var map = ParseMap(lines, out var startPosition, out var endPosition, 99);
        map[startPosition].BestTravelCost = 0;
        return FindShortestStepsFromStartToFinish(map, startPosition, endPosition);
    }

    public long ExecutePart2(string[] lines)
    {
        var map = ParseMap(lines, out _, out var endPosition, -10);
        map[endPosition].BestTravelCost = 0;
        return FindStepCountToFirstLowestPoint(map, endPosition);
    }

    private static Map2D<Cell> ParseMap(string[] lines, out IntVector2 startPosition, out IntVector2 endPosition, int borderHeight)
    {
        Map2D<Cell> map = Map2D<Cell>.Parse(lines, x => new Cell(x, int.MaxValue), () => new Cell(borderHeight, -1));

        startPosition = new();
        endPosition = new();
        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                var cell = map[x, y];
                if (cell.Height == 'S')
                {
                    map[x, y] = new Cell('a', int.MaxValue);
                    startPosition = new IntVector2(x, y);
                }
                else if (cell.Height == 'E')
                {
                    map[x, y] = new Cell('z', int.MaxValue);
                    endPosition = new IntVector2(x, y);
                }
            }
        }

        return map;
    }


    private static long FindShortestStepsFromStartToFinish(Map2D<Cell> map, IntVector2 startPosition, IntVector2 endPosition) {
        Queue<IntVector2> positionsToProcess = new Queue<IntVector2>();
        positionsToProcess.Enqueue(startPosition);
        HashSet<IntVector2> processedPoints = new HashSet<IntVector2>();

        while (positionsToProcess.Count > 0) {
            IntVector2 position = positionsToProcess.Dequeue();
            var currentCell = map[position];
            int currentCost = currentCell.BestTravelCost;

            foreach (IntVector2 direction in _Directions) {
                IntVector2 nextPosition = position + direction;
                Cell nextCell = map[nextPosition];
                if (nextCell.Height > currentCell.Height + 1)
                {
                    continue;
                }

                int cost = 1 + currentCost;
                if (cost >= nextCell.BestTravelCost)
                {
                    continue;
                }

                nextCell.BestTravelCost = cost;
                positionsToProcess.Enqueue(nextPosition);
                processedPoints.Remove(nextPosition);
            }
        }

        return map[endPosition].BestTravelCost;
    }

    private static long FindStepCountToFirstLowestPoint(Map2D<Cell> map, IntVector2 endPosition) {
        Queue<IntVector2> positionsToProcess = new Queue<IntVector2>();
        positionsToProcess.Enqueue(endPosition);
        HashSet<IntVector2> processedPoints = new HashSet<IntVector2>();

        while (positionsToProcess.Count > 0) {
            IntVector2 position = positionsToProcess.Dequeue();
            var currentCell = map[position];
            int currentCost = currentCell.BestTravelCost;

            foreach (IntVector2 direction in _Directions) {
                IntVector2 nextPosition = position + direction;
                Cell nextCell = map[nextPosition];
                if (nextCell.Height < currentCell.Height - 1)
                {
                    continue;
                }

                int cost = 1 + currentCost;
                if (cost >= nextCell.BestTravelCost)
                {
                    continue;
                }

                nextCell.BestTravelCost = cost;
                positionsToProcess.Enqueue(nextPosition);
                processedPoints.Remove(nextPosition);
                if (nextCell.Height == 'a')
                {
                    return nextCell.BestTravelCost;
                }
            }
        }

        return map[endPosition].BestTravelCost;
    }

    public class Cell
    {
        public readonly int Height;
        public int BestTravelCost;

        public Cell(int height, int bestTravelCost) {
            Height = height;
            BestTravelCost = bestTravelCost;
        }
    }
}
