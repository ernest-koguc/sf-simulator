namespace SFSimulator.Core;

public class SimulationOptions : IFightable<EquipmentItem>, IHealthCalculatable, ITotalStatsCalculatable
{
    public bool SwitchPriority { get; set; } = false;
    public int? SwitchLevel { get; set; } = null;
    public GoldBonus GoldBonus { get; set; } = new();
    public ExperienceBonus ExperienceBonus { get; set; } = new();
    public bool DrinkBeerOneByOne { get; set; } = false;
    public int DailyThirst { get; set; } = 320;
    public bool SkipCalendar { get; set; } = false;
    public SpinAmountType SpinAmount { get; set; } = SpinAmountType.Max;
    public decimal DailyGuard { get; set; } = 23;
    public Dictionary<(int Week, int Day), ScheduleDay> Schedule = new();
    public int HydraHeads { get; set; }
    public int GoldPitLevel { get; set; } = 0;
    public int AcademyLevel { get; set; } = 0;
    public int GemMineLevel { get; set; } = 0;
    public int TreasuryLevel { get; set; } = 0;
    public MountType Mount { get; set; } = MountType.Griffin;
    public int Calendar { get; set; }
    public int CalendarDay { get; set; }
    public int FightsForGold { get; set; }
    public WeeklyTasksOptions WeeklyTasksOptions { get; set; } = new(true, true);
    public ExpeditionOptions ExpeditionOptions { get; set; } = new(1.5M, 1.2M);
    public ExpeditionOptions ExpeditionOptionsAfterSwitch { get; set; } = new(1.5M, 1.2M);
    public QuestOptions QuestOptions { get; set; } = new(QuestPriorityType.Experience, 0);
    public QuestOptions QuestOptionsAfterSwitch { get; set; } = new(QuestPriorityType.Experience, 0);
    public bool ExpeditionsInsteadOfQuests { get; set; } = true;
    public int ScrollsUnlocked { get; set; }
    public bool DoDungeons { get; set; }
    public int GuildKnights { get; set; }
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
    public SFToolsDungeonData? DungeonsData { get; set; }
}

public class WeeklyTasksOptions(bool DoWeeklyTasks, bool DrinkExtraBeer)
{
    public bool DoWeeklyTasks { get; set; } = DoWeeklyTasks;
    public bool DrinkExtraBeer { get; set; } = DrinkExtraBeer;
}

public class CompanionSimple
{
    public List<EquipmentItem> Items { get; set; } = default!;
    public ClassType Class { get; set; }
}