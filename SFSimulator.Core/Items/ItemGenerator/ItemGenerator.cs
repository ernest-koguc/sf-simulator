namespace SFSimulator.Core;

public class ItemGenerator : IItemGenerator
{
    private readonly Random _random;
    private readonly IItemValueProvider _itemValueProvider;

    public ItemGenerator(Random random, IItemValueProvider itemValueProvider)
    {
        _random = random;
        _itemValueProvider = itemValueProvider;
    }

    public Item GenerateItem(int characterLevel)
    {
        var item = new Item
        {
            ItemType = (ItemType)_random.Next(1, 11),
            GoldValue = _itemValueProvider.GetGoldValueForItem(characterLevel) * 4,
        };

        return item;
    }
}