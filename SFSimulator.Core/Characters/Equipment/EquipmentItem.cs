namespace SFSimulator.Core;

public class EquipmentItem : IAttributable
{
    public WitchScrollType ScrollType { get; set; }
    public bool HasSocket { get; set; }
    public RuneType RuneType { get; set; }
    public int RuneValue { get; set; }
    public GemType GemType { get; set; }
    public int GemValue { get; set; }
    public int UpgradeLevel { get; set; }
    public int Armor { get; set; }
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    public int Constitution { get; set; }
    public int Luck { get; set; }
}
