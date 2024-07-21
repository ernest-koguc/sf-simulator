using System.ComponentModel;

namespace SFSimulator.Core;

public interface ITotalStatsCalculatable
{
    bool IsCompanion { get; }
    ClassType Class { get; }
    int BaseStrength { get; }
    int BaseDexterity { get; }
    int BaseIntelligence { get; }
    int BaseConstitution { get; }
    int BaseLuck { get; }

    PetsState Pets { get; }
    EquipmentItem? FirstWeapon { get; }
    EquipmentItem? SecondWeapon { get; }
    FightableItems Items { get; }
    List<Potion> Potions { get; }
}

public static class ITotalStatsCalculatableExtensions
{
    public static int GetTotalStatsFor(this ITotalStatsCalculatable totalStatsCalculatable, AttributeType attributeType)
    {
        var items = totalStatsCalculatable.Items.SimpleList;

        var attributesFromItems = items.Sum(i => GetSumOfItem(attributeType, i, totalStatsCalculatable.Class, totalStatsCalculatable.IsCompanion));
        attributesFromItems = (int)(attributesFromItems * ClassConfigurationProvider.GetClassConfiguration(totalStatsCalculatable.Class).ItemBonusMultiplier);

        var baseAttributes = attributeType switch
        {
            AttributeType.Strength => totalStatsCalculatable.BaseStrength,
            AttributeType.Dexterity => totalStatsCalculatable.BaseDexterity,
            AttributeType.Intelligence => totalStatsCalculatable.BaseIntelligence,
            AttributeType.Constitution => totalStatsCalculatable.BaseConstitution,
            AttributeType.Luck => totalStatsCalculatable.BaseLuck,
            _ => throw new InvalidEnumArgumentException(nameof(attributeType))
        };

        var potionType = EnumConverter.AttributeToPotion(attributeType);
        var petBonus = totalStatsCalculatable.Pets.GetPetBonusFor(attributeType);

        var totalAttributes = (baseAttributes + attributesFromItems) * petBonus;

        var potionSize = totalStatsCalculatable.Potions.FirstOrDefault(p => p.Type == potionType)?.Size;
        if (potionSize.HasValue)
            totalAttributes *= 1 + (potionSize.Value / 100D);

        return (int)totalAttributes;
    }

    private static int GetSumOfItem(AttributeType attributeType, EquipmentItem item, ClassType classType, bool isCompanion)
    {
        var gemValue = item.GemType switch
        {
            GemType.None => 0,
            GemType.Black => item.GemValue,
            GemType.Legendary => GetLegendaryGemValue(attributeType, item.GemValue, classType),
            GemType.Strength or GemType.Dexterity or GemType.Intelligence or GemType.Constitution or GemType.Luck =>
                item.GemType == EnumConverter.AttributeToGem(attributeType) ? item.GemValue : 0,
            _ => throw new InvalidEnumArgumentException(nameof(item.GemType))
        };

        if (!isCompanion && item.ItemType == ItemType.Weapon)
        {
            var weaponGemMultiplier = ClassConfigurationProvider.GetClassConfiguration(classType).WeaponGemMultiplier;
            gemValue *= weaponGemMultiplier;
        }

        var attribute = item[attributeType];

        var attributesWithUpgrades = attribute * Math.Pow(1.03D, item.UpgradeLevel);
        var totalAttributes = (int)attributesWithUpgrades + gemValue;

        return totalAttributes;
    }

    private static int GetLegendaryGemValue(AttributeType attributeType, int gemValue, ClassType classType)
    {
        var classMainAttribute = ClassConfigurationProvider.GetClassConfiguration(classType).MainAttribute;
        return attributeType == classMainAttribute || attributeType == AttributeType.Constitution ? gemValue : 0;
    }
}