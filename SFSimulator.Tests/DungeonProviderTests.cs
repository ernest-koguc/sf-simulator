using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System.Linq;

namespace SFSimulator.Tests;

[TestClass]
public class DungeonProviderTests
{
    [TestMethod]
    public void GetAllDungeons_does_not_provide_duplicate_positions()
    {
        var service = DependencyProvider.Get<IDungeonProvider>();

        var dungeons = service.GetAllDungeons(new SimulationContext());

        foreach (var dungeon in dungeons)
        {
            Assert.IsTrue(dungeon.DungeonEnemies.DistinctBy(x => x.Position).Count() == dungeon.DungeonEnemies.Count,
                $"Dungeon {dungeon} ({dungeon.Type}) contains enemies with not unique positions");
        }

        Assert.IsTrue(dungeons.DistinctBy(x => x.Position).Count() == dungeons.Count, "Dungeons contain not unique positions");
    }
}
