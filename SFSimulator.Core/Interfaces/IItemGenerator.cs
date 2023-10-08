namespace SFSimulator.Core
{
    public interface IItemGenerator
    {
        Item GenerateItem(int characterLevel, ItemSourceType sourceType);
        Item GeneratePetFoodItem();
    }
}