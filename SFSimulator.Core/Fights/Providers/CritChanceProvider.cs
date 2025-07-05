namespace SFSimulator.Core;

public class CritChanceProvider : ICritChanceProvider
{
    public double CalculateCritChance<T, E>(IFightable<T> main, IFightable<E> opponent, double cap = 0.5D, double critBonus = 0) where T : IWeaponable where E : IWeaponable
    {
        return Math.Min(cap, (main.Luck * 5D / (opponent.Level * 2) / 100D) + critBonus);
    }
}