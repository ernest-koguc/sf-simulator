using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System;

namespace SFSimulator.Tests;

[TestClass]
public class ExpeditionServiceTests
{
    private IExpeditionService ExpeditionService { get; set; } = DepedencyProvider.GetRequiredService<IExpeditionService>();

    [TestMethod]
    public void ExpeditionService_GetDailyExpeditionGold_ReturnsCorrectGold()
    {
        ExpeditionService.Options = new ExpeditionOptions(2M, 1.3M);
        var characterLevel = 632;
        var goldBonus = new GoldBonus(100, 200, 50, true);
        var isGoldEvent = false;
        var mount = MountType.Griffin;

        var result = Math.Round(ExpeditionService.GetDailyExpeditionGold(characterLevel, goldBonus, isGoldEvent, mount, 320));

        Assert.AreEqual(result, 1900800000);
    }

    [TestMethod]
    public void ExpeditionService_GetDailyExpeditionGold_ReturnsCappedGoldOnGoldEvent()
    {
        ExpeditionService.Options = new ExpeditionOptions(2M, 1.3M);
        var characterLevel = 832;
        var goldBonus = new GoldBonus(100, 200, 50, true);
        var mount = MountType.Griffin;

        var resultOnGoldEvent = Math.Round(ExpeditionService.GetDailyExpeditionGold(characterLevel, goldBonus, true, mount, 320));
        var resultWithoutGoldEvent = Math.Round(ExpeditionService.GetDailyExpeditionGold(characterLevel, goldBonus, false, mount, 320));

        Assert.AreEqual(resultOnGoldEvent, resultWithoutGoldEvent);
    }

    [TestMethod]
    [DataRow(503, 118, 8, true, 2254, "1", 25, false, MountType.None, 1719862L)]
    [DataRow(503, 200, 8, true, 2254, "1", 25, false, MountType.None, 2154158L)]
    [DataRow(503, 200, 8, false, 2254, "1.2", 25, false, MountType.Tiger, 3336986L)]
    [DataRow(455, 200, 10, true, 2233, "1.2", 25, false, MountType.Griffin, 7648208L)]
    [DataRow(455, 200, 10, true, 2233, "1.35", 25, false, MountType.Griffin, 8604234L)]
    [DataRow(557, 200, 10, true, 2233, "1.35", 25, false, MountType.Griffin, 3805873L)]
    [DataRow(561, 200, 10, true, 2233, "1.35", 25, false, MountType.Tiger, 2619124L)]
    [DataRow(597, 200, 10, true, 2233, "1.35", 25, false, MountType.Griffin, 2791533L)]
    public void ExpeditionService_GetDailyExpeditionExperience_ReturnsCorrectExperience(int characterLevel, int guildBonus, int runeBonus, bool hasScroll, int scrapbookItems, string avgStars, int thirst, bool isExperienceEvent, MountType mount, long expectedResult)
    {
        var stars = Convert.ToDecimal(avgStars);
        ExpeditionService.Options = new ExpeditionOptions(2M, stars);
        var experienceBonus = new ExperienceBonus(scrapbookItems / 2283F * 100, guildBonus, runeBonus, hasScroll);

        var result = ExpeditionService.GetDailyExpeditionExperience(characterLevel, experienceBonus, isExperienceEvent, mount, thirst);

        Assert.AreEqual(expectedResult, result);
    }
}