namespace SFSimulator.Core;

public interface IDungeonProvider
{
    List<DungeonEnemy> GetFightablesDungeonEnemies(SimulationContext simulationOptions);
    List<Dungeon> GetAllDungeons(SimulationContext character);
    bool IsValidEnemy(int dungeonPosition, int dungeonEnemyPosition);
    DungeonEnemy GetDungeonEnemy(int dungeonPositon, int dungeonEnemyPosition);
    List<Dungeon> InitDungeons();
}