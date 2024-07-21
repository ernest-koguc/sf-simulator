using System.Collections;

namespace SFSimulator.Core;

public class ItemReequiperService(RuneQuantityProvider runeQuantityProvider, RuneValueProvider runeValueProvider,
        GemTypeUsageProvider gemTypeUsageProvider, IBlackSmithAdvisor blackSmithAdvisor, GuildKnightsProvider guildKnightsProvider)
{
    public ReequipOptions Options { get; set; } = new(10, 1, 10, 1, 1.5D);

    public bool ShouldReequip(int characterLevel)
        => characterLevel >= Options.CharacterLevelOnLastEquipmentChange + Options.CharacterReequipLevelOffset;

    public bool ShouldReequipCompanions(int characterLevel)
        => characterLevel >= Options.CompanionLevelOnLastEquipmentChange + Options.CompanionReequipLevelOffset;

    public void ReequipCharacter(SimulationOptions simulationOptions, int day)
    {
        Options = Options with { CharacterLevelOnLastEquipmentChange = simulationOptions.Level };
        ReequipItems(simulationOptions.Class, simulationOptions.Level, simulationOptions.Aura, simulationOptions.Items, simulationOptions, day, simulationOptions.BlackSmithResources, true);
    }

    public void ReequipCompanions(SimulationOptions simulationOptions, int day)
    {
        Options = Options with { CompanionLevelOnLastEquipmentChange = simulationOptions.Level };
        foreach (var companion in simulationOptions.Companions)
        {
            ReequipItems(companion.Class, simulationOptions.Level, simulationOptions.Aura, companion.Items, simulationOptions, day, simulationOptions.BlackSmithResources, false);
        }
    }

    private void ReequipItems(ClassType classType, int level, int aura, FightableItems items, SimulationOptions simulationOptions, int day, BlackSmithResources blackSmithResources, bool upgradeItems)
    {
        var itemComparer = new ClassAwareItemComparer(classType);
        var runesQuantity = runeQuantityProvider.GetRunesQuantity(day);
        var itemQualityRune = runeValueProvider.GetRuneValue(RuneType.ItemQuality, runesQuantity);
        var gemType = gemTypeUsageProvider.GetGemTypeToUse(day, classType, items.SimpleList.Select(i => i.GemType));
        var knights = guildKnightsProvider.GetKnightsAmount(day);
        var isSecondWeaponSlot = false;

        foreach (var (Item, ItemType) in items)
        {
            var itemBuilder = new EquipmentBuilder(ItemAttributeType.Epic, level, classType, aura,
                    simulationOptions.ScrollsUnlocked, itemQualityRune, ItemType).WithAttributes();

            if (itemBuilder.Compare(Item, itemComparer) < 1)
            {
                continue;
            }

            var runeType = ItemType != ItemType.Weapon ? GetRuneForItem(items.SimpleList) : RuneType.FireDamage;
            var runeValue = runeValueProvider.GetRuneValue(runeType, runesQuantity);

            _ = itemBuilder.WithGem(gemType, simulationOptions.GemMineLevel, knights).WithRune(runeType, runeValue);
            switch (ItemType)
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
                case ItemType.PetFood:
                default:
                    break;
            }

            if (ItemType == ItemType.Weapon)
            {
                items.ChangeWeapon(itemBuilder.Build(), isSecondWeaponSlot);
                isSecondWeaponSlot = true;
            }
            else
            {
                items.ChangeItem(itemBuilder.Build());
            }

            if (Item is not null)
                blackSmithResources += blackSmithAdvisor.DismantleItem(Item);
        }

        if (upgradeItems)
            blackSmithResources -= blackSmithAdvisor.UpgradeItems(items.SimpleList, blackSmithResources);
    }

    private static RuneType GetRuneForItem(List<EquipmentItem> items)
    {
        var hpRuneTotal = items.Where(i => i.RuneType == RuneType.HealthBonus).Sum(i => i.RuneValue);
        if (hpRuneTotal < 15) return RuneType.HealthBonus;
        var resistance = items.Where(i => i.RuneType == RuneType.TotalResistance).Sum(i => i.RuneValue);
        return resistance < 75 ? RuneType.TotalResistance : RuneType.None;
    }
}
public readonly record struct ReequipOptions(int CharacterReequipLevelOffset, int CharacterLevelOnLastEquipmentChange,
        int CompanionReequipLevelOffset, int CompanionLevelOnLastEquipmentChange, double PreferredWeaponRange);

public class FightableItems : IEnumerable<(EquipmentItem? Item, ItemType ItemType)>
{
    private EquipmentItem?[] Items { get; set; }
    private ClassType Class { get; set; }
    private ClassAwareItemComparer Comparer { get; set; }

    public List<EquipmentItem> SimpleList => Items.Where(i => i is not null).Select(i => i!).ToList();
    public EquipmentItem? FirstWeapon => Items[(int)ItemType.Weapon - 1];
    public EquipmentItem? SecondWeapon => Class == ClassType.Assassin ? Items[(int)ItemType.Shield - 1] : null;

    public FightableItems(ClassType classType, List<EquipmentItem>? items = null)
    {
        Class = classType;
        Items = new EquipmentItem[10];
        Comparer = new ClassAwareItemComparer(classType);
        if (items is not null)
        {
            InitItems(items);
        }
    }

    public void ChangeItem(EquipmentItem item)
    {
        if (item.ItemType == ItemType.Weapon)
            throw new InvalidOperationException($"Weapon changing is not supported for this method, consider using {nameof(ChangeWeapon)} method to change weapon!");

        var index = (int)item.ItemType;

        Items[index - 1] = item;
    }

    public void ChangeWeapon(EquipmentItem item, bool isSecondSlot)
    {
        var index = isSecondSlot ? (int)ItemType.Shield : (int)ItemType.Weapon;
        Items[index - 1] = item;
    }

    public IEnumerator<(EquipmentItem? Item, ItemType ItemType)> GetEnumerator()
    {
        for (var i = 0; i < Items.Length; i++)
        {
            var item = Items[i];
            if (i + 1 == (int)ItemType.Shield && Class == ClassType.Assassin) yield return (item, ItemType.Weapon);
            else if (i + 1 == (int)ItemType.Shield && Class == ClassType.Warrior) yield return (item, ItemType.Shield);
            else continue;

            yield return (item, (ItemType)(i + 1));
        }
    }

    private void InitItems(List<EquipmentItem> items)
    {
        items = items.Where(i => i.ItemType is not ItemType.None or ItemType.PetFood).ToList();
        var isSecondWeaponSlot = false;
        foreach (var item in items)
        {
            if (item.ItemType != ItemType.Weapon)
            {
                ChangeItem(item);
            }
            else
            {
                ChangeWeapon(item, isSecondWeaponSlot);
                isSecondWeaponSlot = true;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}