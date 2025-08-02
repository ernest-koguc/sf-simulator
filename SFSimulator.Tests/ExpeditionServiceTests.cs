using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System;
using System.Collections.Generic;

namespace SFSimulator.Tests;

[TestClass]
public class ExpeditionServiceTests
{
    private IExpeditionService ExpeditionService { get; set; } = DependencyProvider.Get<IExpeditionService>();

    [TestMethod]
    public void ExpeditionService_DoExpeditions_ReturnsCorrectGold()
    {
        ExpeditionService.Options = new ExpeditionOptions(2M, 1.3M);
        var simulationContext = new SimulationContext
        {
            Level = 632,
            GoldBonus = new GoldBonus(100, 200, 50, true),
            Mount = MountType.Griffin,
        };
        var events = new List<EventType>();
        var totalGold = 0M;
        ExpeditionService.DoExpeditions(simulationContext, events, 320, gold => totalGold += gold, exp => { });

        Assert.AreEqual(1900800000, Math.Round(totalGold));
    }


    [TestMethod]
    [DataRow(503, 118, 8, true, 2254, "1", 25, false, MountType.None, 1699818L)]
    [DataRow(503, 200, 8, true, 2254, "1", 25, false, MountType.None, 2134114L)]
    [DataRow(503, 200, 8, false, 2254, "1.2", 25, false, MountType.Tiger, 3305936L)]
    [DataRow(455, 200, 10, true, 2233, "1.2", 25, false, MountType.Griffin, 7577894L)]
    [DataRow(455, 200, 10, true, 2233, "1.35", 25, false, MountType.Griffin, 8525131L)]
    [DataRow(557, 200, 10, true, 2233, "1.35", 25, false, MountType.Griffin, 3770884L)]
    [DataRow(561, 200, 10, true, 2233, "1.35", 25, false, MountType.Tiger, 2595045L)]
    [DataRow(597, 200, 10, true, 2233, "1.35", 25, false, MountType.Griffin, 2765869L)]
    public void ExpeditionService_DoExpeditions_ReturnsCorrectExperience(int characterLevel, int guildBonus, int runeBonus, bool hasScroll, int scrapbookItems,
        string avgStars, int thirst, bool isExperienceEvent, MountType mount, long expectedResult)
    {
        var stars = Convert.ToDecimal(avgStars);
        ExpeditionService.Options = new ExpeditionOptions(2M, stars);
        var experienceBonus = new ExperienceBonus(scrapbookItems / (decimal)CoreShared.SCRAPBOOK_LIMIT * 100, guildBonus, runeBonus, hasScroll);
        var simulationContext = new SimulationContext
        {
            Level = characterLevel,
            ExperienceBonus = experienceBonus,
            Mount = mount
        };
        var events = isExperienceEvent ? new List<EventType> { EventType.Experience } : new List<EventType>();

        var totalExperience = 0L;
        ExpeditionService.DoExpeditions(simulationContext, events, thirst, gold => { }, exp => totalExperience += exp);

        Assert.AreEqual(expectedResult, totalExperience);
    }
}