using QuestSimulator.Enums;
using QuestSimulator.Quests;

namespace QuestSimulator.Thirst
{
    public class ThirstSimulator : IThirstSimulator
    {
        private readonly IQuestFactory _questFactory;

        private int MaxBeers { get; set; }
        private int Beers { get; set; } = 0;
        private double CurrentThirst { get; set; }
        public ThirstSimulationOptions ThirstSimulationOptions { get; set; } = new ThirstSimulationOptions();

        private List<EventType>? TavernEvents;

        public ThirstSimulator(IQuestFactory questFactory)
        {
            _questFactory = questFactory;
        }
        public IEnumerable<Quest>? StartThirst(double thirst, QuestValue minQuestValues, int characterLevel, MountType mountType, List<EventType>? currentEvents, bool drinkBeerOneByOne=false)
        {
            if (thirst <= 0) return null;

            TavernEvents = currentEvents;

            if (drinkBeerOneByOne && thirst > 100)
            {
                CurrentThirst = 100;
                Beers = 0;
                MaxBeers = (int)(thirst - 100) / 20;
            }
            else
                CurrentThirst = thirst;

            var generatedQuests = Create3RandomQuests(minQuestValues, characterLevel, mountType);

            return generatedQuests!;
        }
        public IEnumerable<Quest>? NextQuests(Quest previouslyChoosenQuest, QuestValue minQuestValue, int characterLevel, MountType mountType)
        {
            CurrentThirst -= previouslyChoosenQuest.Time;
            if (CurrentThirst==0 && MaxBeers>Beers)
            {
                Beers++;
                CurrentThirst = 20;
            }
            return Create3RandomQuests(minQuestValue, characterLevel, mountType);
        }
        public IEnumerable<Quest> GenerateQuestsFromTimeMachine(double thirst, QuestValue minQuestValue, MountType mountType)
        {
            var quests = new List<Quest>();
            while (thirst > 0)
            {
                    var quest = _questFactory.CreateTimeMachineQuest(minQuestValue, thirst, mountType);
                    quests.Add(quest);
                    thirst -= quest.Time;
            }
            return quests;
        }


        private IEnumerable<Quest>? Create3RandomQuests(QuestValue minQuestValue, int characterLevel, MountType mountType)
        {
            if (CurrentThirst <= 0)
                return null;

            var quests = new List<Quest>();

            if (Beers==10 || Beers == 11)
            {
                quests.Add(
                    _questFactory.CreateBonusQuest(minQuestValue, characterLevel, CurrentThirst, ThirstSimulationOptions.HasGoldScroll, ThirstSimulationOptions.GoldRuneBonus, events: TavernEvents, mountType));
                quests.Add(
                    _questFactory.CreateBonusQuest(minQuestValue, characterLevel, CurrentThirst, ThirstSimulationOptions.HasGoldScroll, ThirstSimulationOptions.GoldRuneBonus, events: TavernEvents, mountType));
                quests.Add(
                    _questFactory.CreateBonusQuest(minQuestValue, characterLevel, CurrentThirst, ThirstSimulationOptions.HasGoldScroll, ThirstSimulationOptions.GoldRuneBonus, events: TavernEvents, mountType));
                return quests;
            }

            quests.Add(
                _questFactory.Create(minQuestValue, characterLevel, CurrentThirst, ThirstSimulationOptions.HasGoldScroll, ThirstSimulationOptions.GoldRuneBonus, events: TavernEvents, mountType));
            quests.Add(
                _questFactory.Create(minQuestValue, characterLevel, CurrentThirst, ThirstSimulationOptions.HasGoldScroll, ThirstSimulationOptions.GoldRuneBonus, events: TavernEvents, mountType));
            quests.Add(
                _questFactory.Create(minQuestValue, characterLevel, CurrentThirst, ThirstSimulationOptions.HasGoldScroll, ThirstSimulationOptions.GoldRuneBonus, events: TavernEvents, mountType));
            return quests;
        }
    }
}
