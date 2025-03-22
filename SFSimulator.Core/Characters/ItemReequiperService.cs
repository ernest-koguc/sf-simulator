namespace SFSimulator.Core;

public class ItemReequiperService(IRuneValueProvider runeValueProvider,
        IGemTypeUsageProvider gemTypeUsageProvider, IBlackSmithAdvisor blackSmithAdvisor, IGuildKnightsProvider guildKnightsProvider) : IItemReequiperService
{
    public ReequipOptions Options { get; set; } = new()
    {
        ChangeGear = true,
        CharacterReequipLevelOffset = 10,
        CharacterLevelOnLastEquipmentChange = -1,
        CompanionReequipLevelOffset = 10,
        CompanionLevelOnLastEquipmentChange = -1,
        PreferredWeaponRange = 1.5D,
    };

    public bool ShouldReequip(int characterLevel)
    {
        if (Options.CharacterLevelOnLastEquipmentChange == -1)
        {
            Options.CharacterLevelOnLastEquipmentChange = characterLevel;
            return false;
        }
        return characterLevel >= Options.CharacterLevelOnLastEquipmentChange + Options.CharacterReequipLevelOffset && Options.ChangeGear;
    }

    public bool ShouldReequipCompanions(int characterLevel)
    {
        if (Options.CompanionLevelOnLastEquipmentChange == -1)
        {
            Options.CompanionLevelOnLastEquipmentChange = characterLevel;
            return false;
        }

        return characterLevel >= Options.CompanionLevelOnLastEquipmentChange + Options.CompanionReequipLevelOffset && Options.ChangeGear;
    }

    public void ReequipCharacter(SimulationContext simulationContext, int day)
    {
        Options.CharacterLevelOnLastEquipmentChange = simulationContext.Level;
        simulationContext.Items = ReequipItems(simulationContext.Class, simulationContext.Items, simulationContext, day, true);
    }

    public void ReequipCompanions(SimulationContext simulationContext, int day)
    {
        Options.CompanionLevelOnLastEquipmentChange = simulationContext.Level;
        foreach (var companion in simulationContext.Companions)
        {
            companion.Items = ReequipItems(companion.Class, companion.Items, simulationContext, day, false);
        }
    }

    private List<EquipmentItem> ReequipItems(ClassType classType, List<EquipmentItem> oldItems, SimulationContext simulationContext, int day, bool upgradeItems)
    {
        var itemComparer = new ClassAwareItemComparer(classType);
        var runesQuantity = simulationContext.RuneQuantity;
        var itemQualityRune = runeValueProvider.GetRuneValue(RuneType.ItemQuality, runesQuantity);
        var knights = guildKnightsProvider.GetKnightsAmount(day);
        var isSecondWeaponSlot = false;

        var newItems = new List<EquipmentItem>();

        foreach (var itemType in GetPossibleItems(classType))
        {
            var itemBuilder = new EquipmentBuilder(ItemAttributeType.Epic, simulationContext.Level, classType, simulationContext.Aura,
                    simulationContext.ScrollsUnlocked, itemQualityRune, itemType).WithAttributes();

            EquipmentItem? currentItem;
            if (itemType != ItemType.Weapon)
            {
                currentItem = oldItems.FirstOrDefault(i => i.ItemType == itemType);
            }
            else if (!isSecondWeaponSlot)
            {
                currentItem = oldItems.FirstOrDefault(i => i.ItemType == ItemType.Weapon);
            }
            else
            {
                currentItem = oldItems.LastOrDefault(i => i.ItemType == ItemType.Weapon);
            }

            if (currentItem is not null && itemBuilder.Compare(currentItem, itemComparer) < 1)
            {
                newItems.Add(currentItem);
                continue;
            }

            var runeType = itemType switch
            {
                ItemType.Weapon => RuneType.FireDamage,
                ItemType.Shield => RuneType.None,
                _ => GetRuneForItem(oldItems)
            };
            var runeValue = runeValueProvider.GetRuneValue(runeType, runesQuantity);

            var gemType = gemTypeUsageProvider.GetGemTypeToUse(day, classType, newItems.Select(i => i.GemType));
            _ = itemBuilder.WithGem(gemType, simulationContext.GemMineLevel, knights).WithRune(runeType, runeValue);
            switch (itemType)
            {
                case ItemType.Weapon:
                    _ = itemBuilder.AsWeapon(Options.PreferredWeaponRange);
                    break;
                case ItemType.Headgear:
                case ItemType.Breastplate:
                case ItemType.Gloves:
                case ItemType.Boots:
                case ItemType.Belt:
                    _ = itemBuilder.WithArmor();
                    break;
                case ItemType.None:
                case ItemType.Trinket:
                case ItemType.Ring:
                case ItemType.Amulet:
                case ItemType.Shield:
                default:
                    break;
            }

            if (simulationContext.Level > 66)
            {
                itemBuilder.WithEnchantment();
            }

            newItems.Add(itemBuilder.Build());

            if (currentItem is not null)
                //adjust this in the future, looks like we cant have our own + and - operators
                simulationContext.BlackSmithResources += blackSmithAdvisor.DismantleItem(currentItem);

        }

        if (upgradeItems)
            //adjust this in the future, looks like we cant have our own + and - operators
            simulationContext.BlackSmithResources -= blackSmithAdvisor.UpgradeItems(newItems, simulationContext.BlackSmithResources);

        return newItems;
    }

    private static RuneType GetRuneForItem(List<EquipmentItem> items)
    {
        var hpRuneTotal = items.Where(i => i.RuneType == RuneType.HealthBonus).Sum(i => i.RuneValue);
        if (hpRuneTotal < 15) return RuneType.HealthBonus;
        var resistance = items.Where(i => i.RuneType == RuneType.TotalResistance).Sum(i => i.RuneValue);
        return resistance < 75 ? RuneType.TotalResistance : RuneType.None;
    }

    private static List<ItemType> GetPossibleItems(ClassType classType)
    {
        var possibleItems = new List<ItemType>
        {
            ItemType.Headgear,
            ItemType.Breastplate,
            ItemType.Gloves,
            ItemType.Boots,
            ItemType.Weapon,
            ItemType.Amulet,
            ItemType.Belt,
            ItemType.Ring,
            ItemType.Trinket
        };


        if (classType is ClassType.Assassin)
        {
            possibleItems.Add(ItemType.Weapon);
        }
        if (classType is ClassType.Warrior)
        {
            possibleItems.Add(ItemType.Shield);
        }

        return possibleItems;
    }
}
public class ReequipOptions
{
    public bool ChangeGear { get; set; }
    public int CharacterReequipLevelOffset { get; set; }
    public int CharacterLevelOnLastEquipmentChange { get; set; }
    public int CompanionReequipLevelOffset { get; set; }
    public int CompanionLevelOnLastEquipmentChange { get; set; }
    public double PreferredWeaponRange { get; set; }
}
