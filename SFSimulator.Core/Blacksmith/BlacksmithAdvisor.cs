namespace SFSimulator.Core;

public class BlackSmithAdvisor : IBlackSmithAdvisor
{
    public BlackSmithResources DismantleItem(EquipmentItem item)
    {
        var attributes = (double)item.Attributes.Max();

        for (var i = 0; i < item.UpgradeLevel; i++)
        {
            attributes = Math.Round(attributes / 1.04D);
        }

        if (item.ItemAttributeType == ItemAttributeType.EpicAllAttributes)
            attributes *= 1.2D;

        if (item.ItemType == ItemType.Weapon)
            attributes /= ClassConfigurationProvider.GetClassConfiguration(item.ItemClassType).WeaponAttributeMultiplier;

        if (item.ItemAttributeType == ItemAttributeType.NormalOneStat && attributes > 66)
            attributes = Math.Round(attributes) * 0.75D;

        attributes = Math.Floor(Math.Pow(Math.Round(attributes), 1.2));

        double splinterRatio, metalRatio;
        switch (item.ItemAttributeType)
        {
            case ItemAttributeType.NormalOneStat:
                metalRatio = 87.5;
                splinterRatio = 0.5;
                break;
            case ItemAttributeType.NormalTwoStats:
                metalRatio = 65;
                splinterRatio = 7.5;
                break;
            case ItemAttributeType.Epic:
            case ItemAttributeType.Legendary:
            case ItemAttributeType.EpicAllAttributes:
            default:
                metalRatio = 37.5;
                splinterRatio = 75;
                break;
        }
        var splinters = (int)Math.Floor(attributes * splinterRatio / 100);
        var metal = (int)Math.Floor(attributes * metalRatio / 100);

        if (item.ItemType == ItemType.Weapon)
        {
            var multiplier = ClassConfigurationProvider.GetClassConfiguration(item.ItemClassType).WeaponAttributeMultiplier;
            metal *= multiplier;
            splinters *= multiplier;
        }

        if (item.UpgradeLevel > 0)
        {
            var refund = GetUpgradeCostRefund(item);
            splinters += refund.Splinters;
            metal += refund.Metal;
        }

        return new BlackSmithResources(splinters, metal);
    }

    public BlackSmithResources UpgradeItems(List<EquipmentItem> items, BlackSmithResources resources)
    {
        var itemPriority = items.Where(i => i.UpgradeLevel < 20)
            .Select(i => new
            {
                Item = i,
                UpgradeCosts = GetCostsOfUpgrading(
                (int)(i.Attributes.Max() * Math.Pow(1.03, i.UpgradeLevel)),
                i.ItemType,
                i.ItemAttributeType,
                i.ItemClassType,
                i.UpgradeLevel,
                i.UpgradeLevel + 1)
            })
            .OrderBy(i => i.UpgradeCosts.Metal + i.UpgradeCosts.Splinters)
            .ToList();

        var nextItem = itemPriority.FirstOrDefault();

        if (nextItem == null)
            return default;

        if (nextItem.UpgradeCosts > resources)
            return default;

        nextItem.Item.UpgradeLevel++;

        return nextItem.UpgradeCosts + UpgradeItems(items, resources - nextItem.UpgradeCosts);
    }

    private BlackSmithResources GetUpgradeCostRefund(EquipmentItem item)
    {
        BlackSmithResources refund = default;

        var baseAttributes = item.Attributes.Max();
        var attributesWithUpgrades = (int)Math.Round(baseAttributes * Math.Pow(1.03D, item.UpgradeLevel));
        for (var upgradeLevel = item.UpgradeLevel; upgradeLevel > 0; upgradeLevel--)
        {
            attributesWithUpgrades = (int)Math.Round(attributesWithUpgrades / 1.04D);
            var cost = GetCostsOfUpgrading(attributesWithUpgrades, item.ItemType, item.ItemAttributeType, item.ItemClassType, upgradeLevel - 1, upgradeLevel);
            refund += cost;
        }

        return refund;
    }

