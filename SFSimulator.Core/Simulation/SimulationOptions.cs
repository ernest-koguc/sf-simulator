namespace SFSimulator.Core;

public class SimulationOptions
{
    public bool SwitchPriority { get; set; } = false;
    public int? SwitchLevel { get; set; } = null;
    public GoldBonus GoldBonus { get; set; } = null!;
    public ExperienceBonus ExperienceBonus { get; set; } = null!;
    public bool DrinkBeerOneByOne { get; set; } = false;
    public int DailyThirst { get; set; } = 320;
    public bool SkipCalendar { get; set; } = false;
    public SpinAmountType SpinAmount { get; set; } = SpinAmountType.Max;
    public decimal DailyGuard { get; set; } = 23;
    public bool SimulateDungeon { get; set; } = false;
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
    public ExpeditionOptions? ExpeditionOptions { get; set; }
    public ExpeditionOptions? ExpeditionOptionsAfterSwitch { get; set; }
    public QuestOptions? QuestOptions { get; set; }
    public QuestOptions? QuestOptionsAfterSwitch { get; set; }
    public bool ExpeditionsInsteadOfQuests { get; set; } = true;
}

public record WeeklyTasksOptions(bool DoWeeklyTasks, bool DrinkExtraBeer);
