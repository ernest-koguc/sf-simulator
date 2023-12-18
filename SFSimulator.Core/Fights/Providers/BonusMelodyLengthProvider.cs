namespace SFSimulator.Core;

public class BonusMelodyLengthProvider : IBonusMelodyLengthProvider
{
    public int GetBonusMelodyLength<T>(IFightable<T> entity) where T : IWeaponable
    {
        if (entity.Constitution >= 0.75 * entity.Intelligence)
            return 2;

        if (entity.Constitution >= 0.5 * entity.Intelligence)
            return 1;

        return 0;
    }
}
