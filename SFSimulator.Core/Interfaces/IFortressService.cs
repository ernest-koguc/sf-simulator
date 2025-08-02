namespace SFSimulator.Core;

public interface IFortressService
{
    void Progress(SimulationContext simulationContext, List<EventType> events);
}