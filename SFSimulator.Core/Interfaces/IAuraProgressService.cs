namespace SFSimulator.Core;

public interface IAuraProgressService
{
    void IncreaseAuraProgress(SimulationContext simulationContext, bool isToiletEvent);
}
