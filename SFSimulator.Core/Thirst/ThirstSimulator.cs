using SFSimulator.Core;

namespace SFSimulator.Core
{
    public class ThirstSimulator : IThirstSimulator
    {
        private readonly IQuestFactory _questFactory;
        private readonly IQuestHelper _questHelper;

        private int MaxBeers { get; set; }
        private int CurrentBeer { get; set; } = 0;
        private decimal CurrentThirst { get; set; }
        public ThirstSimulationOptions ThirstSimulationOptions { get; set; } = new ThirstSimulationOptions();

        private List<EventType>? TavernEvents;

        public ThirstSimulator(IQuestFactory questFactory, IQuestHelper questHelper)
        {
            _questFactory = questFactory;
            _questHelper = questHelper;
        }
        public IEnumerable<Quest>? StartThirst(decimal thirst, QuestValue minQuestValues, int characterLevel, List<EventType>? currentEvents)
        {
            if (thirst <= 0) return null;

            TavernEvents = currentEvents;
            MaxBeers = 0;
            CurrentBeer = 0;
            CurrentThirst = thirst <= 100 ? thirst : 100;

            if (thirst > 100)
            {
                MaxBeers = (int)Math.Round((thirst - 100) / 20);
            }

            var generatedQuests = Create3RandomQuests(minQuestValues, characterLevel);

            return generatedQuests;
        }
        public IEnumerable<Quest>? NextQuests(Quest previouslyChoosenQuest, QuestValue minQuestValue, int characterLevel)
        {
            CurrentThirst -= previouslyChoosenQuest.Time;
            if (CurrentThirst == 0 && MaxBeers > CurrentBeer)
            {
                if (ThirstSimulationOptions.DrinkBeerOneByOne)
                {
                    CurrentBeer++;
                    CurrentThirst = 20;
                }
                else
                {
                    var beersToDrink = MaxBeers - CurrentBeer >= 5 ? 5 : MaxBeers - CurrentBeer;
                    CurrentBeer += beersToDrink;
                    CurrentThirst = beersToDrink * 20;
                }
            }
            return Create3RandomQuests(minQuestValue, characterLevel);
        }
        public IEnumerable<Quest> GenerateQuestsFromTimeMachine(decimal thirst, QuestValue minQuestValue)
        {
            var quests = new List<Quest>();
            while (thirst > 0)
            {
                var quest = _questFactory.CreateTimeMachineQuest(minQuestValue, thirst, ThirstSimulationOptions.Mount);
                quests.Add(quest);
                thirst -= quest.Time;
            }
            return quests;
        }


        private IEnumerable<Quest>? Create3RandomQuests(QuestValue minQuestValue, int characterLevel)
        {
            if (CurrentThirst <= 0)
                return null;

            var quests = new List<Quest>();

            if ((CurrentBeer == 10 || CurrentBeer == 11) && _questHelper.GetTime(4, ThirstSimulationOptions.Mount)>CurrentThirst)
            {
                quests.Add(
                    _questFactory.CreateBonusQuest(minQuestValue, characterLevel, CurrentThirst, ThirstSimulationOptions.HasGoldScroll, ThirstSimulationOptions.GoldRuneBonus, events: TavernEvents, ThirstSimulationOptions.Mount));
                quests.Add(
                    _questFactory.CreateBonusQuest(minQuestValue, characterLevel, CurrentThirst, ThirstSimulationOptions.HasGoldScroll, ThirstSimulationOptions.GoldRuneBonus, events: TavernEvents, ThirstSimulationOptions.Mount));
                quests.Add(
                    _questFactory.CreateBonusQuest(minQuestValue, characterLevel, CurrentThirst, ThirstSimulationOptions.HasGoldScroll, ThirstSimulationOptions.GoldRuneBonus, events: TavernEvents, ThirstSimulationOptions.Mount));
                return quests;
            }

            quests.Add(
                _questFactory.Create(minQuestValue, characterLevel, CurrentThirst, ThirstSimulationOptions.HasGoldScroll, ThirstSimulationOptions.GoldRuneBonus, events: TavernEvents, ThirstSimulationOptions.Mount));
            quests.Add(
                _questFactory.Create(minQuestValue, characterLevel, CurrentThirst, ThirstSimulationOptions.HasGoldScroll, ThirstSimulationOptions.GoldRuneBonus, events: TavernEvents, ThirstSimulationOptions.Mount));
            quests.Add(
                _questFactory.Create(minQuestValue, characterLevel, CurrentThirst, ThirstSimulationOptions.HasGoldScroll, ThirstSimulationOptions.GoldRuneBonus, events: TavernEvents, ThirstSimulationOptions.Mount));
            return quests;
        }
    }
}

public readonly record struct QuestOptions(QuestPriorityType Priority, decimal? HybridRatio);
