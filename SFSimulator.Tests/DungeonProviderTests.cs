using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System;
using System.Linq;

namespace SFSimulator.Tests;

[TestClass]
public class DungeonProviderTests
{
    [TestMethod]
    public void InitDungeons_loads_enemy_stats_from_json()
    {
        var enemy = new DungeonProvider().GetDungeonEnemy(1, 1);

        Assert.AreEqual(ClassType.Mage, enemy.Class);
        Assert.AreEqual(10, enemy.Level);
        Assert.AreEqual(48, enemy.Strength);
        Assert.AreEqual(52, enemy.Dexterity);
        Assert.AreEqual(104, enemy.Intelligence);
        Assert.AreEqual(77, enemy.Constitution);
        Assert.AreEqual(47, enemy.Luck);
        Assert.AreEqual(1694L, enemy.Health);
        Assert.AreEqual(29, enemy.FirstWeapon!.MinDmg);
        Assert.AreEqual(52, enemy.FirstWeapon.MaxDmg);
        Assert.AreEqual(RuneType.None, enemy.FirstWeapon.RuneType);
        Assert.AreEqual(1, enemy.Dungeon.Position);
        Assert.AreSame(enemy, enemy.Dungeon.DungeonEnemies[0]);
    }

    [TestMethod]
    [DataRow(13, 1, DungeonTypeEnum.Default, 10, 210, 16000000L)]
    [DataRow(15, 1, DungeonTypeEnum.Default, 10, 355, 12976200L)]
    [DataRow(113, 1, DungeonTypeEnum.Shadow, 10, 320, 181000000L)]
    [DataRow(-1, 1, DungeonTypeEnum.Tower, 100, 200, 16019700L)]
    [DataRow(-2, 1, DungeonTypeEnum.Twister, 1000, 100, 1100395L)]
    [DataRow(-3, 30, DungeonTypeEnum.LoopOfIdols, 30, 625, 1170344560L)]
    [DataRow(-4, 1000, DungeonTypeEnum.Sandstorm, 1000, 833, 9969844500L)]
    public void InitDungeons_preserves_dungeon_positions_and_types(int position, int floor, DungeonTypeEnum type,
        int floorCount, int level, long health)
    {
        var enemy = new DungeonProvider().GetDungeonEnemy(position, floor);

        Assert.AreEqual(position, enemy.Dungeon.Position);
        Assert.AreEqual(type, enemy.Dungeon.Type);
        Assert.AreEqual(floorCount, enemy.Dungeon.DungeonEnemies.Count);
        Assert.AreEqual(level, enemy.Level);
        Assert.AreEqual(health, enemy.Health);
    }

    [TestMethod]
    [DataRow(1, 1, 100)]
    [DataRow(21, 1, 2050)]
    [DataRow(-1, 1, 15000)]
    [DataRow(13, 1, 10750)]
    [DataRow(121, 1, 7000)]
    public void InitDungeons_applies_explicit_armor_and_dungeon_multipliers(int position, int floor, int armor)
    {
        var enemy = new DungeonProvider().GetDungeonEnemy(position, floor);

        Assert.AreEqual(armor, enemy.Armor);
    }

    [TestMethod]
    [DataRow(1, RuneType.FireDamage, 25, 0, 0)]
    [DataRow(4, RuneType.ColdDamage, 0, 25, 0)]
    [DataRow(2, RuneType.LightningDamage, 0, 0, 25)]
    public void InitDungeons_maps_damage_runes_and_resistances(int floor, RuneType runeType,
        int fireResistance, int coldResistance, int lightningResistance)
    {
        var enemy = new DungeonProvider().GetDungeonEnemy(13, floor);

        Assert.AreEqual(runeType, enemy.FirstWeapon!.RuneType);
        Assert.AreEqual(25, enemy.FirstWeapon.RuneValue);
        Assert.AreEqual(fireResistance, enemy.FireResistance);
        Assert.AreEqual(coldResistance, enemy.ColdResistance);
        Assert.AreEqual(lightningResistance, enemy.LightningResistance);
    }

    [TestMethod]
    [DataRow(9, 195)]
    [DataRow(109, 350)]
    public void InitDungeons_supports_mirror_floors_without_stats(int position, int level)
    {
        var service = new DungeonProvider();
        var enemy = service.GetDungeonEnemy(position, 10);

        Assert.AreEqual(ClassType.Mirror, enemy.Class);
        Assert.AreEqual(level, enemy.Level);
        Assert.AreEqual(0L, enemy.Health);
        Assert.AreEqual(0, enemy.Armor);

        var context = new SimulationContext { Class = ClassType.Mage, BaseIntelligence = 100 };
        service.GetAllDungeons(context);

        Assert.AreEqual(context.Class, enemy.Class);
        Assert.AreEqual(context.Intelligence, enemy.Intelligence);
    }

