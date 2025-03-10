namespace SFSimulator.Core;
public class PetFightable : IFightable<RawWeapon>, IHealthCalculatable
{
    public required ClassType Class { get; set; }
    public required int Level { get; set; }
    public required int Strength { get; set; }
    public required int Dexterity { get; set; }
    public required int Intelligence { get; set; }
    public required int Constitution { get; set; }
    public required int Luck { get; set; }
    public long Health => this.GetHealth();
    public required RawWeapon? FirstWeapon { get; set; }
    public RawWeapon? SecondWeapon => default;
    public int Reaction => 0;
    public required double CritMultiplier { get; set; }
    public int LightningResistance => 0;
    public int FireResistance => 0;
    public int ColdResistance => 0;
    public int HealthRune => 0;
    public double SoloPortal => 0;
    public double GuildPortal => 0;
    public bool HasEternityPotion => false;
    public required int Armor { get; set; }
    public required int Position { get; set; }
    public required PetElementType ElementType { get; set; }
}
