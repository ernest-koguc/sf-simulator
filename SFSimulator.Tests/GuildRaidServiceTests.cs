using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System;
using System.Collections.Generic;

namespace SFSimulator.Tests;

[TestClass]
public class GuildRaidServiceTests
{
    [TestMethod]
    public void SetUpGuildRaidsState_throws_error_if_there_is_no_sufficient_amount_of_solo_portal_levels()
    {
        var guildRaidService = DependencyProvider.Get<IGuildRaidService>();
        var simulationContext = new SimulationContext();
        simulationContext.GuildRaids = 0;

        Assert.ThrowsException<ArgumentException>(() =>
        {
            guildRaidService.SetUpGuildRaidsState(simulationContext, []);
        });
    }

    [TestMethod]
    public void Progress_increases_the_guild_raid_level_when_requirements_are_met()
    {
        var guildRaidService = DependencyProvider.Get<IGuildRaidService>();
        var simulationContext = new SimulationContext();
        simulationContext.GuildRaids = 49;
        List<GuildRaidRequirements> soloPortalRequirements = [new(50, 10, 300)];
        guildRaidService.SetUpGuildRaidsState(simulationContext, soloPortalRequirements);

        guildRaidService.Progress(1, simulationContext);
        Assert.AreEqual(49, simulationContext.GuildRaids, "Guild raid level should not increase when requirements are not met");

        guildRaidService.Progress(10, simulationContext);
        Assert.AreEqual(49, simulationContext.GuildRaids, "Guild raid level should not increase when only minimum days requirement is met");

        simulationContext.Level = 300;
        guildRaidService.Progress(1, simulationContext);
        Assert.AreEqual(49, simulationContext.GuildRaids, "Guild raid level should not increase when only minimum requirement is met");

        guildRaidService.Progress(10, simulationContext);
        Assert.AreEqual(50, simulationContext.GuildRaids, "Guild raid level should increase when requirements are met");
    }
}
