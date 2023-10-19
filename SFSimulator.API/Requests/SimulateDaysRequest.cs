using SFSimulator.Core;
using Mount = SFSimulator.Core.MountType;

namespace SFSimulator.API.Requests;

public class SimulateDaysRequest
{
    public int DaysCount { get; set; }
    public int Level { get; set; }
    public long Experience { get; set; }
    public int BaseStat { get; set; }
    public int GoldPitLevel { get; set; } = 0;
    public int AcademyLevel { get; set; } = 0;
    public int HydraHeads { get; set; } = 0;
    public int GemMineLevel { get; set; } = 0;
    public int TreasuryLevel { get; set; } = 0;
    public string? QuestPriority { get; set; } = null;
    public float? HybridRatio { get; set; } = null;
    public bool SwitchPriority { get; set; } = false;
    public int? SwitchLevel { get; set; } = null;
    public string? PriorityAfterSwitch { get; set; } = null;
    public float ScrapbookFillness { get; set; } = 0;
    public int XpGuildBonus { get; set; } = 0;
    public int XpRuneBonus { get; set; } = 0;
    public bool HasExperienceScroll { get; set; } = false;
    public int Tower { get; set; } = 0;
    public int GoldGuildBonus { get; set; } = 0;
    public int GoldRuneBonus { get; set; } = 0;
    public bool HasGoldScroll { get; set; } = false;
    public bool DrinkBeerOneByOne { get; set; } = false;
    public int DailyThirst { get; set; } = 0;
    public string MountType { get; set; } = nameof(Mount.Griffin);
    public bool SkipCalendar { get; set; } = false;
    public string SpinAmount { get; set; } = nameof(SpinAmountType.Max);
    public float DailyGuard { get; set; } = 0;
    public Schedule Schedule { get; set; } = new Schedule();
    public bool SimulateDungeon { get; set; } = false;
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
    public int Calendar { get; set; }
    public int CalendarDay { get; set; }
}
