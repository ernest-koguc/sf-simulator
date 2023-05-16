using System.ComponentModel.DataAnnotations;

namespace SFSimulator.Core
{
    public class SimulationOptionsDTO
    {
        public string CharacterName { get; set; } = null!;
        public int Level { get; set; } = 1;
        public int BaseStat { get; set; } = 0;
        public int Experience { get; set; } = 0;
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
        public string MountType { get; set; } = "Griffin";
        public bool SkipCalendar { get; set; } = false;
    }
}
