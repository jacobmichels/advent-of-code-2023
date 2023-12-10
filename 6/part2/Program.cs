using Microsoft.VisualBasic;

namespace part1;

public record Race(long time, long distance);

class Program
{
    static void Main(string[] args)
    {
        var race = ParseRace();
        var answer = CountWinningStrategies(race);
        Console.WriteLine(answer);
    }

    static Race ParseRace()
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

        var timeBroken = timeLine.Split(":")[1].Trim().Split(" ").Where(s => !string.IsNullOrWhiteSpace(s));
        var distanceBroken = distanceLine.Split(":")[1].Trim().Split(" ").Where(s => !string.IsNullOrWhiteSpace(s));

        var time = long.Parse(string.Join("", timeBroken));
        var distance = long.Parse(string.Join("", distanceBroken));

        return new Race(time, distance);
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
