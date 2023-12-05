namespace _1;

class Pair
{
    public int Value { get; set; }

    public Pair(char first, char last)
    {
        Value = int.Parse(first.ToString() + last.ToString());
    }
}

class Program
{
    static void Main(string[] args)
    {
        var lines = BuildInputLines();
        var pairs = BuildPairs(lines);
        var sum = SumPairs(pairs);
        Console.WriteLine(sum);
    }

    static List<string> BuildInputLines()
    {
        var lines = new List<string>();
        string? line;
        while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
        {
            lines.Add(line);
        }

        return lines;
    }

    static List<Pair> BuildPairs(List<string> lines)
    {
        var result = new List<Pair>();
        foreach (var line in lines)
        {
            var first = ExtractNumber(line, false);
            var last = ExtractNumber(line, true);

            result.Add(new Pair(first, last));
        }

        return result;
    }

    static char ExtractNumber(string line, bool reverse)
    {
        var chars = line.ToCharArray();
        if (reverse)
        {
            return chars.Reverse().First(char.IsDigit);
        }
        else
        {
            return chars.First(char.IsDigit);
        }

        throw new InvalidDataException();
    }

    static int SumPairs(List<Pair> pairs)
    {
        return pairs.Aggregate(0, (sum, current) => sum += current.Value);
    }
}
