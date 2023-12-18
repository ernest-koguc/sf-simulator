namespace SFSimulator.Core
{
    public class GoldBonus
    {
        public decimal Tower { get; set; }
        public decimal GuildBonus { get; set; }
        public decimal RuneBonus { get; set; }
        public bool HasGoldScroll { get; set; }
        public bool HasArenaGoldScroll { get; set; }
        public GoldBonus(int tower = 100, int guildBonus = 200, int runeBonus = 50, bool questScroll = true, bool arenaScroll = true)
        {
            Tower = tower / 100M;
            GuildBonus = guildBonus / 100M;
            RuneBonus = runeBonus / 100M;
            HasGoldScroll = questScroll;
            HasArenaGoldScroll = arenaScroll;
        }
        public decimal CombinedBonus() => 1 + Tower + GuildBonus;
    }
}
