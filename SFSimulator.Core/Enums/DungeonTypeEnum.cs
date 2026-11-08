namespace SFSimulator.Core;

public enum DungeonTypeEnum
{
    Default = -1,
    Twister = 1,
    Tower = 2,
    Shadow = 3,
    LoopOfIdols = 4,
    Sandstorm = 5,
    Class = 6,
}
public static class DungeonTypeEnumExtensions
{
    extension(DungeonTypeEnum type) 
    {
        public bool WithCompanions
        => type is DungeonTypeEnum.Tower or DungeonTypeEnum.Shadow or DungeonTypeEnum.LoopOfIdols;
    }

    public static DungeonTypeEnum FromPosition(int index) 
    {
        return index switch {
            -1 => DungeonTypeEnum.Tower,
            -2 => DungeonTypeEnum.Twister,
            -3 => DungeonTypeEnum.LoopOfIdols,
            -4 => DungeonTypeEnum.Sandstorm,
            < 100 => DungeonTypeEnum.Default,
            < 200 => DungeonTypeEnum.Shadow,
            < 300 => DungeonTypeEnum.Class,
            _ => throw new ArgumentOutOfRangeException(nameof(index))
        };
    }
}