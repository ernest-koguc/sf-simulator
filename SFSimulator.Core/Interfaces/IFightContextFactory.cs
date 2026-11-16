namespace SFSimulator.Core
{
    public interface IFightContextFactory
    {
        FightContext Create<T, E>(IFightable<T> main, IFightable<E> opponent, CombatRandom random) where T : IWeaponable where E : IWeaponable;
    }
}