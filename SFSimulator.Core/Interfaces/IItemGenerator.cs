namespace SFSimulator.Core;

public interface IItemGenerator
{
    Item GenerateItem(int characterLevel);
    Item GeneratePetFoodItem();
}