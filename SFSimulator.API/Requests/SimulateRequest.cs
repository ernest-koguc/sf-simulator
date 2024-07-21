using SFSimulator.Core;

namespace SFSimulator.API.Requests;

public class SimulateRequest
{
    public int SimulateUntil { get; set; }
    public SimulationFinishCondition Type { get; set; }
    public int Level { get; set; }
    public long Experience { get; set; }
    public int BaseStat { get; set; }
    public int GoldPitLevel { get; set; }
    public int AcademyLevel { get; set; }
    public int HydraHeads { get; set; }
    public int GemMineLevel { get; set; }
    public int TreasuryLevel { get; set; }
    public bool SwitchPriority { get; set; }
    public int? SwitchLevel { get; set; }
    public decimal ScrapbookFillness { get; set; }
    public int XpGuildBonus { get; set; }
    public int XpRuneBonus { get; set; }
    public bool HasExperienceScroll { get; set; }
    public int Tower { get; set; }
    public int GoldGuildBonus { get; set; }
    public int GoldRuneBonus { get; set; }
    public bool HasGoldScroll { get; set; }
    public bool HasArenaGoldScroll { get; set; }
    public bool DrinkBeerOneByOne { get; set; }
    public int DailyThirst { get; set; }
    public MountType MountType { get; set; } = MountType.Griffin;
    public bool SkipCalendar { get; set; }
    public SpinAmountType SpinAmount { get; set; } = SpinAmountType.Max;
    public float DailyGuard { get; set; }
    public Schedule Schedule { get; set; } = new Schedule();
    public int Calendar { get; set; }
    public int CalendarDay { get; set; }
    public int FightsForGold { get; set; }
    public bool DoWeeklyTasks { get; set; }
    public bool DrinkExtraWeeklyBeer { get; set; }
    public bool ExpeditionsInsteadOfQuests { get; set; } = true;
    public QuestOptions? QuestOptions { get; set; }
    public QuestOptions? QuestOptionsAfterSwitch { get; set; }
    public ExpeditionOptions? ExpeditionOptions { get; set; }
    public ExpeditionOptions? ExpeditionOptionsAfterSwitch { get; set; }
    public bool DoDungeons { get; set; }
}