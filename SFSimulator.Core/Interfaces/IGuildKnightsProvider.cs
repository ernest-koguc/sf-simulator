namespace SFSimulator.Core;

public interface IGuildKnightsProvider
{
    void Setup(int currentKnights, ICollection<DayKnightsQuantity>? knightsQuantity = null);
    int GetKnightsAmount(int day);
}