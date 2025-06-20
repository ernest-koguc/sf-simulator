namespace SFSimulator.Core;
public interface IWitchService
{
    List<ItemType> GetAvailableItems(int day, bool isWitchEvent);
}
