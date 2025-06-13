namespace SFSimulator.Core
{
    public interface IBonusMelodyLengthProvider
    {
        int GetBonusMelodyLength<T>(IFightable<T> entity) where T : IWeaponable;
    }
}