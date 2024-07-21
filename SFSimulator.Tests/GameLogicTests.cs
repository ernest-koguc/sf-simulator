using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System.Collections.Generic;

namespace SFSimulator.Tests
{
    [TestClass]
    public class GameLogicTests
    {
        [TestMethod]
        public void GetAcademyHourlyProduction_gives_correct_values()
        {
            var gameLogic = new GameLogic(new Curves());

            var xpValues = new List<long>();

            for (var i = 0; i < 10; i++)
            {
                var xp = gameLogic.GetAcademyHourlyProduction(300 + i, 10 + i, false);
                xpValues.Add(xp);
            }


            Assert.AreEqual(xpValues[0], 168851);
            Assert.AreEqual(xpValues[1], 187027);
            Assert.AreEqual(xpValues[2], 205476);
            Assert.AreEqual(xpValues[3], 224138);
            Assert.AreEqual(xpValues[4], 243081);
            Assert.AreEqual(xpValues[5], 262215);
            Assert.AreEqual(xpValues[6], 281658);
            Assert.AreEqual(xpValues[7], 301286);
            Assert.AreEqual(xpValues[8], 321211);
            Assert.AreEqual(xpValues[9], 341339);
        }
        [TestMethod]
        public void GetDailyMissionExperience_gives_correct_values()
        {
            var gameLogic = new GameLogic(new Curves());

            var xpValues = new List<long>();


            for (var i = 0; i < 10; i++)
            {
                var xp = gameLogic.GetDailyMissionExperience(400 + i, false, i);
                xpValues.Add(xp);
            }

            Assert.AreEqual(xpValues[0], 4987531);
            Assert.AreEqual(xpValues[1], 6218905);
            Assert.AreEqual(xpValues[2], 7444168);
            Assert.AreEqual(xpValues[3], 8663366);
            Assert.AreEqual(xpValues[4], 9876543);
            Assert.AreEqual(xpValues[5], 11083743);
            Assert.AreEqual(xpValues[6], 12285012);
            Assert.AreEqual(xpValues[7], 13480392);
            Assert.AreEqual(xpValues[8], 14669926);
            Assert.AreEqual(xpValues[9], 15853658);

        }

        [TestMethod]
        public void GetDailyMissionGold_gives_correct_values()
        {
            var gameLogic = new GameLogic(new Curves());
            var goldValues = new List<decimal>
            {
                gameLogic.GetDailyMissionGold(227),
                gameLogic.GetDailyMissionGold(455),
                gameLogic.GetDailyMissionGold(502),
                gameLogic.GetDailyMissionGold(632)
            };

            Assert.AreEqual(goldValues[0], 808852);
            Assert.AreEqual(goldValues[1], 25978713);
            Assert.AreEqual(goldValues[2], 44268920);
            Assert.AreEqual(goldValues[3], 160000000);
        }
    }
}