namespace SFSimulator.Core;

public class Character : IFightable<EquipmentWeapon>, IHealthCalculatable
{
    public int Level { get; set; } = 1;
    public ClassType Class { get; set; }
    public long Experience { get; set; } = 0;
    public int BaseStat { get; set; } = 0;
    public decimal Gold { get; set; } = 0;

    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    public int Constitution { get; set; }
    public int Luck { get; set; }
    public int Armor => Items.Sum(i => i.Armor);

    public EquipmentWeapon? FirstWeapon { get; set; }
    public EquipmentWeapon? SecondWeapon { get; set; }
    public List<EquipmentItem> Items { get; set; } = new();
    public List<Potion> Potions { get; set; } = new();

    public int LightningResistance => Items.Where(i => i.RuneType == RuneType.LightningResistance || i.RuneType == RuneType.TotalResistance).Sum(i => i.RuneValue);
    public int FireResistance => Items.Where(i => i.RuneType == RuneType.FireResistance || i.RuneType == RuneType.TotalResistance).Sum(i => i.RuneValue);
    public int ColdResistance => Items.Where(i => i.RuneType == RuneType.ColdResistance || i.RuneType == RuneType.TotalResistance).Sum(i => i.RuneValue);
    public int HealthRune => Items.Where(i => i.RuneType == RuneType.HealthBonus).Sum(i => i.RuneValue);

    public bool HasWeaponScroll => FirstWeapon?.HasEnchantment == true || SecondWeapon?.HasEnchantment == true;
    public bool HasEternityPotion => Potions.Any(p => p.Type == PotionType.Eternity);

    public int GladiatorLevel { get; set; }
    public double SoloPortal { get; set; }
    public double GuildPortal { get; set; }
    public double CritMultiplier => 2 + 0.11 * GladiatorLevel + (HasWeaponScroll ? 0.05 : 0);
    public int Reaction => Items.Any(i => i.ScrollType == WitchScrollType.Reaction) ? 1 : 0;
    public long Health => this.GetHealth();

    public Companion[] Companions { get; set; } = default!;
}
