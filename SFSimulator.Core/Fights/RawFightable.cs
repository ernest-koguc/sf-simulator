namespace SFSimulator.Core;

public class RawFightable : IFightable<RawWeapon>, IHealthCalculatable
{
    public int Level { get; set; } = 1;
    public ClassType Class { get; set; }

    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    public int Constitution { get; set; }
    public int Luck { get; set; }
    public int Armor { get; set; }

    public RawWeapon? FirstWeapon { get; set; }
    public RawWeapon? SecondWeapon { get; set; }

    public int LightningResistance {get; set; }
    public int ColdResistance { get; set; }
    public int FireResistance { get; set; }
    public int HealthRune { get; set; }

    public bool HasWeaponScroll { get; set; }
    public bool HasEternityPotion { get; set; }

    public int GladiatorLevel { get; set; }
    public double SoloPortal { get; set; }
    public double GuildPortal { get; set; }
    public double CritMultiplier => 2 + 0.11 * GladiatorLevel + (HasWeaponScroll ? 0.05 : 0);
    public int Reaction { get; set; } 
    public long Health => this.GetHealth();

    public RawCompanion[] Companions { get; set; } = default!;
}
