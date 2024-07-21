namespace SFSimulator.Core;

public class Character : IFightable<EquipmentItem>, IHealthCalculatable, ITotalStatsCalculatable
{
    public int Level { get; set; } = 1;
    public ClassType Class { get; set; }
    public long Experience { get; set; }
    public int BaseStat { get; set; }
    public decimal Gold { get; set; }

    public bool IsCompanion => false;

    public int BaseStrength { get; set; }
    public int BaseDexterity { get; set; }
    public int BaseIntelligence { get; set; }
    public int BaseConstitution { get; set; }
    public int BaseLuck { get; set; }

    public int Strength => this.GetTotalStatsFor(AttributeType.Strength);
    public int Dexterity => this.GetTotalStatsFor(AttributeType.Dexterity);
    public int Intelligence => this.GetTotalStatsFor(AttributeType.Intelligence);
    public int Constitution => this.GetTotalStatsFor(AttributeType.Constitution);
    public int Luck => this.GetTotalStatsFor(AttributeType.Luck);

    public int Armor => Items.SimpleList.Sum(i => i.Armor);

    public EquipmentItem? FirstWeapon => Items.FirstWeapon;
    public EquipmentItem? SecondWeapon => Items.SecondWeapon;
    public List<Potion> Potions { get; set; } = [];
    public FightableItems Items { get; set; } = default!;

    public int LightningResistance => Items.SimpleList.Where(i => i.RuneType is RuneType.LightningResistance or RuneType.TotalResistance).Sum(i => i.RuneValue);
    public int FireResistance => Items.SimpleList.Where(i => i.RuneType is RuneType.FireResistance or RuneType.TotalResistance).Sum(i => i.RuneValue);
    public int ColdResistance => Items.SimpleList.Where(i => i.RuneType is RuneType.ColdResistance or RuneType.TotalResistance).Sum(i => i.RuneValue);
    public int HealthRune => Items.SimpleList.Where(i => i.RuneType == RuneType.HealthBonus).Sum(i => i.RuneValue);

    public bool HasWeaponScroll => FirstWeapon?.HasEnchantment == true || SecondWeapon?.HasEnchantment == true;
    public bool HasEternityPotion => Potions.Any(p => p.Type == PotionType.Eternity);

    public int GladiatorLevel { get; set; }
    public double SoloPortal { get; set; }
    public double GuildPortal { get; set; }
    public double CritMultiplier => 2 + (0.11 * GladiatorLevel) + (HasWeaponScroll ? 0.05 : 0);
    public int Reaction => Items.SimpleList.Any(i => i.ScrollType == WitchScrollType.Reaction) ? 1 : 0;
    public long Health => this.GetHealth();

    public Companion[] Companions { get; set; } = default!;

    public PetsState Pets { get; set; } = default!;
    public int Aura { get; set; }
    public BlackSmithResources BlackSmithResources { get; set; }
}