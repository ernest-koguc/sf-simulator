namespace SFSimulator.Core;

public class CritChanceProvider : ICritChanceProvider
{
    public double CalculateCritChance(IFightable main, IFightable opponent, double cap = 0.5D)
    {
        return Math.Min(cap, main.Luck * 5D / (opponent.Level * 2) / 100D);
    }
}
