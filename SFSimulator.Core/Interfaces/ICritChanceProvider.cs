namespace SFSimulator.Core
{
    public interface ICritChanceProvider
    {
        double CalculateCritChance<T, E>(IFightable<T> main, IFightable<E> opponent, double cap = 0.5D) where T : IWeaponable where E : IWeaponable;
    }
}