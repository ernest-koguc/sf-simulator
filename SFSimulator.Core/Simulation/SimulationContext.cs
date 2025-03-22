using System.Text.Json;

namespace SFSimulator.Core;

public class SimulationContext : IFightable<EquipmentItem>, IHealthCalculatable, ITotalStatsCalculatable
{
    public bool SwitchPriority { get; set; } = false;
    public int? SwitchLevel { get; set; } = null;
    public GoldBonus GoldBonus { get; set; } = new();
    public ExperienceBonus ExperienceBonus { get; set; } = new();
    public int DailyThirst { get; set; } = 320;
    public bool SkipCalendar { get; set; } = true;
    public SpinAmountType SpinAmount { get; set; } = SpinAmountType.Max;
    public decimal DailyGuard { get; set; } = 23;
    public Dictionary<(int Week, int Day), ScheduleDay> Schedule = new();
    public int HydraHeads { get; set; } = 0;
    public int GoldPitLevel { get; set; } = 0;
    public int AcademyLevel { get; set; } = 0;
    public int GemMineLevel { get; set; } = 0;
    public int TreasuryLevel { get; set; } = 0;
    public MountType Mount { get; set; } = MountType.Griffin;
    public int Calendar { get; set; } = 1;
    public int CalendarDay { get; set; } = 1;
    public WeeklyTasksOptions WeeklyTasksOptions { get; set; } = new(true, true);
    public ExpeditionOptions ExpeditionOptions { get; set; } = new(1.28M, 1.2M);
    public ExpeditionOptions ExpeditionOptionsAfterSwitch { get; set; } = new(1.28M, 1.2M);
    public int ScrollsUnlocked { get; set; } = 9;
    public bool DoDungeons { get; set; }
    public DungeonProgressionOptions DungeonOptions { get; set; } = new()
    {
        InstaKillPercentage = 1,
        DungeonIterations = 1000,
    };
    public ReequipOptions ReequipOptions { get; set; } = new()
    {
        ChangeGear = true,
        CharacterReequipLevelOffset = 10,
        CharacterLevelOnLastEquipmentChange = -1,
        CompanionReequipLevelOffset = 10,
        CompanionLevelOnLastEquipmentChange = -1,
        PreferredWeaponRange = 1.5D,
    };
    public int GuildKnights { get; set; }
    public int GuildRaids { get; set; } = 0;
    public int RuneQuantity { get; set; }
    public int Level { get; set; } = 1;
    public ClassType Class { get; set; } = ClassType.Warrior;
    public long Experience { get; set; }
    public int BaseStat
    {
        get => BaseConstitution + BaseMainAttribute;
    }
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
    public int Armor => Items.Sum(i => i.Armor);

    public EquipmentItem? FirstWeapon => Items.FirstOrDefault(e => e.ItemType == ItemType.Weapon);
    public EquipmentItem? SecondWeapon => Class == ClassType.Assassin && Items.Count(i => i.ItemType == ItemType.Weapon) > 1 ? Items.Last(i => i.ItemType == ItemType.Weapon) : null;
    public List<Potion> Potions { get; set; } = [];
    public List<EquipmentItem> Items { get; set; } = [];

    public int LightningResistance => Items.Where(i => i.RuneType is RuneType.LightningResistance or RuneType.TotalResistance).Sum(i => i.RuneValue);
    public int FireResistance => Items.Where(i => i.RuneType is RuneType.FireResistance or RuneType.TotalResistance).Sum(i => i.RuneValue);
    public int ColdResistance => Items.Where(i => i.RuneType is RuneType.ColdResistance or RuneType.TotalResistance).Sum(i => i.RuneValue);
    public int HealthRune => Items.Where(i => i.RuneType == RuneType.HealthBonus).Sum(i => i.RuneValue);

    public bool HasWeaponScroll => FirstWeapon?.HasEnchantment == true || SecondWeapon?.HasEnchantment == true;
    public bool HasEternityPotion => Potions.Any(p => p.Type == PotionType.Eternity);

