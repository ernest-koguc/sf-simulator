namespace QuestSimulator.Items
{
    public class ItemGoldValueComparer : IComparer<Item>
    {
        public int Compare(Item? x, Item? y)
        {
            if (x?.GoldValue > y?.GoldValue)
                return 1;

            if (x?.GoldValue == y?.GoldValue)
                return 0;

            return -1;
        }
    }
}
