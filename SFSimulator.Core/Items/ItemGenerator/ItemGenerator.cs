using System.ComponentModel;

namespace SFSimulator.Core
{
    public class ItemGenerator : IItemGenerator
    {
        private readonly Random _random;
        private readonly IItemValueProvider _itemValueProvider;

        public ItemGenerator(Random random, IItemValueProvider itemValueProvider)
        {
            _random = random;
            _itemValueProvider = itemValueProvider;
        }

        public Item GenerateItem(int characterLevel, ItemSourceType sourceType)
        {
            return sourceType switch
            {
                ItemSourceType.AfterQuest => GenerateItemAfterQuest(characterLevel),
                ItemSourceType.BeforeQuest => GenerateItemBeforeQuest(characterLevel),
                ItemSourceType.Expedition => GenerateItemFromExpedition(characterLevel),
                _ => throw new InvalidEnumArgumentException(nameof(sourceType)),
            };
        }

        private Item GenerateItemFromExpedition(int characterLevel)
        {
            var item = GenerateItemBeforeQuest(characterLevel);
            item.GoldValue *= 4;
            item.ItemSourceType = ItemSourceType.Expedition;

            return item;
        }

        public Item GeneratePetFoodItem()
            => new() { ItemType = ItemType.PetFood, ItemSourceType = ItemSourceType.AfterQuest, GoldValue = 0 };


        private Item GenerateItemAfterQuest(int characterLevel)
        {
            var item = GenerateItemBeforeQuest(characterLevel);

            item.GoldValue *= 0.80M;
            item.ItemSourceType = ItemSourceType.AfterQuest;

            return item;
        }

        private Item GenerateItemBeforeQuest(int characterLevel)
        {
            var item = new Item
            {
                ItemType = (ItemType)_random.Next(1, 10),
                GoldValue = _itemValueProvider.GetGoldValueForItem(characterLevel),
                ItemSourceType = ItemSourceType.BeforeQuest
            };

            return item;
        }
    }
}