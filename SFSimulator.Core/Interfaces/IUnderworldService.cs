namespace SFSimulator.Core;
public interface IUnderworldService
{
    void Setup(SimulationContext simulationContext);
    void Progress(SimulationContext simulationContext, List<EventType> events);
}
