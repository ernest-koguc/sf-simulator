namespace SFSimulator.Core;

public interface ICharacterDungeonProgressionService
{
    DungeonProgressionOptions DungeonOptions { get; set; }
    ReequipOptions ReequipOptions { set; }

    IEnumerable<Dungeon> GetDungeons(SimulationContext simulationContext);
    void InitCharacterDungeonState(SimulationContext simulationContext);
    void ProgressThrough(SimulationContext simulationContext, CharacterDungeonProgressionService.OnDungeonKill onDungeonKill, int day, List<DungeonEnemy>? dungeonEnemies = null);
    void ResetProgress();
}