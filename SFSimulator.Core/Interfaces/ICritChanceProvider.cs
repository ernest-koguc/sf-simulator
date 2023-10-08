namespace SFSimulator.Core
{
    public interface ICritChanceProvider
    {
        double CalculateCritChance(IFightable main, IFightable opponent, double cap = 0.5D);
    }
}