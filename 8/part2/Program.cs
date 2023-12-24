using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web;

namespace part1;

class Location
{
    public Location? Left { get; set; }
    public Location? Right { get; set; }
    public string Name { get; }

    public Location(string name)
    {
        Name = name;
    }

    public Location(string name, Location left, Location right)
    {
        Left = left;
        Right = right;
        Name = name;
    }

    override public string ToString()
    {
        return Name;
    }
}

class Program
{
    static Dictionary<string, Location> ParseMap()
    {
        var map = new Dictionary<string, Location>();

        string? line;
        while ((line = Console.ReadLine()) != null)
        {
            var matches = Regex.Matches(line, @"[A-Z0-9]{3}", RegexOptions.None);
            if (matches.Count != 3)
            {
                throw new InvalidDataException();
            }

            if (!map.TryGetValue(matches[0].Value, out Location? location))
            {
                location = new Location(matches[0].Value);
                map.Add(matches[0].Value, location);
            }

            if (!map.TryGetValue(matches[1].Value, out Location? leftLocation))
            {
                leftLocation = new Location(matches[1].Value);
                map.Add(matches[1].Value, leftLocation);
            }
            location.Left = leftLocation;

            if (!map.TryGetValue(matches[2].Value, out Location? rightLocation))
            {
                rightLocation = new Location(matches[2].Value);
                map.Add(matches[2].Value, rightLocation);

            }
            location.Right = rightLocation;
        }

        return map;
    }

    static ReadOnlyCollection<char> ParseInstructions()
    {
        var line = Console.ReadLine();
        Console.ReadLine();

        if (line == null)
        {
            throw new InvalidDataException();
        }

        return line.Aggregate(new List<char>(), (list, c) =>
        {
            if (!char.IsWhiteSpace(c))
            {
                list.Add(c);
            }
            return list;
        }).AsReadOnly();
    }

    static bool AllNodesAtEnd(IEnumerable<Location> nodes)
    {
        return nodes.All((x) => x.Name.EndsWith('Z'));
    }

    static char FetchInstructionForStep(long step, ReadOnlyCollection<char> instructions)
    {
        if (instructions.Count == 0)
        {
            throw new InvalidOperationException("Instructions list is empty.");
        }
        return instructions[(int)Math.Abs(step % instructions.Count)];
    }

    static void Main(string[] args)
    {
        var instructions = ParseInstructions();
        var map = ParseMap();

        // foreach (KeyValuePair<string, Location> item in map)
        // {
        //     Console.WriteLine($"Key: {item.Key}, Left: {item.Value.Left} Right: {item.Value.Right}");
        // }

        var nodes = map.Where((x) => x.Key.EndsWith('A')).Select((x) => x.Value).ToList();
        var concurrentPathCount = nodes.Count();

        long step = 0;
        while (!AllNodesAtEnd(nodes))
        {
            var instruction = FetchInstructionForStep(step, instructions);

            for (int i = 0; i < concurrentPathCount; i++)
            {
                var current = nodes[i];
                if (instruction == 'L')
                {
                    current = current.Left;
                    if (current == null)
                    {
                        throw new InvalidDataException();
                    }
                    nodes[i] = current;
                }
                else
                {
                    current = current.Right;
                    if (current == null)
                    {
                        throw new InvalidDataException();
                    }
                    nodes[i] = current;
                }
            }

            step++;
        }

        Console.WriteLine(step);
    }
}
