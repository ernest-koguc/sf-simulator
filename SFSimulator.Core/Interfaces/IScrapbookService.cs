namespace SFSimulator.Core;

public interface IScrapbookService
{
    void InitScrapbook(IEnumerable<Dungeon> dungeons, SimulationContext simulationContext);
    void UpdateScrapbook(SimulationContext simulationContext);
    void UpdateScrapbook(SimulationContext simulationContext, int guildRaidPictures);
    void UpdateScrapbook(SimulationContext simulationContext, DungeonSimulationResult dungeonResult);
}