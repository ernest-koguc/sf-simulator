namespace SFSimulator.Core
{
    public interface IFightStartingBehaviorProvider
    {
        StartingBehaviorEnum GetStartingBehavior(IFightable main, IFightable opponent);
    }
}