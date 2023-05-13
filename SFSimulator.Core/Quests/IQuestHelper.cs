using QuestSimulator.Enums;

namespace QuestSimulator.Quests
{
    public interface IQuestHelper
    {
        float GetTime(int questLength, MountType mountType);
    }
}