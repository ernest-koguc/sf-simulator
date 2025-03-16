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

    [TestMethod]
    public void GetAllDungeons_provides_runes_that_dont_exceed_their_range()
    {
        var service = DependencyProvider.Get<IDungeonProvider>();

        var dungeons = service.GetAllDungeons(new SimulationContext());

        foreach (var enemy in dungeons.SelectMany(d => d.DungeonEnemies))
        {
            Assert.IsTrue(enemy.FireResistance >= 0 && enemy.FireResistance <= 75, $"{nameof(enemy.FireResistance)} out of range for dungeon position {enemy.Dungeon.Position}, enemy position {enemy.Position}");
            Assert.IsTrue(enemy.ColdResistance >= 0 && enemy.ColdResistance <= 75, $"{nameof(enemy.ColdResistance)} out of range for dungeon position {enemy.Dungeon.Position}, enemy position {enemy.Position}");
            Assert.IsTrue(enemy.LightningResistance >= 0 && enemy.LightningResistance <= 75, $"{nameof(enemy.LightningResistance)} out of range for dungeon position {enemy.Dungeon.Position}, enemy position {enemy.Position}");
            Assert.IsTrue(enemy.FirstWeapon?.RuneValue >= 0 && enemy.FirstWeapon?.RuneValue <= 60, $"Fire resistance out of range for dungeon position {enemy.Dungeon.Position}, enemy position {enemy.Position}");
            Assert.IsTrue(enemy.FirstWeapon?.RuneType == RuneType.None ? enemy.FirstWeapon?.RuneValue == 0 : enemy.FirstWeapon?.RuneValue > 0, $"Invalid weapon rune {enemy.Dungeon.Position}, enemy position {enemy.Position}");
        }
    }
}
