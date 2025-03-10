namespace SFSimulator.Core;

public interface IItemReequiperService
{
    ReequipOptions Options { get; set; }

    void ReequipCharacter(SimulationContext simulationContext, int day);
    void ReequipCompanions(SimulationContext simulationContext, int day);
    bool ShouldReequip(int characterLevel);
    bool ShouldReequipCompanions(int characterLevel);
}