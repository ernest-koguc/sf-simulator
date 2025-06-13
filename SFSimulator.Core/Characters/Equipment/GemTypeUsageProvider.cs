

namespace SFSimulator.Core;

public class GemTypeUsageProvider : IGemTypeUsageProvider
{
    private DayGemType[] DayGemTypes { get; set; }

    public GemTypeUsageProvider()
    {
        DayGemTypes = GetDefaultDayGemTypes();
    }

    public void Setup(ICollection<DayGemType> gemTypeStrategy)
    {
        DayGemTypes = gemTypeStrategy.ToArray();
    }

    public GemType GetGemTypeToUse(int day, ClassType classType, IEnumerable<GemType> currentGems)
    {
        var dayGemType = DayGemTypes.FirstOrDefault(d => d.Day <= day);

        if (dayGemType == default)
            return GetConMainGemType(classType, currentGems);

        return dayGemType.GemType;
    }

    private GemType GetConMainGemType(ClassType classType, IEnumerable<GemType> currentGems)
    {
        var mainGemType = GetMainGemType(classType);
        if (currentGems.Count(g => g == mainGemType) > currentGems.Count(g => g == GemType.Constitution))
            return GemType.Constitution;

        return mainGemType;
    }

    private GemType GetMainGemType(ClassType classType)
    {
        return classType switch
        {
            ClassType.Warrior or ClassType.Bert or ClassType.Berserker or ClassType.BattleMage or ClassType.Paladin => GemType.Strength,
            ClassType.Scout or ClassType.Assassin or ClassType.DemonHunter => GemType.Dexterity,
            ClassType.Mage or ClassType.Bard or ClassType.Druid or ClassType.Necromancer => GemType.Intelligence,
            _ => throw new ArgumentException($"Class type {classType} is not supported", nameof(classType))
        };
    }

    private DayGemType[] GetDefaultDayGemTypes()
    {
        return
        [
            new(45, GemType.Legendary),
        ];
    }
}

public readonly record struct DayGemType(int Day, GemType GemType);