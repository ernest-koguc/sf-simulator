using System.ComponentModel;

namespace SFSimulator.Core;

/// <summary>
/// Builder for creating equipment items.
/// </summary>
/// <remarks>
/// The builder is used to create equipment items with specific attributes, armor, gems, and runes.
/// </remarks>
/// <exception cref="InvalidEnumArgumentException">Thrown when the class type is not supported.</exception>
/// <exception cref="InvalidEnumArgumentException">Thrown when the item attribute type is not supported.</exception>
/// <exception cref="InvalidEnumArgumentException">Thrown when the class type is not supported.</exception>
/// <exception cref="InvalidEnumArgumentException">Thrown when the item attribute type is not supported.</exception>
public class EquipmentBuilder(ItemAttributeType itemAttributeType, int characterLevel, ClassType classType, int aura,
            int itemQualityRuneValue, ItemType itemType)
{
    private EquipmentItem EquipmentItem { get; set; } = new EquipmentItem { ItemAttributeType = itemAttributeType, ItemType = itemType };
    private readonly int ItemQuality = (int)Math.Round((3 + characterLevel + aura) * (1 + itemQualityRuneValue / 100M));

    /// <summary>
    /// Adds attributes to the equipment item based on builder configuration.
    /// <returns>The equipment builder for easy chaining.</returns>
    /// </summary>
    /// <remarks>
    /// The attributes are calculated based on the character level, item quality, scrolls unlocked, and aura.
    /// </remarks>
    /// <exception cref="InvalidEnumArgumentException">Thrown when the class type is not supported.</exception>
    /// <exception cref="InvalidEnumArgumentException">Thrown when the item attribute type is not supported.</exception>
    public EquipmentBuilder WithAttributes()
    {
        var itemAttributes = GetItemAttributes();

        EquipmentItem.Strength = itemAttributes.Strength;
        EquipmentItem.Dexterity = itemAttributes.Dexterity;
        EquipmentItem.Intelligence = itemAttributes.Intelligence;
        EquipmentItem.Constitution = itemAttributes.Constitution;
        EquipmentItem.Luck = itemAttributes.Luck;

        return this;
    }

    /// <summary>
    /// Adds armor to the equipment item based on builder configuration.
    /// <returns>The equipment builder for easy chaining.</returns>
    /// </summary>
    /// <remarks>
    /// The armor is calculated based on the character level, item quality, scrolls unlocked, and aura.
    /// </remarks>
    /// <returns>The equipment builder for easy chaining.</returns>
    /// <exception cref="InvalidEnumArgumentException">Thrown when the class type is not supported.</exception>
    public EquipmentBuilder WithArmor()
    {
        if (!CanHaveArmor(itemType)) return this;

        double initialArmor = ItemQuality;

        if (itemAttributeType == ItemAttributeType.Legendary)
            initialArmor = initialArmor * 1.024;

        var armorMultiplier = ClassConfigurationProvider.Get(classType).ItemArmorMultiplier;
        var armor = initialArmor * armorMultiplier;

        EquipmentItem.Armor = (int)armor;
        return this;
    }

    /// <summary>
    /// Adds a gem to the equipment item with a specificed <paramref name="gemType"/> type and value calculated based on builder configuration
    /// and <paramref name="gemMineLevel"/> and <paramref name="knights"/>.
    /// </summary>
    /// <remarks>
    /// The gem value is calculated based on the character level, gem mine level, knights, and gem type. Always biggest gem size is considered.
    /// </remarks>
    /// <returns>The equipment builder for easy chaining.</returns>
    /// <exception cref="InvalidEnumArgumentException">Thrown when the gem type is not supported.</exception>
    public EquipmentBuilder WithGem(GemType gemType, int gemMineLevel, int knights)
    {
        if (gemType == GemType.None || itemType == ItemType.Shield)
        {
            EquipmentItem.GemType = GemType.None;
            EquipmentItem.HasSocket = itemType != ItemType.Shield;
            EquipmentItem.GemValue = 0;
            return this;
        }

        var gemFactor = 0.4D * characterLevel;
        var knightsAddition = knights / 3D;
        var gemMineFactor = 1 + (gemMineLevel - 1) * 0.15D;

        var gemValue = gemFactor * gemMineFactor + knightsAddition;

        switch (gemType)
        {
            case GemType.Strength:
            case GemType.Dexterity:
            case GemType.Intelligence:
            case GemType.Constitution:
            case GemType.Luck:
                break;
            case GemType.Legendary:
                gemValue *= 0.95D;
                break;
            case GemType.Black:
                gemValue *= 2 / 3D;
                break;
            default:
                throw new InvalidEnumArgumentException(nameof(gemType));
        }
        EquipmentItem.GemType = gemType;
        EquipmentItem.HasSocket = true;
        EquipmentItem.GemValue = (int)gemValue;
        return this;
    }

    /// <summary>
    /// Adds a rune to the equipment item with a specificed <paramref name="runeType"/> type and <paramref name="runeValue"/> value.
    /// <returns>The equipment builder for easy chaining.</returns>
    /// </summary>
    public EquipmentBuilder WithRune(RuneType runeType, int runeValue)
    {

        EquipmentItem.RuneValue = runeValue;
        EquipmentItem.RuneType = runeType;
        return this;
    }

    public EquipmentBuilder WithEnchantment()
    {
        EquipmentItem.ScrollType = itemType switch
        {
            ItemType.Headgear => WitchScrollType.QuestExperience,
            ItemType.Breastplate => WitchScrollType.QuestMushroom,
            ItemType.Gloves => WitchScrollType.Reaction,
            ItemType.Boots => WitchScrollType.QuestSpeed,
            ItemType.Amulet => WitchScrollType.QuestItems,
            ItemType.Belt => WitchScrollType.Beer,
            ItemType.Ring => WitchScrollType.QuestGold,
            ItemType.Trinket => WitchScrollType.ArenaGold,
            ItemType.Weapon => WitchScrollType.Crit,
            _ => WitchScrollType.None
        };

        return this;
    }

    /// <summary>
    /// Adds minimum and maximum damage to the blueprint item with a specificed <paramref name="range"/> range.
    /// <param name="range">The range of the weapon. Must be between 1 and 1.75</param>
    /// <returns>The equipment builder for easy chaining.</returns>
    /// </summary>
    public EquipmentBuilder AsWeapon(double range = 1.5D)
    {
        if (range is > 1.5D or <= 1D)
            throw new ArgumentOutOfRangeException(nameof(range), "Range should be between 1 and 1.5");

        var startingAvgDmg = Math.Min(ItemQuality, 1000D) * 2 + Math.Max(ItemQuality - 1000, 0D);


        if (itemAttributeType == ItemAttributeType.Legendary)
        {
            startingAvgDmg = startingAvgDmg * 1.024D;
        }

        var weaponMultiplier = ClassConfigurationProvider.Get(classType).WeaponMultiplier / 2;
        var averageDamage = Math.Round(startingAvgDmg * weaponMultiplier);
        var min = (int)Math.Round((2 - range) * averageDamage);
        var max = (int)Math.Round(range * averageDamage);
        EquipmentItem.MinDmg = min;
        EquipmentItem.MaxDmg = max;
        EquipmentItem.ItemType = ItemType.Weapon;

        var attributesMultiplier = ClassConfigurationProvider.Get(classType).WeaponAttributeMultiplier;
        EquipmentItem.Strength *= attributesMultiplier;
        EquipmentItem.Dexterity *= attributesMultiplier;
        EquipmentItem.Intelligence *= attributesMultiplier;
        EquipmentItem.Constitution *= attributesMultiplier;
        EquipmentItem.Luck *= attributesMultiplier;

        return this;
    }

    /// <summary>
    /// Builds the equipment item.
    /// <returns>The equipment item.</returns>
    /// </summary>
    public EquipmentItem Build()
    {
        EquipmentItem.ItemClassType = classType;
        return EquipmentItem;
    }

    /// <summary>
    /// Creates an equipment item from a <see cref="SFToolsItem"/>.
    /// <returns>The equipment item.</returns>
    /// </summary>
    public static EquipmentItem FromSFToolsItem(ClassType classType, SFToolsItem item)
    {
        ItemType[] armorItemTypes = [ItemType.Headgear, ItemType.Breastplate, ItemType.Gloves, ItemType.Boots, ItemType.Belt];
        var equipmentItem = new EquipmentItem
        {
            ItemClassType = classType,
            ItemType = item.Type,
            MinDmg = item.Type is ItemType.Weapon ? item.DamageMin : 0,
            MaxDmg = item.Type is ItemType.Weapon ? item.DamageMax : 0,
            Armor = armorItemTypes.Contains(item.Type) ? item.Armor : 0,
            ScrollType = item.Enchantment,
            RuneType = item.RuneType,
            RuneValue = item.RuneValue,
            UpgradeLevel = item.Upgrades,
            GemValue = item.GemValue,
            GemType = item.GemType
        };
        var firstAttribute = item.Attributes[0];
        var secondAttribute = item.Attributes[1];

        if (item.UpgradeMultiplier != 0)
        {
            firstAttribute = (int)(firstAttribute / item.UpgradeMultiplier);
            secondAttribute = (int)(secondAttribute / item.UpgradeMultiplier);
        }

        switch (item.AttributeTypes)
        {
            case [21, 0, _]:
                equipmentItem.ItemAttributeType = item.Index >= 100 ? ItemAttributeType.Legendary : ItemAttributeType.Epic;
                equipmentItem.Strength = firstAttribute;
                equipmentItem.Constitution = firstAttribute;
                equipmentItem.Luck = firstAttribute;
                break;
            case [22, 0, _]:
                equipmentItem.ItemAttributeType = item.Index >= 100 ? ItemAttributeType.Legendary : ItemAttributeType.Epic;
                equipmentItem.Dexterity = firstAttribute;
                equipmentItem.Constitution = firstAttribute;
                equipmentItem.Luck = firstAttribute;
                break;
            case [23, 0, _]:
                equipmentItem.ItemAttributeType = item.Index >= 100 ? ItemAttributeType.Legendary : ItemAttributeType.Epic;
                equipmentItem.Intelligence = firstAttribute;
                equipmentItem.Constitution = firstAttribute;
                equipmentItem.Luck = firstAttribute;
                break;
            case [6, 0, _]:
                equipmentItem.ItemAttributeType = ItemAttributeType.EpicAllAttributes;
                equipmentItem.Strength = firstAttribute;
                equipmentItem.Dexterity = firstAttribute;
                equipmentItem.Intelligence = firstAttribute;
                equipmentItem.Constitution = firstAttribute;
                equipmentItem.Luck = firstAttribute;
                break;
            case [7, 0, _]:
                equipmentItem.ItemAttributeType = ItemAttributeType.EpicLuck;
                equipmentItem.Luck = firstAttribute;
                break;
            case [var first, var second, _] when first != 0 && second != 0:
                equipmentItem.ItemAttributeType = ItemAttributeType.NormalTwoStats;
                equipmentItem[(AttributeType)first] = firstAttribute;
                equipmentItem[(AttributeType)second] = secondAttribute;
                break;
            case [var first, _, _] when first != 0:
                equipmentItem.ItemAttributeType = ItemAttributeType.NormalOneStat;
                equipmentItem[(AttributeType)first] = firstAttribute;
                break;

            default:
                break;
        }

        return equipmentItem;
    }

    public static RawWeapon ToRawWeapon(EquipmentItem item)
    {
        if (item.ItemType != ItemType.Weapon)
            throw new ArgumentException("Item is not a weapon", nameof(item));

        return new RawWeapon
        {
            MinDmg = item.MinDmg,
            MaxDmg = item.MaxDmg,
            RuneType = item.RuneType,
            RuneValue = item.RuneValue
        };
    }

    /// <summary>
    /// Compares equipment item <paramref name="x"/> with this instance.
    /// <returns>1 if this instance is better, 0 if they are equal, -1 if <paramref name="x"/> is better.</returns>
    /// </summary>
    public int Compare(EquipmentItem? x, IComparer<EquipmentItem> comparer) => comparer.Compare(EquipmentItem, x);

    private ItemAttributesGroup GetItemAttributes()
    {
        if (ItemQuality >= 350)
        {
            var baseAttribute = 0.001756211D * Math.Pow(ItemQuality, 2) + 1.04214299D * ItemQuality;
            return itemAttributeType switch
            {
                // TODO: make sure that method implementations are correct
                ItemAttributeType.NormalOneStat => GetNormalOneStatItemAttributes((int)Math.Round(baseAttribute * 1.25)),
                ItemAttributeType.NormalTwoStats => GetNormalTwoStatItemAttributes((int)Math.Round(baseAttribute * 5 / 3D / 2)),
                ItemAttributeType.Epic => GetThreeStatItemAttributes((int)Math.Round(baseAttribute)),
                ItemAttributeType.EpicAllAttributes => GetFiveStatItemAttributes((int)Math.Round(baseAttribute * 5 / 6D)),
                ItemAttributeType.Legendary => GetThreeStatItemAttributes((int)Math.Round(baseAttribute * 1.08)),
                _ => throw new InvalidEnumArgumentException($"{nameof(itemAttributeType)} with value {itemAttributeType} is unsupported")
            };

        }

        var itemQuality = ItemQuality;

        return itemAttributeType switch
        {
            ItemAttributeType.NormalOneStat => GetNormalOneStatItemAttributes(itemQuality * 2),
            ItemAttributeType.NormalTwoStats => GetNormalTwoStatItemAttributes(itemQuality),
            ItemAttributeType.Epic => GetThreeStatItemAttributes((int)(itemQuality * 1.2)),
            ItemAttributeType.Legendary => GetThreeStatItemAttributes((int)(itemQuality * 1.2 * 1.08)),
            _ => GetFiveStatItemAttributes(itemQuality),
        };
    }

    private ItemAttributesGroup GetNormalOneStatItemAttributes(int baseAttributes)
    {
        return classType switch
        {
            ClassType.Bard or ClassType.Mage or ClassType.Necromancer or ClassType.Druid => new ItemAttributesGroup(0, 0, baseAttributes, 0, 0),
            ClassType.Warrior or ClassType.BattleMage or ClassType.Berserker or ClassType.Bert or ClassType.Paladin => new ItemAttributesGroup(baseAttributes, 0, 0, 0, 0),
            ClassType.Scout or ClassType.Assassin or ClassType.DemonHunter => new ItemAttributesGroup(0, baseAttributes, 0, 0, 0),
            _ => throw new InvalidEnumArgumentException($"{nameof(classType)} with value {classType} is unsupported")
        };
    }

    private ItemAttributesGroup GetNormalTwoStatItemAttributes(int baseAttributes)
    {
        var itemAttributes = GetNormalOneStatItemAttributes(baseAttributes);
        var twoStatItemAttributes = itemAttributes with { Constitution = baseAttributes };
        return twoStatItemAttributes;
    }

    private ItemAttributesGroup GetThreeStatItemAttributes(int baseAttributes)
    {
        var itemAttributes = GetNormalOneStatItemAttributes(baseAttributes);
        var threeStatItemAttributes = itemAttributes with { Constitution = baseAttributes, Luck = baseAttributes };
        return threeStatItemAttributes;
    }

    private static ItemAttributesGroup GetFiveStatItemAttributes(int baseAttributes) => new(baseAttributes, baseAttributes, baseAttributes, baseAttributes, baseAttributes);

    private static bool CanHaveArmor(ItemType itemType)
        => itemType switch
        {
            ItemType.Headgear or ItemType.Breastplate or ItemType.Gloves or ItemType.Boots or ItemType.Belt => true,
            _ => false
        };
}

public readonly record struct ItemAttributesGroup(int Strength, int Dexterity, int Intelligence, int Constitution, int Luck);