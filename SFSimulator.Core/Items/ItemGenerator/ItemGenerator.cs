using QuestSimulator.Enums;
using QuestSimulator.FileReaders;
using System.ComponentModel;

namespace QuestSimulator.Items
{
    public class ItemGenerator : IItemGenerator
    {
        private readonly Random _random;
        private readonly IValuesReader _valuesReader;
        private readonly Dictionary<int, float> _itemGoldValues;

        public ItemGenerator(Random random, IValuesReader valuesReader)
        {
            _random = random;
            _valuesReader = valuesReader;

            _itemGoldValues = _valuesReader.ReadItemGoldValues();
        }

        public Item GenerateItem(int characterLevel, ItemSourceType sourceType)
        {
            return sourceType switch
            {
                ItemSourceType.AfterQuest => GenerateItemAfterQuest(characterLevel),
                ItemSourceType.BeforeQuest => GenerateItemBeforeQuest(characterLevel),
                _ => throw new InvalidEnumArgumentException(nameof(sourceType)),
            };
        }

        public Item GeneratePetFoodItem()
            => new() { ItemType = ItemType.PetFood, ItemSourceType = ItemSourceType.AfterQuest, GoldValue = 0 };


        private Item GenerateItemAfterQuest(int characterLevel)
        {
            var item = GenerateItemBeforeQuest(characterLevel);

            item.GoldValue *= 0.80f;
            item.ItemSourceType = ItemSourceType.AfterQuest;

            return item;
        }

        private Item GenerateItemBeforeQuest(int characterLevel)
        {
            var item = new Item
            {
                ItemType = (ItemType)_random.Next(1, 10),
                GoldValue = GetGoldValueForItem(characterLevel),
                ItemSourceType = ItemSourceType.BeforeQuest
            };

            return item;
        }

        private float GetGoldValueForItem(int characterLevel)
        {
            if (!_itemGoldValues.TryGetValue(characterLevel, out float goldValue))
                goldValue = 2500000;

            // 20% for item to have half the gold value
            if (_random.Next(0, 5) == 0)
                return goldValue * 0.5f;

            return goldValue;
        }
    }
}