    private BlackSmithResources GetCostsOfUpgrading(int attributes, ItemType itemType, ItemAttributeType itemAttributeType, ClassType classType, int upgradeFromLevel, int upgradeToLevel)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(upgradeToLevel, 1, nameof(upgradeToLevel));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(upgradeToLevel, 20, nameof(upgradeToLevel));
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(upgradeToLevel, upgradeFromLevel, nameof(upgradeToLevel));

        BlackSmithResources cost = default;

        for (var currentUpgradeLevel = upgradeFromLevel + 1; currentUpgradeLevel <= upgradeToLevel; currentUpgradeLevel++)
        {
            var baseAttributes = Math.Round(Math.Pow(1.03, currentUpgradeLevel - upgradeFromLevel - 1) * attributes);

            if (itemAttributeType == ItemAttributeType.EpicAllAttributes)
                baseAttributes *= 1.2D;

            if (itemType == ItemType.Weapon)
                baseAttributes /= ClassConfigurationProvider.GetClassConfiguration(classType).WeaponAttributeMultiplier;

            if (itemAttributeType == ItemAttributeType.NormalOneStat && baseAttributes > 66)
                baseAttributes *= 0.75D;

            baseAttributes = Math.Floor(Math.Pow(Math.Round(baseAttributes), 1.2D));

            double metalRatio, splinterRatio;

            switch (itemAttributeType)
            {
                case ItemAttributeType.NormalOneStat:
                    metalRatio = 50;
                    splinterRatio = 25;
                    break;
                case ItemAttributeType.NormalTwoStats:
                    metalRatio = 50;
                    splinterRatio = 50;
                    break;
                case ItemAttributeType.Epic:
                case ItemAttributeType.Legendary:
                case ItemAttributeType.EpicAllAttributes:
                default:
                    metalRatio = 50;
                    splinterRatio = 75;
                    break;
            }

            (metalRatio, splinterRatio) = upgradeToLevel switch
            {
                1 => (metalRatio * 3, 0),
                2 => (metalRatio * 4, 1),
                >= 3 and <= 8 => (metalRatio * (upgradeFromLevel + 3D), splinterRatio * (upgradeFromLevel - 1D)),
                9 => (metalRatio * 12D, splinterRatio * 8D),
                10 => (metalRatio * 15D, splinterRatio * 10D),
                >= 11 => (metalRatio * (6 + upgradeFromLevel), splinterRatio * (10 + (2D * (upgradeFromLevel - 9)))),
                _ => throw new ArgumentOutOfRangeException(nameof(upgradeToLevel))
            };
            var metalCost = (int)Math.Floor(Math.Floor(metalRatio) * baseAttributes / 100);
            var splinterCost = (int)Math.Floor(Math.Floor(splinterRatio) * baseAttributes / 100);

            if (itemType == ItemType.Weapon)
            {
                var multiplier = ClassConfigurationProvider.GetClassConfiguration(classType).WeaponAttributeMultiplier;
                metalCost *= multiplier;
                splinterCost *= multiplier;
            }

            cost = cost with { Metal = cost.Metal + metalCost, Splinters = cost.Splinters + splinterCost };
        }

        return cost;
    }
}

public readonly record struct BlackSmithResources(int Splinters, int Metal)
{
    public static bool operator >=(BlackSmithResources a, BlackSmithResources b) => a.Splinters >= b.Splinters && a.Metal >= b.Metal;
    public static bool operator <=(BlackSmithResources a, BlackSmithResources b) => a.Splinters <= b.Splinters && a.Metal <= b.Metal;
    public static bool operator >(BlackSmithResources a, BlackSmithResources b) => a.Splinters > b.Splinters || a.Metal > b.Metal;
    public static bool operator <(BlackSmithResources a, BlackSmithResources b) => a.Splinters < b.Splinters && a.Metal < b.Metal;
    public static BlackSmithResources operator +(BlackSmithResources a, BlackSmithResources b) => new(a.Splinters + b.Splinters, a.Metal + b.Metal);
    public static BlackSmithResources operator -(BlackSmithResources a, BlackSmithResources b) => new(a.Splinters - b.Splinters, a.Metal - b.Metal);
}