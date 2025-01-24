namespace SFSimulator.Core;

public class GoldBonus
{
    public int Tower { get; set; }
    public decimal GuildBonus { get; set; }
    public int RuneBonus { get; set; }
    public bool HasGoldScroll { get; set; }
    public bool HasArenaGoldScroll { get; set; }
    // Empty ctor for serialization
    public GoldBonus() : this(100, 200, 50, true, true) { }
    public GoldBonus(int tower = 100, int guildBonus = 200, int runeBonus = 50, bool questScroll = true, bool arenaScroll = true)
    {
        Tower = tower;
        GuildBonus = guildBonus;
        RuneBonus = runeBonus;
        HasGoldScroll = questScroll;
        HasArenaGoldScroll = arenaScroll;
    }
    public decimal TowerPlusGuildBonus => 1 + ((Tower + GuildBonus) / 100M);
}