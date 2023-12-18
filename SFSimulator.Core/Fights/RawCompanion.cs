namespace SFSimulator.Core;

public class RawCompanion : IFightable<RawWeapon>, IHealthCalculatable
{
    public ClassType Class { get; set; }
    public RawFightable? Character { get; set; } = default!;
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    public int Constitution { get; set; }
    public int Luck { get; set; }
    public long Health => this.GetHealth();
    public int Armor { get; set; }
    public RawWeapon? FirstWeapon { get; set; }
    public RawWeapon? SecondWeapon { get; set; }
    public int Reaction { get; set; }
    public bool HasWeaponScroll { get; set; }
    public bool HasEternityPotion => Character?.HasEternityPotion ?? false;
    public double CritMultiplier => 2 + 0.11 * Character?.GladiatorLevel ?? 0 + (HasWeaponScroll ? 0.05 : 0);
    public int LightningResistance { get; set; }
    public int FireResistance { get; set; }
    public int ColdResistance { get; set; }
    public int HealthRune { get; set; }
    public double GuildPortal => Character?.GuildPortal ?? 0;
    public double SoloPortal => Character?.SoloPortal ?? 0;
    public int Level => Character?.Level ?? 1;
}
