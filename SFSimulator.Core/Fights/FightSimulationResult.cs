namespace SFSimulator.Core;
public record FightSimulationResult(int WonFights, bool Succeeded);
public record PetSimulationResult(int WonFights, bool Succeeded, long Experience)
{
    public static PetSimulationResult FailedResult(int wonFights) => new(wonFights, false, 0);
}
public record DungeonSimulationResult(bool Succeeded, long Experience, decimal Gold, Item? Item, int WonFights, DungeonEnemy DungeonEnemy)
{
    public static DungeonSimulationResult FailedResult(int wonFights, DungeonEnemy dungeonEnemy) => new(false, 0, 0, null, wonFights, dungeonEnemy);
}
