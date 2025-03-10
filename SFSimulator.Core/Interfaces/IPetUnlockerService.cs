namespace SFSimulator.Core;

public interface IPetUnlockerService
{
    void UnlockPets(int currentDay, SimulationContext simulationContext, List<EventType> events);
}