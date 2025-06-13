namespace SFSimulator.Core;

public interface IDungeonProvider
{
    List<DungeonEnemy> GetFightablesDungeonEnemies(SimulationContext simulationContext);
    List<Dungeon> GetAllDungeons(SimulationContext simulationContext);
    bool IsValidEnemy(int dungeonPosition, int dungeonEnemyPosition);
    DungeonEnemy GetDungeonEnemy(int dungeonPositon, int dungeonEnemyPosition);
    List<Dungeon> InitDungeons();
}