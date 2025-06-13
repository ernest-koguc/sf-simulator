namespace SFSimulator.Core;

public interface IPortalService
{
    void SetUpPortalState(SimulationContext simulationContext, List<PortalRequirements>? soloPortalRequirements = null, List<PortalRequirements>? guildPortalRequirements = null);
    void Progress(int days, SimulationContext simulationContext);
}