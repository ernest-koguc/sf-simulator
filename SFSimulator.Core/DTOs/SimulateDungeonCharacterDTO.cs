namespace SFSimulator.Core;

public class SimulateDungeonCharacterDTO
{
    public ClassType Class { get; set; }
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    public int Constitution { get; set; }
    public int Luck { get; set; }
    public int Armor { get; set; }
    public Weapon? FirstWeapon { get; set; }
    public Weapon? SecondWeapon { get; set; }
    public ResistanceRuneBonuses RuneBonuses { get; set; } = new ResistanceRuneBonuses();
    public bool HasGlovesScroll { get; set; }
    public bool HasWeaponScroll { get; set; }
    public bool HasEternityPotion { get; set; }
    public int GladiatorLevel { get; set; }
    public int SoloPortal { get; set; }
    public int GuildPortal { get; set; }
}
