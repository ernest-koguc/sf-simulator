using QuestSimulator.Enums;
namespace QuestSimulator.Items
{
    public interface IItemGenerator
    {
        Item GenerateItem(int characterLevel, ItemSourceType sourceType);
        Item GeneratePetFoodItem();
    }
}