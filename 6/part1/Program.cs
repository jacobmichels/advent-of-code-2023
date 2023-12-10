using Microsoft.VisualBasic;

namespace part1;

public record Race(int time, int distance);

class Program
{
    static void Main(string[] args)
    {
        var races = ParseRaces();
        var answer = races.Aggregate(1, (product, race) => product * CountWinningStrategies(race));
        Console.WriteLine(answer);
    }

    static List<Race> ParseRaces()
    {
        string? timeLine = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(timeLine))
        {
            throw new InvalidDataException();
        }

        string? distanceLine = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(distanceLine))
        {
            throw new InvalidDataException();
        }

        var times = timeLine.Split(":")[1].Trim().Split(" ").Where(s => !string.IsNullOrWhiteSpace(s));
        var distances = distanceLine.Split(":")[1].Trim().Split(" ").Where(s => !string.IsNullOrWhiteSpace(s));

        var zipped = times.Zip(distances)
        .Select((zip) => new Race(int.Parse(zip.First), int.Parse(zip.Second)));

        return zipped.ToList();
    }

    static int CountWinningStrategies(Race race)
    {
        var count = 0;
        for (int i = 0; i <= race.time; i++)
        {
            var distance = i * (race.time - i);
            if (distance > race.distance)
            {
                count++;
            }
        }

        return count;
    }
}
