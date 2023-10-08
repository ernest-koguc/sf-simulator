namespace SFSimulator.Core;

public enum DungeonTypeEnum
{
    Default,
    Twister,
    Tower,
    Shadow,
    WithCompanions = Shadow | Tower,
}
