namespace SFSimulator.Core;

public enum DungeonTypeEnum
{
    Default = -1,
    Twister = 1,
    Tower = 2,
    Shadow = 3,
    LoopOfIdols = 4,
    Sandstorm = 5,
}
public static class DungeonTypeEnumExtensions
{
    public static bool WithCompanions(this DungeonTypeEnum type)
        => type is DungeonTypeEnum.Tower or DungeonTypeEnum.Shadow or DungeonTypeEnum.LoopOfIdols;
}