using System.ComponentModel;

namespace SFSimulator.Core;

public static class ItemExtension
{

    public static int GetTotalAttributeValue(this IAttributable item, AttributeType attribute)
    {
        double attributeValue;
        switch (attribute)
        {
            case AttributeType.Strength:
                attributeValue = (int)Math.Ceiling((1D + 0.0356D * item.UpgradeLevel) * item.Strength);
                if (item.GemType == GemType.Strength || item.GemType == GemType.Black || item.GemType == GemType.Legendary)
                {
                    attributeValue += item.GemValue;
                }
                break;
            case AttributeType.Dexterity:
                attributeValue = (int)Math.Ceiling((1D + 0.0356D * item.UpgradeLevel) * item.Dexterity);
                if (item.GemType == GemType.Dexterity || item.GemType == GemType.Black || item.GemType == GemType.Legendary)
                {
                    attributeValue += item.GemValue;
                }
                break;
            case AttributeType.Intelligence:
                attributeValue = (int)Math.Ceiling((1D + 0.0356D * item.UpgradeLevel) * item.Intelligence);
                if (item.GemType == GemType.Intelligence || item.GemType == GemType.Black || item.GemType == GemType.Legendary)
                {
                    attributeValue += item.GemValue;
                }
                break;
            case AttributeType.Constitution:
                attributeValue = (int)Math.Ceiling((1D + 0.0356D * item.UpgradeLevel) * item.Constitution);
                if (item.GemType == GemType.Constitution || item.GemType == GemType.Black || item.GemType == GemType.Legendary)
                {
                    attributeValue += item.GemValue;
                }
                break;
            case AttributeType.Luck:
                attributeValue = (int)Math.Ceiling((1D + 0.0356D * item.UpgradeLevel) * item.Luck);
                if (item.GemType == GemType.Luck || item.GemType == GemType.Black || item.GemType == GemType.Legendary)
                {
                    attributeValue += item.GemValue;
                }
                break;
            default:
                throw new InvalidEnumArgumentException(nameof(attribute));
        }

        return (int)Math.Ceiling(attributeValue);
    }

}
