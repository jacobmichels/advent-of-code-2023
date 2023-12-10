namespace part1;

public record MappingDefinition(long DestRangeStart, long SourceRangeStart, long Length);

class Almanac
{
    public List<long> Seeds { get; }
    public List<MappingDefinition> SeedToSoil { get; }
    public List<MappingDefinition> SoilToFert { get; }
    public List<MappingDefinition> FertToWater { get; }
    public List<MappingDefinition> WaterToLight { get; }
    public List<MappingDefinition> LightToTemp { get; }
    public List<MappingDefinition> TempToHumid { get; }
    public List<MappingDefinition> HumidToLocation { get; }

    private Almanac(List<long> seeds, List<MappingDefinition> seedToSoil, List<MappingDefinition> soilToFert, List<MappingDefinition> fertToWater, List<MappingDefinition> waterToLight, List<MappingDefinition> lightToTemp, List<MappingDefinition> tempToHumid, List<MappingDefinition> humidToLocation)
    {
        Seeds = seeds;
        SeedToSoil = seedToSoil;
        SoilToFert = soilToFert;
        FertToWater = fertToWater;
        WaterToLight = waterToLight;
        LightToTemp = lightToTemp;
        TempToHumid = tempToHumid;
        HumidToLocation = humidToLocation;
    }

    public static Almanac Parse()
    {
        List<long> seeds = new List<long>();
        List<MappingDefinition> seedToSoil = new List<MappingDefinition>();
        List<MappingDefinition> soilToFert = new List<MappingDefinition>();
        List<MappingDefinition> fertToWater = new List<MappingDefinition>();
        List<MappingDefinition> waterToLight = new List<MappingDefinition>();
        List<MappingDefinition> lightToTemp = new List<MappingDefinition>();
        List<MappingDefinition> tempToHumid = new List<MappingDefinition>();
        List<MappingDefinition> humidToLocation = new List<MappingDefinition>();

        string? line;
        while ((line = Console.ReadLine()) != null)
        {
            if (line.StartsWith("seeds: "))
            {
                seeds = ParseSeeds(line);
            }
            else if (line.StartsWith("seed-to-soil map:"))
            {
                seedToSoil = ParseMap();
            }
            else if (line.StartsWith("soil-to-fertilizer map:"))
            {
                soilToFert = ParseMap();
            }
            else if (line.StartsWith("fertilizer-to-water map:"))
            {
                fertToWater = ParseMap();
            }
            else if (line.StartsWith("water-to-light map:"))
            {
                waterToLight = ParseMap();
            }
            else if (line.StartsWith("light-to-temperature map:"))
            {
                lightToTemp = ParseMap();
            }
            else if (line.StartsWith("temperature-to-humidity map:"))
            {
                tempToHumid = ParseMap();
            }
            else if (line.StartsWith("humidity-to-location map:"))
            {
                humidToLocation = ParseMap();
            }
        }

        return new Almanac(seeds, seedToSoil, soilToFert, fertToWater, waterToLight, lightToTemp, tempToHumid, humidToLocation);
    }

    private static List<long> ParseSeeds(string line)
    {
        return line.Split("seeds: ")[1].Split(" ")
            .Where((s) => !String.IsNullOrWhiteSpace(s))
            .Select((s) => long.Parse(s))
            .ToList();
    }

    private static List<MappingDefinition> ParseMap()
    {
        var map = new List<MappingDefinition>();

        string? line;
        while ((line = Console.ReadLine()) != null)
        {
            if (String.IsNullOrWhiteSpace(line))
            {
                break;
            }
            var split = line.Split(" ");

            var destRangeStart = long.Parse(split[0]);
            var sourceRangeStart = long.Parse(split[1]);
            var len = long.Parse(split[2]);

            var def = new MappingDefinition(destRangeStart, sourceRangeStart, len);

            map.Add(def);
        }

        return map;
    }

    private static long ApplyTransformation(List<MappingDefinition> transformation, long seed)
    {
        // find the mapping defintion that this seed is contained in
        // if there isn't one, just return the seed
        var def = transformation.Find((def) =>
        {
            {
                var upperExclusive = def.SourceRangeStart + def.Length;
                var lowerInclusive = def.SourceRangeStart;

                return seed >= lowerInclusive && seed < upperExclusive;
            }
        });
        if (def == null)
        {
            return seed;
        }
        return seed - def.SourceRangeStart + def.DestRangeStart;
    }

    public void Print()
    {
        Console.WriteLine("seeds: " + String.Join(" ", Seeds));
        Console.WriteLine("seed-to-soil map:");
        foreach (var m in SeedToSoil)
        {
            Console.WriteLine($"{m.DestRangeStart} {m.SourceRangeStart} {m.Length}");
        }
        Console.WriteLine("soil-to-fertilizer map:");
        foreach (var m in SoilToFert)
        {
            Console.WriteLine($"{m.DestRangeStart} {m.SourceRangeStart} {m.Length}");
        }
        Console.WriteLine("fertilizer-to-water map:");
        foreach (var m in FertToWater)
        {
            Console.WriteLine($"{m.DestRangeStart} {m.SourceRangeStart} {m.Length}");
        }
        Console.WriteLine("water-to-light map:");
        foreach (var m in WaterToLight)
        {
            Console.WriteLine($"{m.DestRangeStart} {m.SourceRangeStart} {m.Length}");
        }
        Console.WriteLine("light-to-temperature map:");
        foreach (var m in LightToTemp)
        {
            Console.WriteLine($"{m.DestRangeStart} {m.SourceRangeStart} {m.Length}");
        }
        Console.WriteLine("temperature-to-humidity map:");
        foreach (var m in TempToHumid)
        {
            Console.WriteLine($"{m.DestRangeStart} {m.SourceRangeStart} {m.Length}");
        }
        Console.WriteLine("humidity-to-location map:");
        foreach (var m in HumidToLocation)
        {
            Console.WriteLine($"{m.DestRangeStart} {m.SourceRangeStart} {m.Length}");
        }
    }

    public long FindLowestLocation()
    {
        var transformations = new List<List<MappingDefinition>>{
            SeedToSoil,
            SoilToFert,
            FertToWater,
            WaterToLight,
            LightToTemp,
            TempToHumid,
            HumidToLocation
        };

        return Seeds.Select((seed) =>
        {
            foreach (var transformation in transformations)
            {
                seed = ApplyTransformation(transformation, seed);
            }
            return seed;
        }).Aggregate(long.MaxValue, (min, transformed) => Math.Min(min, transformed));
    }
}

class Program
{
    static void Main(string[] args)
    {
        var a = Almanac.Parse();
        Console.WriteLine("parsing done");

        Console.WriteLine(a.FindLowestLocation());
    }
}
