namespace SFSimulator.Core;

public interface IQuestFactory
{
    Quest CreateTimeMachineQuest(QuestValue minimumQuestValue, decimal thirst, MountType mountType = MountType.Griffin);
}