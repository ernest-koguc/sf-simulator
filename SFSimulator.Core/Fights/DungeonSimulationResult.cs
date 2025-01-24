namespace SFSimulator.Core;

public class DungeonSimulationResult
{
    public required bool Succeeded { get; set; }
    public required long Experience { get; set; }
    public required decimal Gold { get; set; }
    public required int WonFights { get; set; }
    public required DungeonEnemy DungeonEnemy { get; set; }

    public static DungeonSimulationResult FailedResult(int wonFights, DungeonEnemy dungeonEnemy) => new()
    {
        Succeeded = false,
        Experience = 0,
        Gold = 0,
        WonFights = wonFights,
        DungeonEnemy = dungeonEnemy
    };
}