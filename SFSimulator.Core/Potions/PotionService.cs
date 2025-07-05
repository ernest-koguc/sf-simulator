namespace SFSimulator.Core;
public class PotionService : IPotionService
{
    public List<Potion> GetPotions(ClassType classType)
    {
        List<Potion> potions =
        [
            new() { Size = 25, Type = PotionType.Constitution },
            new() { Size = 15, Type = PotionType.Eternity }
        ];

        var mainAttribute = ClassConfigurationProvider.Get(classType).MainAttribute;
        var mainPotionType = mainAttribute switch
        {
            AttributeType.Strength => PotionType.Strength,
            AttributeType.Intelligence => PotionType.Intelligence,
            AttributeType.Dexterity => PotionType.Dexterity,
            _ => throw new ArgumentOutOfRangeException($"{mainAttribute} is not a supported attribute for {nameof(PotionType)}")
        };

        potions.Add(new() { Size = 25, Type = mainPotionType });

        return potions;
    }
}
