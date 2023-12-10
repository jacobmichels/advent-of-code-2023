namespace part1;

class Card
{
    public List<int> Numbers { get; set; }
    public HashSet<int> Winners { get; set; }
    public int Copies { get; set; }
    public int Score { get; set; }

    public Card(List<int> numbers, HashSet<int> winners)
    {
        this.Numbers = numbers;
        this.Winners = winners;
        this.Copies = 1;
        this.Score = 0;
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

        ProcessCards(cards);
        Console.WriteLine($"Cards Count: {CountCards(cards)}");
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

    static void ProcessCards(List<Card> cards)
    {
        CalculateScores(cards);
        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            for (int j = 0; j < card.Copies; j++)
            {
                for (int k = i + 1; k <= i + card.Score; k++)
                {
                    cards[k].Copies++;
                }
            }
        }
    }

    static int CountCards(List<Card> cards)
    {
        return cards.Aggregate(0, (sum, card) => sum += card.Copies);
    }

    static void CalculateScores(List<Card> cards)
    {
        foreach (var card in cards)
        {
            CalculateCardScore(card);
        }
    }

    static void CalculateCardScore(Card card)
    {
        card.Score = card.Numbers.Aggregate(0, (sum, num) =>
        {
            if (card.Winners.Contains(num))
            {
                sum += 1;
            }
            return sum;
        });
    }
}
