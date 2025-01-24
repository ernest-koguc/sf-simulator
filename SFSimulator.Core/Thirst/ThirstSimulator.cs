namespace SFSimulator.Core;

public class ThirstSimulator : IThirstSimulator
{
    private readonly IQuestFactory _questFactory;

    public ThirstSimulationOptions ThirstSimulationOptions { get; set; } = new ThirstSimulationOptions();

    public ThirstSimulator(IQuestFactory questFactory)
    {
        _questFactory = questFactory;
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


}
