namespace SFSimulator.Core;
public class GuildRaidService : IGuildRaidService
{
    private List<GuildRaidRequirements> GuildRaidRequirements { get; set; } = [];
    public void SetUpGuildRaidsState(SimulationContext simulationContext, List<GuildRaidRequirements>? guildRaidRequirements = null)
    {
        GuildRaidRequirements = guildRaidRequirements ??= GetDefaultGuildRaidRequirements();
        if (guildRaidRequirements.Count < 50 - simulationContext.GuildRaids)
        {
            throw new ArgumentException("Insufficeint amount of guild raid levels", nameof(guildRaidRequirements));
        }

        var guildRaid = simulationContext.GuildRaids;
        var currentGuildRaidRequirement = GuildRaidRequirements.FirstOrDefault(r => r.GuildRaidLevel == guildRaid);
        GuildRaidRequirements = GuildRaidRequirements
            .SkipWhile(r => r.GuildRaidLevel <= guildRaid)
            .Select(r => r with { MinimumDays = r.MinimumDays - currentGuildRaidRequirement.MinimumDays })
            .ToList();
    }

    public void Progress(int currentDay, SimulationContext simulationContext, Action<int>? addGuildRaidPictures = null)
    {
        var guildRaids = simulationContext.GuildRaids + 1;
        var newGuildRaidLevel = GuildRaidRequirements
            .FirstOrDefault(r => r.GuildRaidLevel == guildRaids && r.MinimumDays <= currentDay && r.MinimumLevel <= simulationContext.Level);
        if (newGuildRaidLevel != default)
        {
            var difference = newGuildRaidLevel.GuildRaidLevel - simulationContext.GuildRaids;
            simulationContext.GuildRaids = newGuildRaidLevel.GuildRaidLevel;
            GuildRaidRequirements.Remove(newGuildRaidLevel);

            if (difference <= 0)
            {
                return;
            }

            if (addGuildRaidPictures is not null)
            {
                addGuildRaidPictures(difference);
            }

            if (simulationContext.GuildRaids <= 50)
            {
                var newExpGuildBonus = simulationContext.ExperienceBonus.GuildBonus + (2 * difference);
                simulationContext.ExperienceBonus.GuildBonus = Math.Min(200, newExpGuildBonus);
                var newGoldGuildBonus = simulationContext.GoldBonus.GuildBonus + (2 * difference);
                simulationContext.GoldBonus.GuildBonus = Math.Min(200, newGoldGuildBonus);
            }
        }
    }

    private static List<GuildRaidRequirements> GetDefaultGuildRaidRequirements() =>
    [
        new (1, 2, 50),
        new (2, 2,55),
        new (3, 2, 60),
        new (4, 3, 65),
        new (5, 3, 70),
        new (6, 3, 75),
        new (7, 4, 80),
        new (8, 4, 85),
        new (9, 4, 90),
        new (10, 5, 95),
        new (11, 5, 100),
        new (12, 5, 105),
        new (13, 6, 110),
        new (14, 6, 115),
        new (15, 6, 120),
        new (16, 7, 125),
        new (17, 7, 130),
        new (18, 7, 135),
        new (19, 8, 140),
        new (20, 8, 145),
        new (21, 9, 150),
        new (22, 9, 155),
        new (23, 10, 160),
        new (24, 10, 165),
        new (25, 11, 170),
        new (26, 12, 174),
        new (27, 12, 178),
        new (28, 13, 182),
        new (29, 13, 186),
        new (30, 14, 190),
        new (31, 14, 194),
        new (32, 15, 198),
        new (33, 15, 202),
        new (34, 16, 206),
        new (35, 16, 210),
        new (36, 18, 214),
        new (37, 18, 218),
        new (38, 19, 222),
        new (39, 19, 226),
        new (40, 20, 230),
        new (41, 21, 234),
        new (42, 22, 238),
        new (43, 23, 242),
        new (44, 24, 246),
        new (45, 25, 250),
        new (46, 26, 254),
        new (47, 27, 258),
        new (48, 28, 262),
        new (49, 29, 266),
        new (50, 30, 270)
    ];
}

public readonly record struct GuildRaidRequirements(int GuildRaidLevel, int MinimumDays, int MinimumLevel);
