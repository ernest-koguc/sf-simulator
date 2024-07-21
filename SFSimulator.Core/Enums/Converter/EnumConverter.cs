using System.ComponentModel;

namespace SFSimulator.Core;

public static class EnumConverter
{
    public static GemType AttributeToGem(AttributeType attributeType)
    {
        return attributeType switch
        {
            AttributeType.Strength => GemType.Strength,
            AttributeType.Dexterity => GemType.Dexterity,
            AttributeType.Intelligence => GemType.Intelligence,
            AttributeType.Constitution => GemType.Constitution,
            AttributeType.Luck => GemType.Luck,
            _ => throw new InvalidEnumArgumentException(nameof(attributeType))
        };
    }

    public static PotionType AttributeToPotion(AttributeType attributeType)
    {
        return attributeType switch
        {
            AttributeType.Strength => PotionType.Strength,
            AttributeType.Dexterity => PotionType.Dexterity,
            AttributeType.Intelligence => PotionType.Intelligence,
            AttributeType.Constitution => PotionType.Constitution,
            AttributeType.Luck => PotionType.Luck,
            _ => throw new InvalidEnumArgumentException(nameof(attributeType))
        };
    }
}
