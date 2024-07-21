namespace SFSimulator.Core
{
    public interface IQuestChooser
    {
        public QuestOptions QuestOptions { get; set; }
        Quest ChooseBestQuest(IEnumerable<Quest> quests);
    }
}