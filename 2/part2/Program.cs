using System.Text.RegularExpressions;

namespace part1;

class Game
{
    public int id { get; set; }

    public int redMax { get; set; }
    public int blueMax { get; set; }
    public int greenMax { get; set; }

    public List<Round> rounds { get; set; }

    public Game(int id, List<Round> rounds, int redMax, int blueMax, int greenMax)
    {
        this.id = id;
        this.rounds = rounds;
        this.redMax = redMax;
        this.blueMax = blueMax;
        this.greenMax = greenMax;
    }

    public void Print()
    {
        Console.WriteLine($"{id} red {redMax} green {greenMax} blue {blueMax}");
    }
}

class Round
{
    public int Red { get; set; }
    public int Blue { get; set; }
    public int Green { get; set; }

    public Round(int red, int blue, int green)
    {
        this.Red = red;
        this.Blue = blue;
        this.Green = green;
    }
}

class Program
{
    static void Main(string[] args)
    {
        var games = ParseGames();
        foreach (var game in games)
        {
            game.Print();
        }
        var sum = SumPowers(games);
        Console.WriteLine(sum);
    }

    static List<Game> ParseGames()
    {
        var games = new List<Game>();
        string? line;
        while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
        {
            games.Add(ParseGame(line));
        }

        return games;
    }

    static Game ParseGame(string line)
    {
        Regex gameIdMatcher = new Regex(@"Game (\d+):");
        Match match = gameIdMatcher.Match(line);

        if (!match.Success)
        {
            throw new InvalidDataException();
        }

        int gameId = int.Parse(match.Groups[1].Value);

        var roundStrings = line.Split(":")[1].Split(";");
        var trimmedRoundStrings = roundStrings.Select((r) => r.Trim()).ToList();

        var rounds = new List<Round>();
        foreach (var round in trimmedRoundStrings)
        {
            rounds.Add(ParseRound(round));
        }

        var redMax = rounds.Aggregate(0, (max, r) => Math.Max(max, r.Red));
        var blueMax = rounds.Aggregate(0, (max, r) => Math.Max(max, r.Blue));
        var greenMax = rounds.Aggregate(0, (max, r) => Math.Max(max, r.Green));

        return new Game(gameId, rounds, redMax, blueMax, greenMax);
    }

    static Round ParseRound(string roundStr)
    {
        Regex colorMatcher = new Regex(@"(\d+) (red|blue|green)");

        var matches = colorMatcher.Matches(roundStr);

        int red = 0, blue = 0, green = 0;

        foreach (Match match in matches)
        {
            int value = int.Parse(match.Groups[1].Value);
            string color = match.Groups[2].Value;

            switch (color)
            {
                case "red":
                    red = value;
                    break;
                case "blue":
                    blue = value;
                    break;
                case "green":
                    green = value;
                    break;
            }
        }

        return new Round(red, blue, green);
    }

    static int SumPowers(List<Game> games)
    {
        return games.Aggregate(0, (sum, game) =>
        {
            sum += game.redMax * game.blueMax * game.greenMax;
            return sum;
        });
    }
}
