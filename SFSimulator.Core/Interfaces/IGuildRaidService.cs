namespace SFSimulator.Core;

public interface IGuildRaidService
{
    void Progress(int currentDay, SimulationContext simulationContext);
    void SetUpGuildRaidsState(SimulationContext simulationContext, List<GuildRaidRequirements>? guildRaidRequirements = null);
}