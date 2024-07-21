namespace SFSimulator.Core;

public interface IRuneQuantityProvider
{
    void Setup(ICollection<DayRuneQuantity> quantity);

    int GetRunesQuantity(int day);
}