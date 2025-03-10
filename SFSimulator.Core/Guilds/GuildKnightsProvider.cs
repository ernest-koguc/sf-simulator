namespace SFSimulator.Core;

public class GuildKnightsProvider : IGuildKnightsProvider
{
    private DayKnightsQuantity[] KnightsQuantity { get; set; } = GetDefaultKnightsQuantity();

    public void Setup(int currentKnights, ICollection<DayKnightsQuantity>? knightsQuantity = null)
    {
        var quantities = knightsQuantity?.ToArray() ?? GetDefaultKnightsQuantity();

        if (quantities.All(e => e.Quantity > currentKnights))
        {
            KnightsQuantity = [.. quantities];
            return;
        }

        var quantitiesForCurrentKnights = quantities
            .SkipWhile(e => e.Quantity <= currentKnights)
            .Aggregate(new List<DayKnightsQuantity>().AsEnumerable(), (prev, next) => prev.Append(next with { Day = next.Day - prev.LastOrDefault().Day }))
            .Skip(1);

        KnightsQuantity = quantitiesForCurrentKnights.ToArray();
    }

    public int GetKnightsAmount(int day) => KnightsQuantity.LastOrDefault((e) => e.Day <= day).Quantity;

    private static DayKnightsQuantity[] GetDefaultKnightsQuantity()
    {
        return
        [
            new(2, 250),
            new(3, 300),
            new(4, 350),
            new(5, 400),
            new(6, 450),
            new(9, 500),
            new(19, 550),
            new(29, 600),
            new(43, 650),
            new(60, 700),
            new(79, 750),
            new(153, 800),
            new(165, 850),
            new(178, 900),
            new(224, 950),
            new(239, 1000),
        ];
    }
}

public readonly record struct DayKnightsQuantity(int Day, int Quantity);