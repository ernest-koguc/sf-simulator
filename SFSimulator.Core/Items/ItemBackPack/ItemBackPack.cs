using System.ComponentModel;

namespace SFSimulator.Core
{
    public class ItemBackPack : IItemBackPack
    {
        public List<Item> Items { get; set; } = new List<Item>();
        public int Size { get; private set; }
        public IComparer<Item> ItemComparer { get; private set; }

        public ItemBackPack(IComparer<Item> itemComparer, int size)
        {
            Size = size;
            ItemComparer = itemComparer;
        }

        public decimal? AddItemToBackPack(Item? item, IEnumerable<ItemType> currentItemsForWitch)
        {
            if (item == null || item.ItemType == ItemType.PetFood)
                return null;

            if (currentItemsForWitch.Contains(item.ItemType))
                return item.GoldValue * 2;

            Items.Add(item);

            Items.Sort(ItemComparer);

            if (Items.Count <= Size) return null;

            var itemToSell = Items.Take(1).FirstOrDefault();
            Items.RemoveAll(i => i == itemToSell);

            return itemToSell?.GoldValue;
        }

        public decimal? SellSpecifiedItemTypeToWitch(IEnumerable<ItemType> currentItemsForWitch)
        {
            if (currentItemsForWitch.Contains(ItemType.None) || currentItemsForWitch.Contains(ItemType.PetFood))
                throw new InvalidEnumArgumentException("Item type cannot be 'None' or 'PetFood'");

            var gold = 0M;

            Items.Where(item => currentItemsForWitch.Contains(item.ItemType)).ToList().ForEach(item =>
            {
                gold += item.GoldValue;
                Items.Remove(item);
            });

            Items.Sort(ItemComparer);
            return gold > 0 ? gold * 2 : null;
        }

        public decimal? SellAllItemsToWItch()
        {
            var sumGold = Items.Sum(item => item.GoldValue);
            Items.Clear();
            return sumGold > 0 ? sumGold * 2 : null;
        }
    }
}