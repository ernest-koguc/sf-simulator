namespace SFSimulator.Core
{
    public interface IDungeonProvider
    {
        List<DungeonEnemy> GetFightablesDungeonEnemies();
        List<Dungeon> GetAllDungeons();
        bool IsValidEnemy(int dungeonPosition, int dungeonEnemyPosition);
        DungeonEnemy GetDungeonEnemy(int dungeonPositon, int dungeonEnemyPositon);
    }
}
