namespace SFSimulator.Core;

public class CharacterDungeonProgressionService(IDungeonProvider dungeonProvider,
        IDungeonSimulator dungeonSimulator, IItemReequiperService itemReequiper) : ICharacterDungeonProgressionService
{
    public delegate void OnDungeonKill(DungeonSimulationResult result);
    public DungeonProgressionOptions DungeonOptions { get; set; } = new()
    {
        InstaKillPercentage = 1,
        DungeonIterations = 1000,
    };
    public ReequipOptions ReequipOptions { set => itemReequiper.Options = value; }

    public IEnumerable<Dungeon> GetDungeons(SimulationContext simulationContext)
    {
        return dungeonProvider.GetAllDungeons(simulationContext);
    }

    public void ResetProgress()
    {
        dungeonProvider.InitDungeons();
    }

    public void ProgressThrough(SimulationContext simulationContext, OnDungeonKill onDungeonKill, int day, List<DungeonEnemy>? dungeonEnemies = null)
    {
        if (!ShouldDoDungeons(simulationContext.Level)) return;

        DrHouse.Differential("Doing dungeons");

        Reequip(simulationContext, day);

        UnlockDungeons(simulationContext);

        var enemiesToIterate = dungeonEnemies ?? dungeonProvider.GetFightablesDungeonEnemies(simulationContext);
        var dungeonsKilled = 0;

        foreach (var enemy in enemiesToIterate)
        {
            // If the enemy was defeated in a side loop then skip
            if (enemy.IsDefeated) continue;

            // Make sure it is at least 1 if we have few iteration and small insta kill % (e.g. 100 iterations and 0.1% would result in 0)
            var winThreshold = Math.Max(1, (int)(DungeonOptions.DungeonIterations * (DungeonOptions.InstaKillPercentage / 100)));

            var result = dungeonSimulator.SimulateDungeon(enemy, simulationContext, simulationContext.Companions,
                new(DungeonOptions.DungeonIterations, winThreshold, true));

            if (!result.Succeeded) continue;

            dungeonsKilled++;
            DrHouse.Differential($"Defeated enemy {enemy.Name} in dungeon {enemy.Dungeon.Name} {enemy.Dungeon.Type}");
            OnDungeonEnemyKilled(result, enemy, onDungeonKill);


            var nextEnemy = enemy.Dungeon.DungeonEnemies.OrderBy(e => e.Position).FirstOrDefault(e => !e.IsDefeated);
            if (nextEnemy is not null)
                ProgressThrough(simulationContext, onDungeonKill, day, [nextEnemy]);
        }

        if (dungeonsKilled == 0)
            return;

        ProgressThrough(simulationContext, onDungeonKill, day);
    }

    private void Reequip(SimulationContext simulationContext, int day)
    {
        if (itemReequiper.ShouldReequip(simulationContext.Level)) itemReequiper.ReequipCharacter(simulationContext, day);
        if (itemReequiper.ShouldReequipCompanions(simulationContext.Level)) itemReequiper.ReequipCompanions(simulationContext, day);
    }

    private void UnlockDungeons(SimulationContext simulationContext)
    {
        var dungeons = dungeonProvider.GetAllDungeons(simulationContext);
        var lockedDungeons = dungeons.Where(d => !d.IsUnlocked);
        var dungeonUnlockContext = new DungeonUnlockContext(simulationContext.Level, dungeons);

        foreach (var dungeon in lockedDungeons)
        {
            dungeon.IsUnlocked = dungeon.UnlockResolve(dungeonUnlockContext);
        }
    }

    private static void OnDungeonEnemyKilled(DungeonSimulationResult result, DungeonEnemy dungeonEnemy, OnDungeonKill onDungeonKill)
    {
        dungeonEnemy.IsDefeated = true;

        if (dungeonEnemy.Dungeon.DungeonEnemies.All(e => e.IsDefeated))
            dungeonEnemy.Dungeon.IsDefeated = true;

        onDungeonKill(result);
    }

    private bool ShouldDoDungeons(int characterLevel) => !DungeonOptions.DoDungeonPause
        || characterLevel < DungeonOptions.DungeonPauseStartLevel
        || characterLevel >= DungeonOptions.DungeonPauseEndLevel;

    public void InitCharacterDungeonState(SimulationContext simulationContext) => dungeonProvider.InitCharacterDungeonState(simulationContext);
}