namespace SFSimulator.Core;

public class GoldBonus(int tower = 100, int guildBonus = 200, int runeBonus = 50, bool questScroll = true)
{
    public int Tower { get; set; } = tower;
    public decimal GuildBonus { get; set; } = guildBonus;
    public int RuneBonus { get; set; } = runeBonus;
    public bool HasGoldScroll { get; set; } = questScroll;
    // Empty ctor for serialization
    public GoldBonus() : this(100, 200, 50, true) { }

    public decimal TowerPlusGuildBonus => 1 + ((Tower + GuildBonus) / 100M);
}