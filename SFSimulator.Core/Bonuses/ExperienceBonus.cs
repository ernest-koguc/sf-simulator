namespace SFSimulator.Core;

public class ExperienceBonus
{
    public decimal ScrapbookFillness { get; set; } = 0;
    public decimal GuildBonus { get; set; } = 0;
    public int RuneBonus { get; set; } = 0;
    public bool HasExperienceScroll { get; set; } = false;
    // Empty ctor for serialization
    public ExperienceBonus() : this(100, 200, 10, true) { }
    public ExperienceBonus(decimal scrapbook = 100, decimal guild = 200, int runeBonus = 10, bool scroll = true)
    {
        ScrapbookFillness = scrapbook;
        GuildBonus = guild;
        RuneBonus = runeBonus;
        HasExperienceScroll = scroll;
    }
    public decimal CombinedBonus => ScrapbookPlusGuildPlusRuneBonus * (HasExperienceScroll ? 1.1M : 1);
    public decimal ScrapbookPlusGuildPlusRuneBonus => (1 + ((ScrapbookFillness + GuildBonus) / 100M)) * (1 + (RuneBonus / 100M));
}