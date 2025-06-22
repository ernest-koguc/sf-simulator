using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System.Linq;

namespace SFSimulator.Tests;

[TestClass]
public class GameFormulasServiceTests
{
    [TestMethod]
    [DataRow(300, 10, 168851L)]
    [DataRow(301, 11, 187027L)]
    [DataRow(302, 12, 205476L)]
    [DataRow(303, 13, 224138L)]
    [DataRow(304, 14, 243081L)]
    [DataRow(305, 15, 262215L)]
    [DataRow(306, 16, 281658L)]
    [DataRow(307, 17, 301286L)]
    [DataRow(308, 18, 321211L)]
    [DataRow(309, 19, 341339L)]
    public void GetAcademyHourlyProduction_gives_correct_values(int level, int academyLevel, long experience)
    {
        var gameFormulasService = DependencyProvider.Get<IGameFormulasService>();
        var result = gameFormulasService.GetAcademyHourlyProduction(level, academyLevel, false);

        Assert.AreEqual(result, experience);
    }

    [TestMethod]
    [DataRow(400, 0, 4987531L)]
    [DataRow(401, 1, 6218905L)]
    [DataRow(402, 2, 7444168L)]
    [DataRow(403, 3, 8663366L)]
    [DataRow(404, 4, 9876543L)]
    [DataRow(405, 5, 11083743L)]
    [DataRow(406, 6, 12285012L)]
    [DataRow(407, 7, 13480392L)]
    [DataRow(408, 8, 14669926L)]
    [DataRow(409, 9, 15853658L)]
    public void GetDailyMissionExperience_gives_correct_values(int level, int hydraLevel, long experience)
    {
        var gameFormulasService = DependencyProvider.Get<IGameFormulasService>();
        var result = gameFormulasService.GetDailyMissionExperience(level, false, hydraLevel);

        Assert.AreEqual(result, experience);
    }

    [TestMethod]
    [DataRow(227, 808_852D)]
    [DataRow(455, 25_978_713D)]
    [DataRow(502, 44_268_920D)]
    [DataRow(632, 160_000_000D)]
    public void GetDailyMissionGold_gives_correct_values(int level, double gold)
    {
        var gameLogic = new GameFormulasService(new Curves());
        var result = gameLogic.GetDailyMissionGold(level);

        Assert.AreEqual((decimal)gold, result);
    }

    [TestMethod]
    [DataRow(1, 80L)]
    [DataRow(100, 649_764L)]
    [DataRow(393, 51_135_095L)]
    [DataRow(400, 49_025_697L)]
    [DataRow(632, 12_135_721L)]
    public void GetExperienceForPetDungeonEnemy_gives_correct_values(int level, long xp)
    {
        var gameFormulaService = DependencyProvider.Get<IGameFormulasService>();
        var result = gameFormulaService.GetExperienceForPetDungeonEnemy(level);

        Assert.AreEqual(xp, result);
    }

    [TestMethod]
    [DataRow(1, 1, false)]
    [DataRow(1, 3, true)]
    [DataRow(1, 5, true)]
    [DataRow(1, 7, true)]
    [DataRow(1, 10, true)]
    [DataRow(10, 4, false)]
    [DataRow(10, 10, true)]
    [DataRow(99, 658, true)]
    [DataRow(99, 711, false)]
    [DataRow(19, 1, true)]
    [DataRow(19, 10, true)]
    [DataRow(22, 1, true)]
    [DataRow(22, 10, true)]
    [DataRow(98, 1, false)]
    [DataRow(98, 5, false)]
    [DataRow(98, 100, false)]
    public void GetGoldForDungeonEnemey_gives_gold_or_item_with_gold_value(int dungeonPosition, int enemyPosition, bool givesItem) // if not item than gold)
    {
        var gameFormulaService = DependencyProvider.Get<IGameFormulasService>();
        var dungeonProvider = DependencyProvider.Get<IDungeonProvider>();

        var dungeonEnemy = dungeonProvider.GetDungeonEnemy(dungeonPosition, enemyPosition);

        var gold = gameFormulaService.GetGoldForDungeonEnemy(dungeonEnemy);
        var hasItem = gameFormulaService.DoesDungeonEnemyDropItem(dungeonEnemy);

        Assert.AreEqual(givesItem, hasItem, "Expected dungeon to give an item");
        Assert.IsTrue(givesItem ? gold == 0 : gold > 0, "Expected dungeon to either have 0 gold and an item or to not have any item but have gold");
    }

