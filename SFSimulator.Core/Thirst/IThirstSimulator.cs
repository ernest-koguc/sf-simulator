namespace SFSimulator.Core
{
    public interface IThirstSimulator
    {
        ThirstSimulationOptions ThirstSimulationOptions { get; set; }
        IEnumerable<Quest>? StartThirst(double thirst, QuestValue minQuestValues, int characterLevel, MountType mountType, List<EventType>? currentEvents, bool drinkBeerOneByOne = false);
        IEnumerable<Quest>? NextQuests(Quest previouslyChoosenQuest, QuestValue minQuestValues, int characterLevel, MountType mountType);
        IEnumerable<Quest> GenerateQuestsFromTimeMachine(double thirst, QuestValue minQuestValue, MountType mountType);
    }
}