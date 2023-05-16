namespace SFSimulator.Core
{
    public interface IValuesReader
    {
        Dictionary<int, int> ReadExperienceForNextLevel();

        Dictionary<int, float> ReadItemGoldValues();
    }
}