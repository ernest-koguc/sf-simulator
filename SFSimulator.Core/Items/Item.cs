using QuestSimulator.Enums;

namespace QuestSimulator.Items
{
    public class Item
    {
        public ItemType ItemType { get; set; }

        public ItemSourceType ItemSourceType { get; set; }
        public float GoldValue { get; set; }

    }
}