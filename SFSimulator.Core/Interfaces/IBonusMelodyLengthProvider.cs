namespace SFSimulator.Core
{
    public interface IBonusMelodyLengthProvider
    {
        int GetBonusMelodyLength(IFightable entity);
    }
}