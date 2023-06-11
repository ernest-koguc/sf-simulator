namespace SFSimulator.Core
{
    public interface IQuestFactory
    {
        Quest Create(QuestValue minimumQuestValue, int characterLevel, double thirst, bool hasGoldScroll = false, float goldRune = 0, IEnumerable<EventType>? events = null, MountType mountType = MountType.Griffin);
        Quest Create(QuestValue minimumQuestValue, int characterLevel, double thirst, int time, bool hasGoldScroll = false, float goldRune = 0, IEnumerable<EventType>? events = null, MountType mountType = MountType.Griffin);
        Quest CreateBonusQuest(QuestValue minimumQuestValue, int characterLevel, double thirst, bool hasGoldScroll = false, float goldRune = 0, IEnumerable<EventType>? events = null, MountType mountType = MountType.Griffin);
        Quest CreateTimeMachineQuest(QuestValue minimumQuestValue, double thirst, MountType mountType = MountType.Griffin);
    }
}