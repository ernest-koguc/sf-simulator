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
        List<GuildRaidRequirements> guildRaidRequirements = [new(50, 10, 300)];
        guildRaidService.SetUpGuildRaidsState(simulationContext, guildRaidRequirements);

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

    [TestMethod]
    public void Progress_increases_the_guild_bonuses()
    {
        var guildRaidService = DependencyProvider.Get<IGuildRaidService>();
        var simulationContext = new SimulationContext();
        simulationContext.GuildRaids = 49;
        simulationContext.Level = 300;
        simulationContext.GoldBonus.GuildBonus = 98;
        simulationContext.ExperienceBonus.GuildBonus = 98;

        List<GuildRaidRequirements> guildRaidRequirements = [new(50, 10, 300)];
        guildRaidService.SetUpGuildRaidsState(simulationContext, guildRaidRequirements);

        guildRaidService.Progress(10, simulationContext);
        Assert.AreEqual(100, simulationContext.GoldBonus.GuildBonus);
        Assert.AreEqual(100, simulationContext.ExperienceBonus.GuildBonus);
    }

    [TestMethod]
    public void Progress_does_not_increase_guild_bonus_over_the_cap()
    {
        var guildRaidService = DependencyProvider.Get<IGuildRaidService>();
        var simulationContext = new SimulationContext();
        simulationContext.GuildRaids = 49;
        simulationContext.Level = 300;
        simulationContext.GoldBonus.GuildBonus = 200;
        simulationContext.ExperienceBonus.GuildBonus = 200;

        List<GuildRaidRequirements> guildRaidRequirements = [new(50, 10, 300)];
        guildRaidService.SetUpGuildRaidsState(simulationContext, guildRaidRequirements);

        guildRaidService.Progress(10, simulationContext);
        Assert.AreEqual(200, simulationContext.GoldBonus.GuildBonus);
        Assert.AreEqual(200, simulationContext.ExperienceBonus.GuildBonus);
    }
}
