namespace SFSimulator.Core;

public class Companion : IFightable<EquipmentWeapon>, IHealthCalculatable
{
    public Character Character { get; set; } = null!;

    public int Level => Character.Level;
    public ClassType Class { get; set; }

    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    public int Constitution { get; set; }
    public int Luck { get; set; }
    public int Armor { get; set; }

    public EquipmentWeapon? FirstWeapon { get; set; }
    public EquipmentWeapon? SecondWeapon { get; set; }
    public List<EquipmentItem> Items { get; set; } = new();

    public int LightningResistance { get; set; }
    public int FireResistance { get; set; }
    public int ColdResistance { get; set; }
    public int HealthRune { get; set; }

    public bool HasGlovesScroll { get; set; }
    public bool HasWeaponScroll => FirstWeapon?.HasEnchantment == true || SecondWeapon?.HasEnchantment == true;
    public bool HasEternityPotion { get; set; }

    public double SoloPortal => Character.SoloPortal;
    public double GuildPortal => Character.GuildPortal;
    public double CritMultiplier => 2 + 0.11 * Character.GladiatorLevel + (HasWeaponScroll ? 0.05 : 0);
    public int Reaction => Items.Any(i => i.ScrollType == WitchScrollType.Reaction) ? 1 : 0;
    public long Health => this.GetHealth();
}
