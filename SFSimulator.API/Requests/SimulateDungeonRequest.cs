using SFSimulator.Core;

namespace SFSimulator.API.Requests;

public class SimulateDungeonRequest
{
    public int DungeonPosition { get; set; }
    public int DungeonEnemyPosition { get; set; }
    public int Iterations { get; set; }
    public int WinTreshold { get; set; }
    public int Level { get; set; }
    public ClassType Class { get; set; }
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    public int Constitution { get; set; }
    public int Luck { get; set; }
    public int Armor { get; set; }
    public RawWeapon? FirstWeapon { get; set; }
    public RawWeapon? SecondWeapon { get; set; }
    public bool HasGlovesScroll { get; set; }
    public bool HasWeaponScroll { get; set; }
    public bool HasEternityPotion { get; set; }
    public int Reaction { get; set; }
    public int GladiatorLevel { get; set; }
    public int SoloPortal { get; set; }
    public int GuildPortal { get; set; }
    public int LightningResistance { get; set; }
    public int ColdResistance { get; set; }
    public int FireResistance { get; set; }
    public int HealthRune { get; set; }
}