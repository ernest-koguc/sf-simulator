using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuestSimulator.Characters;
using QuestSimulator.FileReaders;
using QuestSimulator.Utility;
using System.Collections.Generic;

namespace QuestSimulatorTests
{
    [TestClass]
    public class CharacterHelperTests
    {
        [TestMethod]
        public void GetMinimumQuestValue_gives_correct_values()
        {
            //var valuesReader = new ValuesReader();
            //var charHelper = new CharacterHelper(valuesReader, new CurvesHelper());
            //var goldBonus = new GoldBonus() { HasGoldScroll = false };
            //var xpBonus = new ExperienceBonus() { HasExperienceScroll = false, ScrapbookFillness = 0.95f };
            //for (var x = 330; x <= 450; x++)
            //{
            //    Assert.AreEqual(questValues[x].Gold, charHelper.GetMinimumQuestValue(x, xpBonus, goldBonus).Gold, 0.99d);
            //    Assert.AreEqual(questValues[x].Experience, charHelper.GetMinimumQuestValue(x, xpBonus, goldBonus).Experience, 10d);
            //}
        }
        [TestMethod]
        public void GetAcademyHourlyProduction_gives_correct_values()
        {
            var characterHelper = new CharacterHelper(new ValuesReader(), new CurvesHelper());

            var xpValues = new List<int>();

            for (var i = 0; i < 10; i++)
            {
                var xp = characterHelper.GetAcademyHourlyProduction(300 + i, 10 + i, false);
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
        public void GetDailyMissionReward_gives_correct_values()
        {
            var characterHelper = new CharacterHelper(new ValuesReader(), new CurvesHelper());

            var xpValues = new List<int>();


            for (var i = 0; i < 10; i++)
            {
                var xp = characterHelper.GetDailyMissionReward(400 + i, false, i);
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
    }
}
