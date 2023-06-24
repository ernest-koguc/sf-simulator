namespace SFSimulator.Core
{
    public interface IQuestChooser
    {
        Quest ChooseBestQuest(IEnumerable<Quest> quests, QuestPriorityType priority, QuestChooserAI questChooserAI, decimal? hybridRatio);
    }
}