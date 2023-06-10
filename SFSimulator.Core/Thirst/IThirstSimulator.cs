namespace SFSimulator.Core
{
    public interface IThirstSimulator
    {
        ThirstSimulationOptions ThirstSimulationOptions { get; set; }
        IEnumerable<Quest>? StartThirst(double thirst, QuestValue minQuestValues, int characterLevel, List<EventType>? currentEvents);
        IEnumerable<Quest>? NextQuests(Quest previouslyChoosenQuest, QuestValue minQuestValues, int characterLevel);
        IEnumerable<Quest> GenerateQuestsFromTimeMachine(double thirst, QuestValue minQuestValue);
    }
}