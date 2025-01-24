namespace SFSimulator.Core;

public interface IThirstSimulator
{
    ThirstSimulationOptions ThirstSimulationOptions { get; set; }
    IEnumerable<Quest> GenerateQuestsFromTimeMachine(decimal thirst, QuestValue minQuestValue);
}