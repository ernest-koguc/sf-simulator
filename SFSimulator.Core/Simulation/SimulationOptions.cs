using QuestSimulator.Characters;
using QuestSimulator.Enums;
using QuestSimulator.Quests;

namespace QuestSimulator.Simulation
{
    public class SimulationOptions
    {
        public Priority QuestPriority { get; set; } = Priority.XP;
        public float? HybridRatio { get; set; } = null;
        public QuestChooserAI QuestChooserAI { get; set; } = QuestChooserAI.SMART;
        public bool SwitchPriority { get; set; } = false;
        public int? SwitchLevel { get; set; } = null;
        public Priority PriorityAfterSwitch { get; set; } = Priority.GOLD;
        public GoldBonus GoldBonus { get; set; } = null!;
        public ExperienceBonus ExperienceBonus { get; set; } = null!;
        public bool DrinkBeerOneByOne { get; set; } = false;
        public int DailyThirst { get; set; } = 0;
        public bool SkipCalendar { get; set; } = false;

        #region DUNG BREAK EXPERIMENTAL
        public int? SavedDungeonsStart { get; set; } = null;
        public int SavedLevels { get; set; } = 0;
        #endregion
    }
}
