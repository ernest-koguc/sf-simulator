namespace SFSimulator.Core
{
    public interface IItemValueProvider
    {
        decimal GetGoldValueForItem(int characterLevel);
    }
}