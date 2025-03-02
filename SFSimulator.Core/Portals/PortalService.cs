namespace SFSimulator.Core;
public class PortalService : IPortalService
{
    private List<PortalRequirements> SoloPortalRequirements { get; set; } = [];
    private List<PortalRequirements> GuildPortalRequirements { get; set; } = [];

    public void SetUpPortalState(SimulationContext simulationContext, List<PortalRequirements>? soloPortalRequirements = null, List<PortalRequirements>? guildPortalRequirements = null)
    {
        SoloPortalRequirements = soloPortalRequirements ??= GetDefaultSoloPortalRequirements();
        GuildPortalRequirements = guildPortalRequirements ??= GetDefaultGuildPortalRequirements();
        if (soloPortalRequirements.Count < 50 - simulationContext.SoloPortal)
        {
            throw new ArgumentException("Insufficeint amount of portal levels", nameof(soloPortalRequirements));
        }

        if (guildPortalRequirements.Count < 50 - simulationContext.GuildPortal)
        {
            throw new ArgumentException("Insufficeint amount of portal levels", nameof(guildPortalRequirements));
        }

        var soloPortal = (int)simulationContext.SoloPortal;
        SoloPortalRequirements = SoloPortalRequirements
            .SkipWhile(e => e.PortalLevel < soloPortal)
            .Iterate((prev, next) => next with { MinimumDays = next.MinimumDays - prev.MinimumDays })
            .ToList();

        var guildPortal = (int)simulationContext.GuildPortal;
        GuildPortalRequirements = GuildPortalRequirements
            .SkipWhile(e => e.PortalLevel < guildPortal)
            .Iterate((prev, next) => next with { MinimumDays = next.MinimumDays - prev.MinimumDays })
            .ToList();
    }

    public void Progress(int days, SimulationContext simulationContext)
    {
        var soloPortal = (int)simulationContext.SoloPortal + 1;
        var newSoloPortal = SoloPortalRequirements.FirstOrDefault(r => r.PortalLevel == soloPortal && r.MinimumDays <= days && r.MinimumLevel <= simulationContext.Level);
        if (newSoloPortal != default)
        {
            simulationContext.SoloPortal = newSoloPortal.PortalLevel;
            SoloPortalRequirements.Remove(newSoloPortal);
        }

        var guildPortal = (int)simulationContext.GuildPortal + 1;
        var newGuildPortal = GuildPortalRequirements.FirstOrDefault(r => r.PortalLevel == guildPortal && r.MinimumDays <= days && r.MinimumLevel <= simulationContext.Level);
        if (newGuildPortal != default)
        {
            simulationContext.GuildPortal = newGuildPortal.PortalLevel;
            GuildPortalRequirements.Remove(newGuildPortal);
        }
    }

    #region Default Guild Portal
    private List<PortalRequirements> GetDefaultGuildPortalRequirements() =>
    [
        new (1, 1, 160),
        new (2, 3, 165),
        new (3, 6, 170),
        new (4, 9, 175),
        new (5, 12, 180),
        new (6, 15, 185),
        new (7, 18, 185),
        new (8, 21, 190),
        new (9, 24, 195),
        new (10, 27, 200),
        new (11, 30, 205),
        new (12, 33, 210),
        new (13, 36, 215),
        new (14, 39, 220),
        new (15, 42, 225),
        new (16, 45, 230),
        new (17, 48, 235),
        new (18, 51, 240),
        new (19, 54, 245),
        new (20, 57, 250),
        new (21, 60, 255),
        new (22, 63, 260),
        new (23, 66, 265),
        new (24, 69, 270),
        new (25, 72, 275),
        new (26, 75, 280),
        new (27, 78, 285),
        new (28, 81, 290),
        new (29, 84, 295),
        new (30, 87, 300),
        new (31, 90, 305),
        new (32, 93, 310),
        new (33, 96, 315),
        new (34, 99, 320),
        new (35, 102, 325),
        new (36, 105, 330),
        new (37, 108, 335),
        new (38, 111, 340),
        new (39, 114, 345),
        new (40, 117, 350),
        new (41, 120, 355),
        new (42, 123, 360),
        new (43, 126, 365),
        new (44, 129, 370),
        new (45, 132, 375),
        new (46, 135, 380),
        new (47, 138, 385),
        new (48, 141, 390),
        new (49, 144, 395),
        new (50, 147, 400)
    ];
    #endregion

    #region Default Solo Portal
    private List<PortalRequirements> GetDefaultSoloPortalRequirements() =>
    [
        new (1, 2, 160),
        new (2, 4, 165),
        new (3, 6, 170),
        new (4, 8, 175),
        new (5, 10, 180),
        new (6, 12, 185),
        new (7, 14, 185),
        new (8, 16, 190),
        new (9, 18, 195),
        new (10, 20, 200),
        new (11, 22, 205),
        new (12, 24, 210),
        new (13, 26, 215),
        new (14, 28, 220),
        new (15, 30, 225),
        new (16, 32, 230),
        new (17, 34, 235),
        new (18, 36, 240),
        new (19, 38, 245),
        new (20, 40, 250),
        new (21, 42, 255),
        new (22, 44, 260),
        new (23, 46, 265),
        new (24, 48, 270),
        new (25, 50, 275),
        new (26, 52, 280),
        new (27, 54, 285),
        new (28, 56, 290),
        new (29, 58, 295),
        new (30, 60, 300),
        new (31, 62, 305),
        new (32, 64, 310),
        new (33, 66, 315),
        new (34, 68, 320),
        new (35, 70, 325),
        new (36, 72, 330),
        new (37, 74, 335),
        new (38, 76, 340),
        new (39, 78, 345),
        new (40, 80, 350),
        new (41, 82, 355),
        new (42, 84, 360),
        new (43, 86, 365),
        new (44, 88, 370),
        new (45, 90, 375),
        new (46, 92, 380),
        new (47, 94, 385),
        new (48, 96, 390),
        new (49, 98, 395),
        new (50, 100, 400)
    ];
    #endregion
}

public readonly record struct PortalRequirements(int PortalLevel, int MinimumDays, int MinimumLevel);
