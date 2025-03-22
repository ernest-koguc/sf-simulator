namespace SFSimulator.Core;

public interface IRuneQuantityProvider
{
    void Setup(SimulationContext simulationContext, ICollection<DayRuneQuantity>? quantity = null);
    void IncreaseRuneQuantity(SimulationContext simulationContext, int day);
}