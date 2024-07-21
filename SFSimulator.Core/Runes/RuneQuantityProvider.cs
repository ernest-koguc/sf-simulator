namespace SFSimulator.Core;

public class RuneQuantityProvider : IRuneQuantityProvider
{
    private DayRuneQuantity[] RuneQuantity { get; set; }
    public RuneQuantityProvider()
    {
        RuneQuantity = GetDefaultRuneQuantity();
    }

    public void Setup(ICollection<DayRuneQuantity> quantity)
    {
        RuneQuantity = quantity.ToArray();

        if (quantity.Count == 0) RuneQuantity = [default];
    }

    public int GetRunesQuantity(int day) => RuneQuantity.Last((e) => e.Day <= day).Quantity;

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