    public int GladiatorLevel { get; set; }
    public double SoloPortal { get; set; }
    public double GuildPortal { get; set; }
    public double CritMultiplier => 2 + (0.11 * GladiatorLevel) + (HasWeaponScroll ? 0.05 : 0);
    public int Reaction => Items.Any(i => i.ScrollType == WitchScrollType.Reaction) ? 1 : 0;
    public long Health => this.GetHealth();

    public Companion[] Companions { get; set; } = [];

    public bool SellPetFood { get; set; } = true;
    public bool DoPetsDungeons { get; set; } = false;
    public PetsState Pets { get; set; } = new();
    public int Aura { get; set; }
    public int AuraFillLevel { get; set; }
    public AuraProgressStrategyType AuraStrategy { get; set; } = AuraProgressStrategyType.OnlyEpicItems;
    public BlackSmithResources BlackSmithResources { get; set; } = new(0, 0);
    public SFToolsDungeonData? DungeonsData { get; set; }
    public SimulationFinishCondition FinishCondition { get; set; } = new();
    public BaseStatsIncreaseStrategyType BaseStatsIncreaseStrategy { get; set; } = BaseStatsIncreaseStrategyType.Keep_50_50_until_same_cost_then_60_40;

    public int BaseMainAttribute => ClassConfigurationProvider.GetClassConfiguration(Class).MainAttribute switch
    {
        AttributeType.Strength => BaseStrength,
        AttributeType.Dexterity => BaseDexterity,
        AttributeType.Intelligence => BaseIntelligence,
        AttributeType.Constitution => BaseConstitution,
        AttributeType.Luck => BaseLuck,
        _ => throw new ArgumentOutOfRangeException($"{Class}'s main attribute is not supported")
    };

    public void SetBaseMainAttribute(int value)
    {
        var attribute = ClassConfigurationProvider.GetClassConfiguration(Class).MainAttribute;
        switch (attribute)
        {
            case AttributeType.Strength:
                BaseStrength = value;
                break;
            case AttributeType.Dexterity:
                BaseDexterity = value;
                break;
            case AttributeType.Intelligence:
                BaseIntelligence = value;
                break;
            case AttributeType.Constitution:
                BaseConstitution = value;
                break;
            case AttributeType.Luck:
                BaseLuck = value;
                break;
            default:
                throw new ArgumentOutOfRangeException($"{Class}'s main attribute is not supported");
        }
    }

    public int TotalMainAttribute => this.GetTotalStatsFor(ClassConfigurationProvider.GetClassConfiguration(Class).MainAttribute);

    public int this[AttributeType attribute]
    {
        get => attribute switch
        {
            AttributeType.Strength => BaseStrength,
            AttributeType.Dexterity => BaseDexterity,
            AttributeType.Intelligence => BaseIntelligence,
            AttributeType.Constitution => BaseConstitution,
            AttributeType.Luck => BaseLuck,
            _ => throw new ArgumentOutOfRangeException($"{attribute} is not a supported attribute")
        };
        set
        {
            switch (attribute)
            {
                case AttributeType.Strength:
                    BaseStrength = value;
                    break;
                case AttributeType.Dexterity:
                    BaseDexterity = value;
                    break;
                case AttributeType.Intelligence:
                    BaseIntelligence = value;
                    break;
                case AttributeType.Constitution:
                    BaseConstitution = value;
                    break;
                case AttributeType.Luck:
                    BaseLuck = value;
                    break;
                default: throw new ArgumentOutOfRangeException($"{attribute} is not a supported attribute");
            }
        }
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
}

public class WeeklyTasksOptions(bool DoWeeklyTasks, bool DrinkExtraBeer)
{
    public WeeklyTasksOptions() : this(false, false) { }
    public bool DoWeeklyTasks { get; set; } = DoWeeklyTasks;
    public bool DrinkExtraBeer { get; set; } = DrinkExtraBeer;
}

public class CompanionSimple
{
    public List<EquipmentItem> Items { get; set; } = default!;
    public ClassType Class { get; set; }
}