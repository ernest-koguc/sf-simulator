namespace SFSimulator.Core
{
    public interface IItemBackPack
    {
        IComparer<Item> ItemComparer { get; }
        List<Item> Items { get; set; }
        int Size { get; }
        decimal? AddItemToBackPack(Item? item, IEnumerable<ItemType> currentItemsForWitch);
        decimal? SellAllItemsToWItch();
        decimal? SellSpecifiedItemTypeToWitch(IEnumerable<ItemType> currentItemsForWitch);
    }
}