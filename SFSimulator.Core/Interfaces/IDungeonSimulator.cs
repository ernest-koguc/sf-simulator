﻿namespace SFSimulator.Core
{
    public interface IDungeonSimulator
    {
        DungeonSimulationResult SimulateDungeon(DungeonEnemy dungeonEnemy, Character character, int iterations, int winThreshold);
        DungeonSimulationResult SimulateDungeon(DungeonEnemy dungeonEnemy, RawFightable character, int iterations, int winThreshold);
        Task<DungeonSimulationResult> SimulateAllOpenDungeonsAsync(Character character, int iterations, int winThreshold);
    }
}
