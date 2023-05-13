using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuestSimulator.Enums;
using QuestSimulator.FileReaders;
using QuestSimulator.Items;
using QuestSimulator.Quests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestSimulatorTests
{
    [TestClass]
    public class QuestFactoryTests
    {
        [TestMethod]
        public void Create_returns_quest_with_proper_gold_and_experience_value()
        {
            var questFactory = new QuestFactory(new QuestHelper(), new ItemGenerator(new Random(0), new ValuesReader()), new Random(0));
            for (int i = 0; i < 100; i++)
            {
                var quest = questFactory.Create(new QuestValue(10000000000, 100000000), 1, 1);
                Assert.IsTrue(quest.Gold <= 10000000 && quest.Gold > 0);
                quest = questFactory.Create(new QuestValue(-10000, -10000), 1, 1);
                Assert.IsTrue(quest.Experience >= 0);
                Assert.IsTrue(quest.Gold >= 0);
                quest = questFactory.Create(new QuestValue(10000000000, 100000000), 1, 1, true, 0.5f);
                Assert.IsTrue(quest.Gold <= 16500000);
                quest = questFactory.Create(new QuestValue(10000000000, 100000000), 1, 1, false, 0.5f);
                Assert.IsTrue(quest.Gold <= 15000000);
                quest = questFactory.Create(new QuestValue(10000000000, 100000000), 1, 1, true, 0.3f);
                Assert.IsTrue(quest.Gold <= 14300000);
                quest = questFactory.Create(new QuestValue(10000000000, 100000000), 1, 1, true, 0.5f, new List<EventType> { EventType.GOLD });
                Assert.IsTrue(quest.Gold <= 16500000);
            }
        }
        [TestMethod]
        public void Create_returns_quest_with_proper_time()
        {
            for (int i = 0; i < 100; i++)
            {
                var questFactory = new QuestFactory(new QuestHelper(), new ItemGenerator(new Random(), new ValuesReader()), new Random());
                var quest = questFactory.Create(new QuestValue(100, 100), 1, 20);
                Assert.IsTrue(quest.Time <= 10);
                Assert.IsTrue(quest.Time > 0);
                Assert.IsTrue(quest.Time == 2.5 || quest.Time == 5 || quest.Time == 7.5 || quest.Time == 10);
            }
        }
        [TestMethod]
        public void Create_returns_quest_with_items()
        {
            var random = new Random(0);
            var questList = new List<Quest>();
            var questFactory = new QuestFactory(new QuestHelper(), new ItemGenerator(random, new ValuesReader()), random);

            for (int i = 0; i < 100; i++)
            {
                questList.Add(questFactory.Create(new QuestValue(100, 100), 1, 20));
            }

            Assert.IsTrue(questList.Any(q => q.Item != null));
        }
        [TestMethod]
        public void Create_returns_quset_with_double_xp_on_XPEvent()
        {
            var random = new Random(0);
            var random2 = new Random(0);
            var questFactory = new QuestFactory(new QuestHelper(), new ItemGenerator(random, new ValuesReader()), random);
            var questFactory2 = new QuestFactory(new QuestHelper(), new ItemGenerator(random2, new ValuesReader()), random2);

            for (int i = 0; i < 100; i++)
            {
                var normalQuest = questFactory.Create(new QuestValue(100, 100), 1, 20);
                var eventQuest = questFactory2.Create(new QuestValue(100, 100), 1, 20, false, 0, new List<EventType> { EventType.EXPERIENCE });

                Assert.IsTrue(normalQuest.Experience == eventQuest.Experience / 2);
            }
        }
        [TestMethod]
        public void Create_returns_quset_with_quintuple_gold_on_GoldEvent()
        {
            var random = new Random(0);
            var random2 = new Random(0);
            var questFactory = new QuestFactory(new QuestHelper(), new ItemGenerator(random, new ValuesReader()), random);
            var questFactory2 = new QuestFactory(new QuestHelper(), new ItemGenerator(random2, new ValuesReader()), random2);

            for (int i = 0; i < 100; i++)
            {
                var normalQuest = questFactory.Create(new QuestValue(100, 100), 1, 20);
                var eventQuest = questFactory2.Create(new QuestValue(100, 100), 1, 20, false, 0, new List<EventType> { EventType.GOLD });

                var normalGold = Math.Floor(normalQuest.Gold);
                var eventGold = Math.Floor(eventQuest.Gold / 5);

                Assert.IsTrue(normalGold == eventGold);
            }
        }
        [TestMethod]
        public void Create_returns_truncuated_quest()
        {
            var random = new Random(0);
            var questFactory = new QuestFactory(new QuestHelper(), new ItemGenerator(random, new ValuesReader()), random);

            var questList = new List<Quest>();

            for (int i = 0; i < 100; i++)
            {
                var quest = questFactory.Create(new QuestValue(1000000000000000, 100), 1, 2.5);
                questList.Add(quest);
            }
            Assert.IsTrue(questList.Any(q => q.Gold != 16500000));
        }
        [TestMethod]
        public void Create_returns_not_truncuated_quest_if_thirst_is_above_or_equal_10()
        {
            var random = new Random(0);
            var questFactory = new QuestFactory(new QuestHelper(), new ItemGenerator(random, new ValuesReader()), random);

            var questList = new List<Quest>();

            for (int i = 0; i < 100; i++)
            {
                var quest = questFactory.Create(new QuestValue(1000000000000000, 100), 1, 10, true, 0.5f);
                questList.Add(quest);
            }
            Assert.IsFalse(questList.Any(q => q.Gold != 16500000));
        }
        [TestMethod]
        public void CreateBonusQuest_returns_not_truncuated_quest()
        {
            var random = new Random(0);
            var questFactory = new QuestFactory(new QuestHelper(), new ItemGenerator(random, new ValuesReader()), random);
            var questList = new List<Quest>();

            for (int i = 0; i < 100; i++)
            {
                var quest = questFactory.CreateBonusQuest(new QuestValue(1000000000000000, 100), 1, 2.5, true, 0.5f);
                questList.Add(quest);
            }

            Assert.IsTrue(questList.All(q => q.Gold == 16500000));
        }
        [TestMethod]
        public void Create_has_estimated_10percent_to_return_quest_with_item_generated_before_quest()
        {
            var random = new Random(0);
            var questFactory = new QuestFactory(new QuestHelper(), new ItemGenerator(random, new ValuesReader()), random);
            var questList = new List<Quest>();

            int i = 0;

            for (; i < 1000; i++)
            {
                var quest = questFactory.CreateBonusQuest(new QuestValue(1000000000000000, 100), 1, 20, true, 0.5f);
                questList.Add(quest);
            }

            var count = questList.Count(q => q.Item != null && q.Item.ItemSourceType == ItemSourceType.BeforeQuest);

            Assert.IsTrue(count >= i*0.095 && count <= i*0.105);
        }
        [TestMethod]
        public void Create_has_estimated_9percent_to_return_quest_with_pet_item()
        {
            var random = new Random(0);
            var questFactory = new QuestFactory(new QuestHelper(), new ItemGenerator(random, new ValuesReader()), random);
            var questList = new List<Quest>();

            int i = 0;

            for (; i < 1000; i++)
            {
                var quest = questFactory.CreateBonusQuest(new QuestValue(1000000000000000, 100), 1, 20, true, 0.5f);
                questList.Add(quest);
            }

            var count = questList.Count(q => q.Item != null && q.Item.ItemType == ItemType.PetFood);

            Assert.IsTrue(count >= i * 0.085 && count <= i * 0.095);
        }
        [TestMethod]
        public void Create_has_estimated_8percent_to_return_quest_with_item_generated_after_quest()
        {
            var random = new Random(0);
            var questFactory = new QuestFactory(new QuestHelper(), new ItemGenerator(random, new ValuesReader()), random);
            var questList = new List<Quest>();

            int i = 0;

            for (; i < 1000; i++)
            {
                var quest = questFactory.CreateBonusQuest(new QuestValue(1000000000000000, 100), 1, 20, true, 0.5f);
                questList.Add(quest);
            }

            var count = questList.Count(q => q.Item != null && q.Item.ItemSourceType == ItemSourceType.AfterQuest && q.Item.ItemType!= ItemType.PetFood);

            Assert.IsTrue(count >= i * 0.075 && count <= i * 0.087);
        }
    }
}
