namespace SFSimulator.Core
{
    public interface IQuestFactory
    {
        Quest Create(QuestValue minimumQuestValue, int characterLevel, decimal thirst, bool hasGoldScroll = false, decimal goldRune = 0, IEnumerable<EventType>? events = null, MountType mountType = MountType.Griffin);
        Quest Create(QuestValue minimumQuestValue, int characterLevel, decimal thirst, int time, bool hasGoldScroll = false, decimal goldRune = 0, IEnumerable<EventType>? events = null, MountType mountType = MountType.Griffin);
        Quest CreateBonusQuest(QuestValue minimumQuestValue, int characterLevel, decimal thirst, bool hasGoldScroll = false, decimal goldRune = 0, IEnumerable<EventType>? events = null, MountType mountType = MountType.Griffin);
        Quest CreateTimeMachineQuest(QuestValue minimumQuestValue, decimal thirst, MountType mountType = MountType.Griffin);
    }
}