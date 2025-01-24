namespace SFSimulator.Core;

public enum DungeonTypeEnum
{
    Default = -1,
    Twister = 1,
    Tower = 2,
    Shadow = 4,
    LoopOfIdols = 8,
}
public static class DungeonTypeEnumExtensions
{
    public static bool WithCompanions(this DungeonTypeEnum type)
    {
        return (DungeonTypeEnum.Tower | DungeonTypeEnum.Shadow | DungeonTypeEnum.LoopOfIdols).HasFlag(type);
    }
}