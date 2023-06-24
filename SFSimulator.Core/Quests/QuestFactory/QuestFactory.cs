namespace SFSimulator.Core
{
    public class QuestFactory : IQuestFactory
    {
        private readonly IQuestHelper _questHelper;
        private readonly IItemGenerator _itemGeneartor;
        private readonly Random _random;
        public QuestFactory(IQuestHelper questHelper, IItemGenerator itemGenerator, Random random)
        {
            _questHelper = questHelper;
            _itemGeneartor = itemGenerator;
            _random = random;
        }
        public Quest Create(QuestValue minimumQuestValue, int characterLevel, decimal thirst, bool hasGoldScroll = false, decimal goldRune = 0, IEnumerable<EventType>? events = null, MountType mountType = MountType.Griffin)
        {
            var time = _random.Next(1, 5);
            return Create(minimumQuestValue, characterLevel, thirst, time, hasGoldScroll, goldRune, events, mountType);
        }
        public Quest Create(QuestValue minimumQuestValue, int characterLevel, decimal thirst, int time, bool hasGoldScroll = false, decimal goldRune = 0, IEnumerable<EventType>? events = null, MountType mountType = MountType.Griffin)
        {
            var convertedTime = _questHelper.GetTime(time, mountType);
            var randomMultiplier = (decimal)(_random.NextDouble() + _random.Next(1, 5));
            var gold = randomMultiplier * minimumQuestValue.Gold * time;

            var experience = (6 - randomMultiplier) * minimumQuestValue.Experience * time;

            if (events is not null && events.Contains(EventType.Gold))
                gold *= 5;

            if (events is not null && events.Contains(EventType.Experience))
                experience *= 2;

            gold = gold < 0 ? 0 : gold;
            experience = experience < 0 ? 0 : experience;
            gold = gold > 10000000 ? 10000000 : gold;

            gold = hasGoldScroll ? gold * 1.1M * (1 + goldRune) : gold * (1 + goldRune);


            //Truncate too long quests time and also their xp and gold
            if (convertedTime > thirst)
            {
                gold *= thirst / convertedTime;
                experience *= thirst / convertedTime;
                convertedTime = thirst;
            }

            Item? item = GenerateItem(characterLevel);

            return new Quest(minimumQuestValue, gold, experience, convertedTime, item);
        }

        public Quest CreateBonusQuest(QuestValue minimumQuestValue, int characterLevel, decimal thirst, bool hasGoldScroll = false, decimal goldRune = 0, IEnumerable<EventType>? events = null, MountType mountType = MountType.Griffin)
        {
            var time = _random.Next(1, 5);
            var convertedTime = _questHelper.GetTime(time, mountType);
            var randomMultiplier = (decimal)(_random.NextDouble() + _random.Next(1, 5));
            var gold = randomMultiplier * minimumQuestValue.Gold * time;

            var experience = (6 - randomMultiplier) * minimumQuestValue.Experience * time;

            if (events is not null && events.Contains(EventType.Gold))
                gold *= 5;

            if (events is not null && events.Contains(EventType.Experience))
                experience *= 2;


            experience = experience < 0 ? 0 : experience;
            gold = gold < 0 ? 0 : gold;
            gold = gold > 10000000 ? 10000000 : gold;

            gold = hasGoldScroll ? gold * 1.1M * (1 + goldRune) : gold * (1 + goldRune);

            //Truncate too long quests time only (xp and gold for bonus quest is untouched)
            if (convertedTime > thirst)
            {
                convertedTime = thirst;
            }

            Item? item = GenerateItem(characterLevel);

            return new Quest(minimumQuestValue, gold, experience, convertedTime, item);
        }

        public Quest CreateTimeMachineQuest(QuestValue minimumQuestValue, decimal thirst, MountType mountType = MountType.Griffin)
        {
            var time = _random.Next(1, 5);
            var convertedTime = _questHelper.GetTime(time, mountType);
            var randomMultiplier = (decimal)(_random.NextDouble() + _random.Next(1, 5));
            var gold = randomMultiplier * minimumQuestValue.Gold * time;

            var experience = (6 - randomMultiplier) * minimumQuestValue.Experience * time;

            gold = gold < 0 ? 0 : gold;
            experience = experience < 0 ? 0 : experience;
            gold = gold > 16500000 ? 16500000 : gold;

            //Truncate too long quests time and also their xp and gold
            if (convertedTime > thirst)
            {
                gold *= thirst / convertedTime;
                experience *= thirst / convertedTime;
                convertedTime = thirst;
            }

            return new Quest(minimumQuestValue, gold, experience, convertedTime);
        }

        private Item? GenerateItem(int characterLevel)
        {
            var randomNumber = _random.Next(0, 10);

            if (randomNumber == 0)
                return _itemGeneartor.GenerateItem(characterLevel, ItemSourceType.BeforeQuest);

            randomNumber = _random.Next(0, 10);

            if (randomNumber == 0)
                return _itemGeneartor.GeneratePetFoodItem();

            randomNumber = _random.Next(0, 10);

            if (randomNumber == 0)
                return _itemGeneartor.GenerateItem(characterLevel, ItemSourceType.AfterQuest);

            return null;
        }
    }
}
