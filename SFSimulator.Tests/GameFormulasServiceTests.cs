using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;

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
}