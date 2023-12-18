namespace SFSimulator.Core
{
    public interface IFightableContextFactory
    {
        IFightableContext Create<T, E>(IFightable<T> main, IFightable<E> opponent) where T : IWeaponable where E : IWeaponable;
    }
}
