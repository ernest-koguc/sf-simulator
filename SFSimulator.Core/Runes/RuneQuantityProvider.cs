namespace SFSimulator.Core;

public class RuneQuantityProvider : IRuneQuantityProvider
{
    private DayRuneQuantity[] RuneQuantity { get; set; }
    public RuneQuantityProvider()
    {
        RuneQuantity = GetDefaultRuneQuantity();
    }

    public void Setup(SimulationContext simulationContext, ICollection<DayRuneQuantity>? quantity = null)
    {
        RuneQuantity = quantity?.ToArray() ?? GetDefaultRuneQuantity();
        var lastRecord = RuneQuantity.LastOrDefault(r => r.Quantity <= simulationContext.RuneQuantity);

        RuneQuantity = RuneQuantity
            .SkipWhile(e => e.Quantity <= simulationContext.RuneQuantity)
            .Select(r => r with { Day = r.Day - lastRecord.Day })
            .ToArray();
    }

    public void IncreaseRuneQuantity(SimulationContext simulationContext, int day)
    {
        if (simulationContext.RuneQuantity >= 100)
        {
            return;
        }

        var runeQuantity = RuneQuantity.LastOrDefault((e) => e.Day <= day);
        if (runeQuantity != default)
        {
            simulationContext.RuneQuantity = runeQuantity.Quantity;
        }
    }

    private DayRuneQuantity[] GetDefaultRuneQuantity()
    {
        List<DayRuneQuantity> list = new();
        for (var i = 1; i <= 100; i++)
        {
            list.Add(new DayRuneQuantity(i, i));
        }

        return list.ToArray();
    }
}

public readonly record struct DayRuneQuantity(int Day, int Quantity);