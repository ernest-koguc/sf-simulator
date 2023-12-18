namespace SFSimulator.Core;

public class RawWeapon : IWeaponable
{
    public int RuneValue { get; set; }
    public RuneType RuneType { get; set; }
    public int MaxDmg { get; set; }
    public int MinDmg { get; set; }
}
