namespace SFSimulator.Core;

public class EndpointDataDTO
{
    public string CharacterName { get; set; } = null!;
    public int Level { get; set; } = 1;
    public int Experience { get; set; } = 0;

    public int BaseStat { get; set; } = 0;
    public int GoldPitLevel { get; set; } = 0;
    public int AcademyLevel { get; set; } = 0;
    public int GemMineLevel { get; set; } = 0;
    public int TreasuryLevel { get; set; } = 0;

    public int HydraHeads { get; set; } = 0;
    public float ScrapbookFillness { get; set; } = 0;
    public int XpGuildBonus { get; set; } = 0;
    public int XpRuneBonus { get; set; } = 0;
    public bool HasExperienceScroll { get; set; } = false;
    public int Tower { get; set; } = 0;
    public int GoldGuildBonus { get; set; } = 0;
    public int GoldRuneBonus { get; set; } = 0;
    public bool HasGoldScroll { get; set; } = false;
    public bool HasArenaGoldScroll { get; set; } = false;
    public MountType MountType { get; set; } = MountType.Griffin; 
    public int Calendar { get; set; }
    public int CalendarDay { get; set; }

    public int SoloPortal { get; set; }
    public int GuildPortal { get; set; }
    public int GladiatorLevel { get; set; }
    public ClassType Class { get; set; }
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    public int Constitution { get; set; }
    public int Luck { get; set; }
    public List<Potion> Potions { get; set; } = default!;
    public List<EquipmentItem> Items { get; set; } = default!;
    public EquipmentWeapon? FirstWeapon { get; set; }
    public EquipmentWeapon? SecondWeapon { get; set; }
}
