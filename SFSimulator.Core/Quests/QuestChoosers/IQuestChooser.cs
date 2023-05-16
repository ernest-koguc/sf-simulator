namespace SFSimulator.Core
{
    public interface IQuestChooser
    {
        Quest ChooseBestQuest(IEnumerable<Quest> quests, Priority priority, QuestChooserAI questChooserAI, float? hybridRatio);
    }
}