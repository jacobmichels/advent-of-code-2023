namespace part1;

class Card
{
    public List<int> Numbers { get; set; }
    public HashSet<int> Winners { get; set; }

    public Card(List<int> numbers, HashSet<int> winners)
    {
        this.Numbers = numbers;
        this.Winners = winners;
    }

    public void Print()
    {
        Console.Write($"Winners: ");
        foreach (var winner in Winners)
        {
            Console.Write($"{winner} ");
        }
        Console.WriteLine();

        Console.Write($"Numbers: ");
        foreach (var number in Numbers)
        {
            Console.Write($"{number} ");
        }
        Console.WriteLine();
    }
}

class Program
{
    static void Main(string[] args)
    {
        var cards = ParseCards();
        foreach (var card in cards)
        {
            card.Print();
        }

        Console.WriteLine($"Score: {CalculateScore(cards)}");
    }

    static List<Card> ParseCards()
    {
        List<Card> cards = new List<Card>();
        string? line;
        while ((line = Console.ReadLine()) != null)
        {
            var card = ParseCard(line);
            cards.Add(card);
        }
        return cards;
    }

    static Card ParseCard(string line)
    {
        var split = line.Split("|");
        var numbersSection = split[1].Trim();
        var winnersSection = split[0].Trim().Split(": ")[1].Trim();

        var winners = winnersSection.Split(" ").Where(num => !string.IsNullOrEmpty(num)).Select(num => int.Parse(num)).ToHashSet();
        var numbers = numbersSection.Split(" ").Where(num => !string.IsNullOrEmpty(num)).Select(num => int.Parse(num)).ToList();

        return new Card(numbers, winners);
    }

    static int CalculateScore(List<Card> cards)
    {
        var sum = 0;
        foreach (var card in cards)
        {
            sum += CalculateCardScore(card);
        }

        return sum;
    }

    static int CalculateCardScore(Card card)
    {
        return card.Numbers.Aggregate(0, (sum, num) =>
        {
            if (card.Winners.Contains(num))
            {
                if (sum == 0)
                {
                    sum = 1;
                }
                else
                {
                    sum = sum * 2;
                }
            }
            return sum;
        });
    }
}
