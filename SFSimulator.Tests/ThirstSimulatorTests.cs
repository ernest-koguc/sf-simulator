using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System;
using System.Collections.Generic;

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
                var thirstSimulator = new ThirstSimulator(new QuestFactory(new QuestHelper(), new ItemGenerator(new Random(), new ValuesReader()), new Random()), new QuestHelper());
                thirstSimulator.ThirstSimulationOptions.Mount = MountType.Griffin;
                var thirst = 200;
                var choosenQuestsThirst = 0d;
                var quests = thirstSimulator.StartThirst(thirst, new QuestValue(10000, 10000), 1, new List<EventType>());


                //SMART AI
                while (quests is not null)
                {
                    var choosenQuest = questChooser.ChooseBestQuest(quests, Priority.GOLD, QuestChooserAI.SMART);
                    choosenQuestsThirst += choosenQuest.Time;
                    quests = thirstSimulator.NextQuests(choosenQuest, new QuestValue(10000, 10000), 1);
                }
                Assert.AreEqual(thirst, choosenQuestsThirst);

                choosenQuestsThirst = 0d;
                //SIMPLE AI
                quests = thirstSimulator.StartThirst(thirst, new QuestValue(10000, 10000), 1, new List<EventType>());
                while (quests is not null)
                {
                    var choosenQuest = questChooser.ChooseBestQuest(quests, Priority.GOLD, QuestChooserAI.SIMPLE);
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
            var thirstSimulator = new ThirstSimulator(new QuestFactory(new QuestHelper(), new ItemGenerator(new Random(), new ValuesReader()), new Random()), new QuestHelper());
            thirstSimulator.ThirstSimulationOptions.Mount = MountType.Griffin;
            var choosenQuestsThirst = new List<Quest>();
            var quests = thirstSimulator.StartThirst(200, new QuestValue(10000, 10000), 1, new List<EventType>());

            while (quests is not null)
            {
                var choosenQuest = questChooser.ChooseBestQuest(quests, Priority.GOLD, QuestChooserAI.SMART);
                choosenQuestsThirst.Add(choosenQuest);
                quests = thirstSimulator.NextQuests(choosenQuest, new QuestValue(10000, 10000), 1);
            }
            quests = thirstSimulator.StartThirst(200, new QuestValue(10000, 10000), 1, new List<EventType>());

            while (quests is not null)
            {
                var choosenQuest = questChooser.ChooseBestQuest(quests, Priority.GOLD, QuestChooserAI.SIMPLE);
                choosenQuestsThirst.Add(choosenQuest);
                quests = thirstSimulator.NextQuests(choosenQuest, new QuestValue(10000, 10000), 1);
            }

            foreach (var choosenQuest in choosenQuestsThirst)
            {
                var time = choosenQuest.Time;
                if (time != 2.5 && time != 5 && time != 7.5 && time != 10)
                    Assert.Fail();
            }
        }
    }
}