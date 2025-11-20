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
        var mainAttribute = ClassConfigurationProvider.Get(classType).MainAttribute;
        return mainAttribute switch
        {
            AttributeType.Strength => GemType.Strength,
            AttributeType.Dexterity => GemType.Dexterity,
            AttributeType.Intelligence => GemType.Intelligence,
            _ => throw new NotImplementedException($"Main attribute of {mainAttribute} could not be mapped to gem type")
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