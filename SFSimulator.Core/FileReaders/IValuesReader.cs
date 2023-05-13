
using QuestSimulator.Quests;

namespace QuestSimulator.FileReaders
{
    public interface IValuesReader
    {
        Dictionary<int, int> ReadExperienceForNextLevel();

        Dictionary<int, float> ReadItemGoldValues();
    }
}