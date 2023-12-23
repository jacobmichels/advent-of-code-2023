using System.Diagnostics;

namespace part1;


public enum HandType
{
    FiveOfAKind,
    FourOfAKind,
    FullHouse,
    ThreeOfAKind,
    TwoPair,
    OnePair,
    HighCard,
}

public static class HandTypeExtensions
{
    public static int GetValue(this HandType type)
    {
        switch (type)
        {
            case HandType.FiveOfAKind:
                return 7;
            case HandType.FourOfAKind:
                return 6;
            case HandType.FullHouse:
                return 5;
            case HandType.ThreeOfAKind:
                return 4;
            case HandType.TwoPair:
                return 3;
            case HandType.OnePair:
                return 2;
            case HandType.HighCard:
                return 1;
            default:
                throw new InvalidOperationException("Unknown hand type");
        }
    }
}

class Hand
{
    public List<char> Cards { get; }
    public HandType Type { get; }
    public int Bid { get; }

    public Hand(List<char> cards, int bid)
    {
        Cards = cards;
        Bid = bid;

        Type = ComputeType();
    }

    private HandType ComputeType()
    {
        if (IsFiveOfAKind())
        {
            return HandType.FiveOfAKind;
        }
        else if (IsFourOfAKind())
        {
            return HandType.FourOfAKind;
        }
        else if (IsFullHouse())
        {
            return HandType.FullHouse;
        }
        else if (IsThreeOfAKind())
        {
            return HandType.ThreeOfAKind;
        }
        else if (IsTwoPair())
        {
            return HandType.TwoPair;
        }
        else if (IsOnePair())
        {
            return HandType.OnePair;
        }
        else
        {
            return HandType.HighCard;
        }
    }

    private bool IsFiveOfAKind()
    {
        return Cards.Distinct().Count() == 1;
    }

    private bool IsFourOfAKind()
    {
        var distinctCards = Cards.Distinct().ToList();
        if (distinctCards.Count != 2)
        {
            return false;
        }

        var cardCounts = Cards.GroupBy(c => c)
                              .Select(group => group.Count())
                              .OrderBy(count => count)
                              .ToList();

        return cardCounts.SequenceEqual(new[] { 1, 4 });
    }

    private bool IsFullHouse()
    {
        var distinctCards = Cards.Distinct().ToList();
        if (distinctCards.Count != 2)
        {
            return false;
        }

        var cardCounts = Cards.GroupBy(c => c)
                              .Select(group => group.Count())
                              .OrderBy(count => count)
                              .ToList();

        return cardCounts.SequenceEqual(new[] { 2, 3 });
    }

    private bool IsThreeOfAKind()
    {
        var distinctCards = Cards.Distinct();
        if (distinctCards.Count() != 3)
        {
            return false;
        }

        var cardCounts = Cards.GroupBy(c => c)
                              .Select(group => group.Count())
                              .OrderBy(count => count)
                              .ToList();

        return cardCounts.SequenceEqual(new[] { 1, 1, 3 });
    }

    private bool IsTwoPair()
    {
        var distinctCards = Cards.Distinct();
        if (distinctCards.Count() != 3)
        {
            return false;
        }

        var cardCounts = Cards.GroupBy(c => c)
                              .Select(group => group.Count())
                              .OrderBy(count => count)
                              .ToList();

        return cardCounts.SequenceEqual(new[] { 1, 2, 2 });
    }

    private bool IsOnePair()
    {
        var distinctCards = Cards.Distinct();
        if (distinctCards.Count() != 4)
        {
            return false;
        }

        var cardCounts = Cards.GroupBy(c => c)
                              .Select(group => group.Count())
                              .OrderBy(count => count)
                              .ToList();
        return cardCounts.SequenceEqual(new[] { 1, 1, 1, 2 });
    }
}

class Program
{
    static void Main(string[] args)
    {
        var hands = ParseHands();
        // I want to sort hands by rank. Rank is determined by stength of a hand relative to the other hands in the game.
        hands.Sort(delegate (Hand x, Hand y)
        {
            if (x.Type.GetValue() > y.Type.GetValue())
            {
                return -1;
            }
            else if (x.Type.GetValue() < y.Type.GetValue())
            {
                return 1;
            }
            else
            {
                return CompareSameTypeHands(x, y);
            }
        });

        // oh my god my sort sorts in desc rather than asc. LOL
        // so instead of fixing the problem I just slap a reversal here
        hands.Reverse();

        foreach (var hand in hands)
        {
            Console.Write($"{hand.Type.GetValue()} {hand.Bid} ");
            foreach (var card in hand.Cards)
            {
                Console.Write(card);
            }
            Console.WriteLine();
        }

        // var sum = 0;
        // for (int i = 0; i < hands.Count; i++)
        // {
        //     var rank = i + 1;
        //     var hand = hands[i];

        //     Console.WriteLine($"{rank * hand.Bid}");
        //     sum += rank * hand.Bid;
        // }

        // Console.WriteLine(sum);
    }

    static int CompareSameTypeHands(Hand x, Hand y)
    {
        Dictionary<char, int> CardValueTable = new Dictionary<char, int>
        {
            {'A', 14},
            {'K', 13},
            {'Q', 12},
            {'J', 11},
            {'T', 10},
            {'9', 9},
            {'8', 8},
            {'7', 7},
            {'6', 6},
            {'5', 5},
            {'4', 4},
            {'3', 3},
            {'2', 2},
        };

        var zipped = x.Cards.Zip(y.Cards).Select((x) => (CardValueTable[x.First], CardValueTable[x.Second]));
        foreach (var (first, second) in zipped)
        {
            if (first > second)
            {
                return -1;
            }
            else if (first < second)
            {
                return 1;
            }
        }

        throw new UnreachableException();
    }

    static List<Hand> ParseHands()
    {
        var hands = new List<Hand>();
        string? line;
        while ((line = Console.ReadLine()) != null)
        {
            hands.Add(ParseHand(line));
        }
        return hands;
    }

    static Hand ParseHand(string line)
    {
        var split = line.Split(" ");

        var cards = new List<char>();
        // this is constant time, there will always be 5 cards
        foreach (var c in split[0])
        {
            cards.Add(c);
        }

        var bid = int.Parse(split[1].Trim());

        return new Hand(cards, bid);
    }
}
