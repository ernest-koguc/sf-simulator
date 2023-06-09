﻿namespace SFSimulator.Core
{
    public class SimulationOptions
    {
        public string CharacterName { get; set; } = string.Empty;
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

        public Dictionary<(int Week, int Day), ScheduleDay> Schedule = new();

        #region DUNG BREAK EXPERIMENTAL
        public int? SavedDungeonsStart { get; set; } = null;
        public int SavedLevels { get; set; } = 0;
        #endregion
    }
}
