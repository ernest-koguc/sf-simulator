namespace SFSimulator.Core;

public interface IWeaponable
{
    int MinDmg { get; }
    int MaxDmg { get; }
    RuneType RuneType { get; }
    int RuneValue { get; }
}