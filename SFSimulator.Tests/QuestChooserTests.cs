using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System;
using System.Collections.Generic;

namespace SFSimulator.Tests
{
    [TestClass]
    public class QuestChooserTests
    {
        [TestMethod]
        public void ChooseBestQuest_gives_correct_values_with_hybrid()
        {
            var questFactory = new QuestFactory(new QuestHelper(), new ItemGenerator(new Random(1), new ValuesReader()), new Random(1));
            var questChooser = new QuestChooser();
            var characterHelper = new CharacterHelper(new ValuesReader(), new CurvesHelper());
            var questValues = characterHelper.GetMinimumQuestValue(400, new ExperienceBonus(), new GoldBonus());

            for (var i =0; i<100; i++)
            {
                var quest = questFactory.Create(questValues, 400, 100, true, 50, new List<EventType>(), MountType.Griffin);

                var chosenQuest = questChooser.ChooseBestQuest(new List<Quest>() { quest }, Priority.HYBRID, QuestChooserAI.SMART, 0.1F);

                Assert.IsTrue(chosenQuest.Experience == quest.Experience);
                Assert.IsTrue(chosenQuest.Gold == quest.Gold);
                Assert.IsTrue(chosenQuest.MinimumValues == quest.MinimumValues);
                Assert.IsTrue(chosenQuest.Item == quest.Item);
                Assert.IsTrue(chosenQuest.Time == quest.Time);
            }
        }
    }
}
