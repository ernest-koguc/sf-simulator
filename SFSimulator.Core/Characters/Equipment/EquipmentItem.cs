namespace SFSimulator.Core;

public class EquipmentItem : IWeaponable
{
    public ItemAttributeType ItemAttributeType { get; set; }
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
    public ItemType ItemType { get; set; }
    public ClassType ItemClassType { get; set; }
    public int[] Attributes => [Strength, Dexterity, Intelligence, Constitution, Luck];
    public bool HasEnchantment => ScrollType != WitchScrollType.None;

    public int MinDmg { get; set; }
    public int MaxDmg { get; set; }

    public int this[AttributeType attribute]
    {
        get => attribute switch
        {
            AttributeType.Strength => Strength,
            AttributeType.Dexterity => Dexterity,
            AttributeType.Intelligence => Intelligence,
            AttributeType.Constitution => Constitution,
            AttributeType.Luck => Luck,
            _ => throw new ArgumentOutOfRangeException($"{attribute} is not a supported attribute")
        };
        set
        {
            switch (attribute)
            {
                case AttributeType.Strength:
                    Strength = value;
                    break;
                case AttributeType.Dexterity:
                    Dexterity = value;
                    break;
                case AttributeType.Intelligence:
                    Intelligence = value;
                    break;
                case AttributeType.Constitution:
                    Constitution = value;
                    break;
                case AttributeType.Luck:
                    Luck = value;
                    break;
                default: throw new ArgumentOutOfRangeException($"{attribute} is not a supported attribute");
            }
        }
    }
}