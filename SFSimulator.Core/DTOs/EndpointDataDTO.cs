namespace SFSimulator.Core
{
    public class EndpointDataDTO
    {
        public string CharacterName { get; set; } = null!;
        public string Prefix { get; set; } = null!;
        public int Level { get; set; } = 1;
        public int BaseStat { get; set; } = 0;
        public int Experience { get; set; } = 0;
        public int GoldPitLevel { get; set; } = 0;
        public int AcademyLevel { get; set; } = 0;
        public int HydraHeads { get; set; } = 0;
        public int GemMineLevel { get; set; } = 0;
        public int TreasuryLevel { get; set; } = 0;
        public float ScrapbookFillness { get; set; } = 0;
        public int XpGuildBonus { get; set; } = 0;
        public int XpRuneBonus { get; set; } = 0;
        public bool HasExperienceScroll { get; set; } = false;
        public int Tower { get; set; } = 0;
        public int GoldGuildBonus { get; set; } = 0;
        public int GoldRuneBonus { get; set; } = 0;
        public bool HasGoldScroll { get; set; } = false;
        public string MountType { get; set; } = "Griffin";
    }
}
