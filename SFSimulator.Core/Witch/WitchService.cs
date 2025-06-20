namespace SFSimulator.Core;
public class WitchService : IWitchService
{
    private static readonly ItemType[] WitchItems =
    [
        ItemType.Weapon,
        ItemType.Headgear,
        ItemType.Breastplate,
        ItemType.Gloves,
        ItemType.Boots,
        ItemType.Amulet,
        ItemType.Belt,
        ItemType.Ring,
        ItemType.Trinket,

    ];

    public List<ItemType> GetAvailableItems(int day, bool isWitchEvent)
    {
        if (isWitchEvent)
        {
            return Enumerable.Range(1, 10).Select(i => (ItemType)i).ToList();
        }

        var currentItem = WitchItems[day % 9];

        if (currentItem == ItemType.Weapon)
            return [currentItem, ItemType.Shield];

        return [currentItem];
    }
}
