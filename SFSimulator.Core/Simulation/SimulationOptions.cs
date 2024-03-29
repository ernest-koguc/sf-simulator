namespace SFSimulator.Core;

public class SimulationOptions
{
    public QuestPriorityType QuestPriority { get; set; } = QuestPriorityType.Experience;
    public decimal? HybridRatio { get; set; } = null;
    public QuestChooserAI QuestChooserAI { get; set; } = QuestChooserAI.Smart;
    public bool SwitchPriority { get; set; } = false;
    public int? SwitchLevel { get; set; } = null;
    public QuestPriorityType PriorityAfterSwitch { get; set; } = QuestPriorityType.Gold;
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
    public WeeklyTasksOptions WeeklyTasksOptions { get; set; } = new (true, true);
}

public record WeeklyTasksOptions(bool DoWeeklyTasks, bool DrinkExtraBeer);
