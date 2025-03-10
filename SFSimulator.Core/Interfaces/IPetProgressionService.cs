namespace SFSimulator.Core;

public interface IPetProgressionService
{
    void GivePetFood(PetsState pets, double amount, PetElementType? elementType = null);
    void ProgressThrough(int currentDay, SimulationContext simulationContext, List<EventType> events, Action<PetSimulationResult> onWonFight);
    decimal SellPetFood(PetsState pets, int characterLevel);
}