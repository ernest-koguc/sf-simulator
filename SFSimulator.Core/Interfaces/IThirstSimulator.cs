namespace SFSimulator.Core
{
    public interface IThirstSimulator
    {
        ThirstSimulationOptions ThirstSimulationOptions { get; set; }
        IEnumerable<Quest>? StartThirst(decimal thirst, QuestValue minQuestValues, int characterLevel, List<EventType>? currentEvents);
        IEnumerable<Quest>? NextQuests(Quest previouslyChoosenQuest, QuestValue minQuestValues, int characterLevel);
        IEnumerable<Quest> GenerateQuestsFromTimeMachine(decimal thirst, QuestValue minQuestValue);
    }
}