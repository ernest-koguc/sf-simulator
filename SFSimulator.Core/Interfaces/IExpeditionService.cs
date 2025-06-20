namespace SFSimulator.Core;

public interface IExpeditionService
{
    ExpeditionOptions Options { get; set; }
    List<Item> GetDailyExpeditionItems(int characterLevel, int thirst);
    double GetDailyExpeditionPetFood(int characterLevel, GoldBonus goldBonus, List<EventType> events, MountType mount, int thirst);
    void DoExpeditions(SimulationContext simulationContext, List<EventType> events, decimal thirst, Action<decimal> giveGold, Action<long> giveExperience);
}