namespace QuestSimulator.Characters
{
    public class GoldBonus
    {
        public float Tower { get; set; } = 0;
        public float GuildBonus { get; set; } = 0;
        public float RuneBonus { get; set; } = 0;
        public bool HasGoldScroll { get; set; } = false;
        public GoldBonus(float tower = 100, float guildBonus = 200, float runeBonus = 50, bool scroll = true)
        {
            Tower = tower/100;
            GuildBonus = guildBonus/100;
            RuneBonus = runeBonus/100;
            HasGoldScroll = scroll;
        }
        public float CombinedBonus() => (1 + Tower + GuildBonus);
    }
}
