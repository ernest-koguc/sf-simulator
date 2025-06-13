using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System;
using System.Collections.Generic;

namespace SFSimulator.Tests;

[TestClass]
public class PortalServiceTests
{
    [TestMethod]
    public void SetUpPortalState_throws_error_if_there_is_no_sufficient_amount_of_solo_portal_levels()
    {
        var portalService = DependencyProvider.Get<IPortalService>();
        var simulationContext = new SimulationContext();
        simulationContext.SoloPortal = 0;
        simulationContext.GuildPortal = 0;

        Assert.ThrowsException<ArgumentException>(() =>
        {
            portalService.SetUpPortalState(simulationContext, [], []);
        });
    }

    [TestMethod]
    public void SetUpPortalState_throws_error_if_there_is_no_sufficient_amount_of_guild_portal_levels()
    {
        var portalService = DependencyProvider.Get<IPortalService>();
        var simulationContext = new SimulationContext();
        simulationContext.SoloPortal = 50;
        simulationContext.GuildPortal = 0;

        Assert.ThrowsException<ArgumentException>(() =>
        {
            portalService.SetUpPortalState(simulationContext, [], []);
        });
    }

    [TestMethod]
    public void Progress_increases_the_solo_portal_level_when_requirements_are_met()
    {
        var portalService = DependencyProvider.Get<IPortalService>();
        var simulationContext = new SimulationContext();
        simulationContext.SoloPortal = 49;
        simulationContext.GuildPortal = 50;
        List<PortalRequirements> soloPortalRequirements = [new(50, 10, 300)];
        portalService.SetUpPortalState(simulationContext, soloPortalRequirements, []);

        portalService.Progress(1, simulationContext);
        Assert.AreEqual(49, simulationContext.SoloPortal, "Solo portal level should not increase when requirements are not met");

        portalService.Progress(10, simulationContext);
        Assert.AreEqual(49, simulationContext.SoloPortal, "Solo portal level should not increase when only minimum days requirement is met");

        simulationContext.Level = 300;
        portalService.Progress(1, simulationContext);
        Assert.AreEqual(49, simulationContext.SoloPortal, "Solo portal level should not increase when only minimum requirement is met");

        portalService.Progress(10, simulationContext);
        Assert.AreEqual(50, simulationContext.SoloPortal, "Solo portal level should increase when requirements are met");
    }

    [TestMethod]
    public void Progress_increases_the_guild_portal_level_when_requirements_are_met()
    {
        var portalService = DependencyProvider.Get<IPortalService>();
        var simulationContext = new SimulationContext();
        simulationContext.SoloPortal = 50;
        simulationContext.GuildPortal = 49;
        List<PortalRequirements> guildPortalRequirements = [new(50, 10, 300)];
        portalService.SetUpPortalState(simulationContext, [], guildPortalRequirements);

        portalService.Progress(1, simulationContext);
        Assert.AreEqual(49, simulationContext.GuildPortal, "Solo portal level should not increase when requirements are not met");

        portalService.Progress(10, simulationContext);
        Assert.AreEqual(49, simulationContext.GuildPortal, "Solo portal level should not increase when only minimum days requirement is met");

        simulationContext.Level = 300;
        portalService.Progress(1, simulationContext);
        Assert.AreEqual(49, simulationContext.GuildPortal, "Solo portal level should not increase when only minimum requirement is met");

        portalService.Progress(10, simulationContext);
        Assert.AreEqual(50, simulationContext.GuildPortal, "Solo portal level should increase when requirements are met");
    }
}
