namespace SFSimulator.Core
{
    public class GoldBonus
    {
        public decimal Tower { get; set; } = 0;
        public decimal GuildBonus { get; set; } = 0;
        public decimal RuneBonus { get; set; } = 0;
        public bool HasGoldScroll { get; set; } = false;
        public GoldBonus(int tower = 100, int guildBonus = 200, int runeBonus = 50, bool scroll = true)
        {
            Tower = tower / 100M;
            GuildBonus = guildBonus / 100M;
            RuneBonus = runeBonus / 100M;
            HasGoldScroll = scroll;
        }
        public decimal CombinedBonus() => 1 + Tower + GuildBonus;
    }
}