    [TestMethod]
    [DataRow(1, 1, 6.5)]
    [DataRow(1, 2, 9)]
    [DataRow(1, 6, 32)]
    [DataRow(1, 9, 154)]
    [DataRow(10, 9, 144_249.1)]
    [DataRow(11, 4, 228_866.2)]
    [DataRow(16, 2, 287_140.4)]
    [DataRow(99, 711, 2_747_370.9)]
    [DataRow(98, 5, 318_717)]
    [DataRow(98, 100, 7_386_146.1)]
    public void GetGoldForDungeonEnemey_gives_gold_or_item_with_gold_value(int dungeonPosition, int enemyPosition, double expectedGold)
    {
        var gameFormulaService = DependencyProvider.Get<IGameFormulasService>();
        var dungeonProvider = DependencyProvider.Get<IDungeonProvider>();

        var dungeonEnemy = dungeonProvider.GetDungeonEnemy(dungeonPosition, enemyPosition);

        var gold = gameFormulaService.GetGoldForDungeonEnemy(dungeonEnemy);

        Assert.AreEqual((decimal)expectedGold, gold);
    }

    [TestMethod]
    public void GetGoldForDungeonEnemy_does_not_give_gold_or_item_for_dungeons_in_shadow_loop()
    {
        var gameFormulaService = DependencyProvider.Get<IGameFormulasService>();
        var dungeonProvider = DependencyProvider.Get<IDungeonProvider>();

        var dungeons = dungeonProvider.GetAllDungeons(new SimulationContext());

        foreach (var enemy in dungeons.Where(d => d.Type is DungeonTypeEnum.Shadow or DungeonTypeEnum.LoopOfIdols).SelectMany(d => d.DungeonEnemies))
        {

            var gold = gameFormulaService.GetGoldForDungeonEnemy(enemy);
            var hasItem = gameFormulaService.DoesDungeonEnemyDropItem(enemy);

            Assert.AreEqual(false, hasItem, "Expected dungeon to not give an item");
            Assert.AreEqual(0, gold, "Expected dungeon to not give any gold");
        }
    }

    [TestMethod]
    public void GetExpeditionGold_is_capped_during_gold_event()
    {
        var gameFormulaService = DependencyProvider.Get<IGameFormulasService>();
        var characterLevel = 832;
        var goldBonus = new GoldBonus(100, 200, 50, true);
        var mount = MountType.Griffin;

        var withGoldEvent = gameFormulaService.GetExpeditionChestGold(characterLevel, goldBonus, true, mount, 320);
        var withoutGoldEvent = gameFormulaService.GetExpeditionChestGold(characterLevel, goldBonus, false, mount, 320);
        Assert.AreEqual(withGoldEvent, withoutGoldEvent);

        withGoldEvent = gameFormulaService.GetExpeditionFinalGold(characterLevel, goldBonus, true, mount, 320);
        withoutGoldEvent = gameFormulaService.GetExpeditionFinalGold(characterLevel, goldBonus, false, mount, 320);
        Assert.AreEqual(withGoldEvent, withoutGoldEvent);

        withGoldEvent = gameFormulaService.GetExpeditionMidwayGold(characterLevel, goldBonus, true, mount, 320);
        withoutGoldEvent = gameFormulaService.GetExpeditionMidwayGold(characterLevel, goldBonus, false, mount, 320);
        Assert.AreEqual(withGoldEvent, withoutGoldEvent);
    }

    [TestMethod]
    [DataRow(100, 10, 5_208)]
    [DataRow(150, 15, 58_812)]
    [DataRow(200, 20, 381_942)]
    [DataRow(300, 30, 6_322_622)]
    [DataRow(400, 40, 52_523_705)]
    [DataRow(500, 50, 292_354_110)]
    [DataRow(632, 99, 300_000_000)]
    public void GetGoldPitCapacity_returns_correct_capacity(int characterLevel, int goldPitLevel, int expectedCapacity)
    {
        var gameFormulaService = DependencyProvider.Get<IGameFormulasService>();
        var capacity = gameFormulaService.GetGoldPitCapacity(characterLevel, goldPitLevel);

        Assert.AreEqual(expectedCapacity, capacity);
    }

    [TestMethod]
    [DataRow(100, 10, 45_747)]
    [DataRow(150, 15, 511_497)]
    [DataRow(393, 20, 6_921_840)]
    [DataRow(500, 19, 2_535_030)]
    [DataRow(632, 20, 1_022_484)]
    public void GetAcademyCapacity_returns_correct_capacity(int characterLevel, int academyLevel, int expectedCapacity)
    {
        var gameFormulaService = DependencyProvider.Get<IGameFormulasService>();
        var capacity = gameFormulaService.GetAcademyCapacity(characterLevel, academyLevel);

        Assert.AreEqual(expectedCapacity, capacity);
    }
}