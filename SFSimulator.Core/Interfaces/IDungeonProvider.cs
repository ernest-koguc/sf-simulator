namespace SFSimulator.Core;

public interface IDungeonProvider
{
    List<DungeonEnemy> GetFightablesDungeonEnemies(SimulationOptions simulationOptions);
    List<Dungeon> GetAllDungeons(SimulationOptions character);
    bool IsValidEnemy(int dungeonPosition, int dungeonEnemyPosition);
    DungeonEnemy GetDungeonEnemy(int dungeonPositon, int dungeonEnemyPosition);
}