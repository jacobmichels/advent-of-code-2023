using System.Text;

namespace part2;

class Pair
{
    public int Value { get; set; }

    public Pair(char first, char last)
    {
        Value = int.Parse(first.ToString() + last.ToString());
        Console.WriteLine($"{first} {last}: {Value}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        var lines = BuildInputLines();

        var pairs = BuildPairs(lines);

        var sum = pairs.Aggregate(0, (sum, pair) => sum += pair.Value);

        Console.WriteLine(sum);
    }

    static List<Pair> BuildPairs(List<string> lines)
    {
        return lines.Select(BuildPair).ToList();
    }

    static Pair BuildPair(string line)
    {
        var dict = new Dictionary<string, char>{
            {"one", '1'},
            {"two", '2'},
            {"three", '3'},
            {"four", '4'},
            {"five", '5'},
            {"six", '6'},
            {"seven", '7'},
            {"eight", '8'},
            {"nine", '9'},
            {"zero", '0'}
        };

        var extracted = ExtractValue(dict, line);

        if (extracted.Item1 == 'x' || extracted.Item2 == 'x')
        {
            throw new InvalidDataException();
        }

        return new Pair(extracted.Item1, extracted.Item2);
    }

    static (char, char) ExtractValue(Dictionary<string, char> dict, string line)
    {
        char first = 'x';
        char last = 'x';
        foreach (var iter in line.Select((c, i) => new { Value = c, Index = i }))
        {
            Console.WriteLine($"value: {iter.Value} index: {iter.Index}");
            if (dict.ContainsValue(iter.Value))
            {
                Console.WriteLine("value contained!");
                if (first == 'x')
                {
                    first = iter.Value;
                }
                last = iter.Value;
                continue;
            }
            try
            {
                char value;
                var threeLookAhead = line.Substring(iter.Index, 3);
                if (dict.TryGetValue(threeLookAhead, out value))
                {
                    Console.WriteLine($"found threeLookAhead {threeLookAhead}");
                    if (first == 'x')
                    {
                        first = value;
                    }
                    last = value;
                    continue;
                }

                var fourLookAhead = line.Substring(iter.Index, 4);
                if (dict.TryGetValue(fourLookAhead, out value))
                {
                    Console.WriteLine($"found fourLookAhead {fourLookAhead}");
                    if (first == 'x')
                    {
                        first = value;
                    }
                    last = value;
                    continue;
                }

                var fiveLookAhead = line.Substring(iter.Index, 5);
                if (dict.TryGetValue(fiveLookAhead, out value))
                {
                    Console.WriteLine($"found fiveLookAhead {fiveLookAhead}");
                    if (first == 'x')
                    {
                        first = value;
                    }
                    last = value;
                    continue;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                continue;
            }
        }
        return (first, last);
    }

    static List<string> BuildInputLines()
    {
        var lines = new List<string>();
        string? line;
        while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
        {
            lines.Add(line.ToLower());
        }

        return lines;
    }
}
