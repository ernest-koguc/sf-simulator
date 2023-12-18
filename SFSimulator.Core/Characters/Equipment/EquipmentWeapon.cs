namespace SFSimulator.Core;

public class EquipmentWeapon : IAttributable, IWeaponable
{
    public bool HasEnchantment { get; set; }
    public bool HasSocket { get; set; }
    public RuneType RuneType { get; set; }
    public int RuneValue { get; set; }
    public GemType GemType { get; set; }
    public int GemValue { get; set; }
    public int UpgradeLevel { get; set; }
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    public int Constitution { get; set; }
    public int Luck { get; set; }

    public int MinDmg { get; set; } = 0;
    public int MaxDmg { get; set; } = 0;
}
