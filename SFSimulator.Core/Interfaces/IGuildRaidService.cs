namespace SFSimulator.Core;

public interface IGuildRaidService
{
    void Progress(int currentDay, SimulationContext simulationContext, Action<int>? addGuildRaidPictures = null);
    void SetUpGuildRaidsState(SimulationContext simulationContext, List<GuildRaidRequirements>? guildRaidRequirements = null);
}