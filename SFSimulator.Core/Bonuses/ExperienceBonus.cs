namespace SFSimulator.Core
{
    public class ExperienceBonus
    {
        public float ScrapbookFillness { get; set; } = 0;
        public float GuildBonus { get; set; } = 0;
        public float RuneBonus { get; set; } = 0;
        public bool HasExperienceScroll { get; set; } = false;
        public ExperienceBonus(float scrapbook = 95, float guild = 200, float runeBonus = 10, bool scroll = true)
        {
            ScrapbookFillness = scrapbook / 100;
            GuildBonus = guild / 100;
            RuneBonus = runeBonus / 100;
            HasExperienceScroll = scroll;
        }
        public float CombinedBonus() => HasExperienceScroll ? (1 + ScrapbookFillness + GuildBonus) * (1 + RuneBonus) * 1.1f : (1 + ScrapbookFillness + GuildBonus) * (1 + RuneBonus);
    }
}
