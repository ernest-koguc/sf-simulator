namespace SFSimulator.Core;

public interface IGemTypeUsageProvider
{
    void Setup(ICollection<DayGemType> gemTypeStrategy);
    GemType GetGemTypeToUse(int day, ClassType classType, IEnumerable<GemType> currentGems);
}