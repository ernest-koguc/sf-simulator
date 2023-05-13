using QuestSimulator.Enums;

namespace QuestSimulator.Quests
{
    public interface IQuestChooser
    {
        Quest ChooseBestQuest(IEnumerable<Quest> quests, Priority priority, QuestChooserAI questChooserAI, float? hybridRatio);
    }
}