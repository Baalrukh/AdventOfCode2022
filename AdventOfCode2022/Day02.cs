namespace AdventOfCode2022;

public class Day02 : Exercise
{
    public class RPSObject
    {
        public static readonly RPSObject Rock = new RPSObject(1);
        public static readonly RPSObject Paper  = new RPSObject(2);
        public static readonly RPSObject Scissors  = new RPSObject(3);

        public static readonly IReadOnlyList<RPSObject> All = new[] { Rock, Paper, Scissors };
        
        public RPSObject WinAgainst { get; private set; }
        public int Points { get; }

        static RPSObject()
        {
            Rock.WinAgainst = Scissors;
            Paper.WinAgainst = Rock;
            Scissors.WinAgainst = Paper;
        }

        public RPSObject(int points)
        {
            Points = points;
        }
    }

    private static Dictionary<int, RPSObject> _RPSObjectMapping = new Dictionary<int, RPSObject>
    {
        { 0, RPSObject.Rock },
        { 1, RPSObject.Paper },
        { 2, RPSObject.Scissors },
    };

    public long ExecutePart1(string[] lines)
    {
        return lines.GroupBy(x => x).Sum(g => GetPoints(GetMatchUp(g.Key)) * g.Count());
    }

    private int GetPoints((RPSObject opponentObject, RPSObject elfObject) matchUp)
    {
        return matchUp.elfObject.Points + GetMatchVictoryPoints(matchUp.opponentObject, matchUp.elfObject);
    }

    public long ExecutePart2(string[] lines)
    {
        var matchups = RPSObject.All.SelectMany(x => RPSObject.All.Select(y => (x, y)));
        var dictionary = matchups.ToDictionary(GetMatchupResultString, GetPoints);

        return lines.GroupBy(x => x).Sum(g => dictionary[g.Key] * g.Count());
    }

    public string GetMatchupResultString((RPSObject opponent, RPSObject elf) matchup)
    {
        return $"{ToChar(matchup.opponent)} {GetMatchupResultChar(matchup)}";
    }

    private char GetMatchupResultChar((RPSObject opponent, RPSObject elf) matchup)
    {
        if (matchup.opponent == matchup.elf)
        {
            return 'Y';
        }

        if (matchup.opponent.WinAgainst == matchup.elf)
        {
            return 'X';
        }

        return 'Z';
    }
    
    private char ToChar(RPSObject rpsObject)
    {
        if (rpsObject == RPSObject.Rock)
        {
            return 'A';
        }
        if (rpsObject == RPSObject.Paper)
        {
            return 'B';
        }

        return 'C';
    }

    private static (RPSObject opponentObject, RPSObject elfObject) GetMatchUp(string line)
    {
        return (_RPSObjectMapping[line[0] - 'A'], _RPSObjectMapping[line[2] - 'X']);
    }

    public int GetMatchVictoryPoints(RPSObject opponentObject, RPSObject elfObject)
    {
        if (opponentObject == elfObject)
        {
            return 3;
        }

        if (opponentObject.WinAgainst == elfObject)
        {
            return 0;
        }

        return 6;
    }
}
