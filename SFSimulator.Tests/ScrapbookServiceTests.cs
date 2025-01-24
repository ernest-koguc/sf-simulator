using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System.Linq;

namespace SFSimulator.Tests;

[TestClass]
public class ScrapbookServiceTests
{
    [TestMethod]
    [DataRow(10, 0.2276)]
    [DataRow(100, 0.7149)]
    [DataRow(250, 0.7434)]
    [DataRow(500, 0.7624)]
    public void InitScrapbook_gives_correct_count(int level, double expectedFillness)
    {
        //TODO: Move to interface
        var scrapbookService = DependencyProvider.GetRequiredService<ScrapbookService>();
        var dungeonProvider = DependencyProvider.GetRequiredService<IDungeonProvider>();
        var simulationContext = new SimulationContext();
        simulationContext.Level = level;
        simulationContext.ExperienceBonus.ScrapbookFillness = 0;
        var dungeons = dungeonProvider.InitDungeons();

        scrapbookService.InitScrapbook(dungeons, simulationContext);

        Assert.AreEqual(expectedFillness, (double)simulationContext.ExperienceBonus.ScrapbookFillness, 0.001);
    }

    [TestMethod]
    public void InitScrapbook_gives_full_scrapbook_when_dungeons_are_finished()
    {
        var scrapbookService = DependencyProvider.GetRequiredService<ScrapbookService>();
        var dungeonProvider = DependencyProvider.GetRequiredService<IDungeonProvider>();
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

        Assert.AreEqual(1, (double)simulationContext.ExperienceBonus.ScrapbookFillness);
    }
}
