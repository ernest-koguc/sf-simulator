using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFSimulator.Tests
{
    [TestClass]
    public class ThirstSimulatorTests
    {
        [TestMethod]
        public void NextQuests_gives_same_collective_thirst_as_set_in_StartThirstMethod()
        {
            for (int i = 0; i < 10; i++)
            {
                var questChooser = new QuestChooser();
                questChooser.QuestOptions = new QuestOptions(QuestPriorityType.Gold, null);
                var thirstSimulator = new ThirstSimulator(new QuestFactory(new QuestHelper(), new ItemGenerator(new Random(), new ItemValueProvider(new Random(0))), new Random()), new QuestHelper());
                thirstSimulator.ThirstSimulationOptions.Mount = MountType.Griffin;
                var thirst = 200;
                var choosenQuestsThirst = 0M;
                var quests = thirstSimulator.StartThirst(thirst, new QuestValue(10000, 10000), 1, new List<EventType>());

                while (quests is not null)
                {
                    var choosenQuest = questChooser.ChooseBestQuest(quests);
                    choosenQuestsThirst += choosenQuest.Time;
                    quests = thirstSimulator.NextQuests(choosenQuest, new QuestValue(10000, 10000), 1);
                }
                Assert.AreEqual(thirst, choosenQuestsThirst);
            }
        }
        [TestMethod]
        public void NextQuests_generates_quests_with_correct_time()
        {
            var questChooser = new QuestChooser();
            questChooser.QuestOptions = new QuestOptions(QuestPriorityType.Gold, null);
            var thirstSimulator = new ThirstSimulator(new QuestFactory(new QuestHelper(), new ItemGenerator(new Random(), new ItemValueProvider(new Random(0))), new Random()), new QuestHelper());
            thirstSimulator.ThirstSimulationOptions.Mount = MountType.Griffin;
            var choosenQuestsThirst = new List<Quest>();
            var quests = thirstSimulator.StartThirst(200, new QuestValue(10000, 10000), 1, new List<EventType>());

            while (quests is not null)
            {
                var choosenQuest = questChooser.ChooseBestQuest(quests);
                choosenQuestsThirst.Add(choosenQuest);
                quests = thirstSimulator.NextQuests(choosenQuest, new QuestValue(10000, 10000), 1);
            }

            Assert.IsFalse(choosenQuestsThirst.Any(x => x.Time != 2.5M && x.Time != 5 && x.Time != 7.5M && x.Time != 10));
        }
    }
}