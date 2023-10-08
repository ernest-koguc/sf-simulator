namespace SFSimulator.Core
{
    public interface IFightableContextFactory
    {
        IFightableContext Create(IFightable main, IFightable opponent);
    }
}