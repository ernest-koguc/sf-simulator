namespace SFSimulator.Core
{
    public interface IItemBackPack
    {
        IComparer<Item> ItemComparer { get; }
        List<Item> Items { get; set; }
        int Size { get; }
        float? AddItemToBackPack(Item? item, IEnumerable<ItemType> currentItemsForWitch);
        float? SellAllItemsToWItch();
        float? SellSpecifiedItemTypeToWitch(IEnumerable<ItemType> currentItemsForWitch);
    }
}