    [TestMethod]
    public void InitDungeons_initializes_both_assassin_weapons()
    {
        var enemy = new DungeonProvider().GetDungeonEnemy(24, 6);

        Assert.AreEqual(ClassType.Assassin, enemy.Class);
        Assert.IsNotNull(enemy.SecondWeapon);
        Assert.AreEqual(2524, enemy.SecondWeapon.MinDmg);
        Assert.AreEqual(4318, enemy.SecondWeapon.MaxDmg);
        Assert.AreEqual(RuneType.ColdDamage, enemy.SecondWeapon.RuneType);
        Assert.AreEqual(45, enemy.SecondWeapon.RuneValue);
    }

    [TestMethod]
    public void InitDungeons_does_not_share_mutable_state_with_cached_definitions()
    {
        var service = new DungeonProvider();
        var enemy = service.GetDungeonEnemy(1, 1);
        enemy.IsDefeated = true;
        enemy.Dungeon.IsDefeated = true;
        enemy.FirstWeapon!.MinDmg = 0;

        var freshEnemy = new DungeonProvider().GetDungeonEnemy(1, 1);

        Assert.AreNotSame(enemy, freshEnemy);
        Assert.AreNotSame(enemy.Dungeon, freshEnemy.Dungeon);
        Assert.IsFalse(freshEnemy.IsDefeated);
        Assert.IsFalse(freshEnemy.Dungeon.IsDefeated);
        Assert.AreEqual(29, freshEnemy.FirstWeapon!.MinDmg);
    }

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
            AssertRuneIsInLimit(nameof(enemy.FireResistance), 0, 75, enemy.FireResistance, enemy.Dungeon.Position, enemy.Position);
            AssertRuneIsInLimit(nameof(enemy.ColdResistance), 0, 75, enemy.ColdResistance, enemy.Dungeon.Position, enemy.Position);
            AssertRuneIsInLimit(nameof(enemy.LightningResistance), 0, 75, enemy.LightningResistance, enemy.Dungeon.Position, enemy.Position);
            if (enemy.FirstWeapon is not null)
            {
                Assert.IsTrue(enemy.FirstWeapon.RuneValue >= 0 && enemy.FirstWeapon.RuneValue <= 60, $"First weapon rune out of range (actual of ({enemy.FirstWeapon?.RuneValue}) for dungeon position {enemy.Dungeon.Position}, enemy position {enemy.Position}");
                Assert.IsTrue(enemy.FirstWeapon.RuneType == RuneType.None ? enemy.FirstWeapon.RuneValue == 0 : enemy.FirstWeapon.RuneValue > 0, $"Invalid weapon rune {enemy.Dungeon.Position}, enemy position {enemy.Position}");
            }
            if (enemy.SecondWeapon is not null)
            {
                Assert.IsTrue(enemy.SecondWeapon.RuneValue >= 0 && enemy.SecondWeapon.RuneValue <= 60, $"First weapon rune out of range (actual of ({enemy.SecondWeapon?.RuneValue}) for dungeon position {enemy.Dungeon.Position}, enemy position {enemy.Position}");
                Assert.IsTrue(enemy.SecondWeapon.RuneType == RuneType.None ? enemy.SecondWeapon.RuneValue == 0 : enemy.SecondWeapon.RuneValue > 0, $"Invalid weapon rune {enemy.Dungeon.Position}, enemy position {enemy.Position}");
            }
        }

    }
        private void AssertRuneIsInLimit(string runeName, int from, int to, int actual, int dungPosition, int enemyPosition)
        {
            Assert.IsTrue(actual >= from && actual <= to, $"{runeName} was out of range (actual of {actual}) for dungeon {dungPosition} enemy {enemyPosition}");
        }

    [TestMethod]
    public void GetAllDungeons_all_dungeons_and_enemies_have_names()
    {
        var service = DependencyProvider.Get<IDungeonProvider>();

        var dungeons = service.GetAllDungeons(new SimulationContext());

        foreach (var enemy in dungeons.SelectMany(d => d.DungeonEnemies))
        {
            string? name = null;
            try
            {
                name = enemy.Name;
            }
            catch (Exception)
            {
                Assert.Fail($"Dungeon position {enemy.Dungeon.Position}, enemy position {enemy.Position} does not have a name");
            }

            if (name == string.Empty)
            {
                Assert.Inconclusive($"Dungeon position {enemy.Dungeon.Position}, enemy position {enemy.Position} has an empty name");
            }

        }

        foreach (var dungeon in dungeons)
        {
            try
            {
                var name = dungeon.Name;
            }
            catch (Exception)
            {
                Assert.Fail($"Dungeon position {dungeon.Position} does not have a name");
            }
        }
    }
}