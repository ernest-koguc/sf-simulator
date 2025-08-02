using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System.Linq;

namespace SFSimulator.Tests;

[TestClass]
public class ScrapbookServiceTests
{
    [TestMethod]
    [DataRow(10, 22.1988)]
    [DataRow(100, 69.7135)]
    [DataRow(250, 72.4936)]
    [DataRow(500, 74.347)]
    public void InitScrapbook_gives_correct_count(int level, double expectedFillness)
    {
        var scrapbookService = DependencyProvider.Get<IScrapbookService>();
        var dungeonProvider = DependencyProvider.Get<IDungeonProvider>();
        var simulationContext = new SimulationContext();
        simulationContext.Level = level;
        simulationContext.ExperienceBonus.ScrapbookFillness = 0;
        var dungeons = dungeonProvider.InitDungeons();

        scrapbookService.InitScrapbook(dungeons, simulationContext);

        Assert.AreEqual(expectedFillness, (double)simulationContext.ExperienceBonus.ScrapbookFillness, 0.001);
    }

    [TestMethod]
    [DataRow(1, 25)]
    [DataRow(10, 80)]
    [DataRow(100, 90)]
    [DataRow(250, 100)]
    public void InitScrapbook_doesnt_decrease_initial_scrapbook(int level, double initialFillness)
    {
        var scrapbookService = DependencyProvider.Get<IScrapbookService>();
        var dungeonProvider = DependencyProvider.Get<IDungeonProvider>();
        var simulationContext = new SimulationContext();
        simulationContext.Level = level;
        simulationContext.ExperienceBonus.ScrapbookFillness = (decimal)initialFillness;
        var dungeons = dungeonProvider.InitDungeons();

        scrapbookService.InitScrapbook(dungeons, simulationContext);

        Assert.AreEqual(initialFillness, (double)simulationContext.ExperienceBonus.ScrapbookFillness, 0.001);
    }

    [TestMethod]
    public void InitScrapbook_gives_full_scrapbook_when_dungeons_are_finished()
    {
        var scrapbookService = DependencyProvider.Get<IScrapbookService>();
        var dungeonProvider = DependencyProvider.Get<IDungeonProvider>();
        var simulationContext = new SimulationContext();
        simulationContext.Level = 700;
        simulationContext.SoloPortal = 50;
        simulationContext.GuildPortal = 50;
        simulationContext.GuildRaids = 50;
        simulationContext.ExperienceBonus.ScrapbookFillness = 0;
        var dungeons = dungeonProvider.InitDungeons();
        foreach (var dungeonEnemy in dungeons.SelectMany(d => d.DungeonEnemies))
        {
            dungeonEnemy.IsDefeated = true;
        }

        scrapbookService.InitScrapbook(dungeons, simulationContext);

        Assert.AreEqual(100, (double)simulationContext.ExperienceBonus.ScrapbookFillness);
    }
}
