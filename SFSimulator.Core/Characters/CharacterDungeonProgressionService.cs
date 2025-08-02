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

    public void InitCharacterDungeonState(SimulationContext simulationContext)
    {
        var dungeonData = simulationContext.DungeonsData;
        if (dungeonData is null) return;

        var lightWorld = dungeonData.Normal.Select((currentPosition, dungeonIndex) => (currentPosition, MapDungeonIndex(dungeonIndex, false)));
        var shadowWorld = dungeonData.Shadow.Select((currentPosition, dungeonIndex) => (currentPosition, MapDungeonIndex(dungeonIndex, true)));
        var loopOfIdols = (dungeonData.Youtube, 130);
        var tower = (dungeonData.Tower, 98);
        var twister = (dungeonData.Twister, 99);
        var combinedDungeons = lightWorld.Union(shadowWorld).Append(tower).Append(loopOfIdols).Append(twister);

        var dungeons = dungeonProvider.GetAllDungeons(simulationContext);

        foreach (var (beatenEnemyPosition, dungeonPosition) in combinedDungeons)
        {
            var dungeon = dungeons.FirstOrDefault(d => d.Position == dungeonPosition);
            if (dungeon is null) continue;
            var beatenEnemy = dungeon.DungeonEnemies.FirstOrDefault(e => e.Position == beatenEnemyPosition);
            if (beatenEnemy is null)
            {
                dungeon.IsUnlocked = false;
                continue;
            }

            foreach (var enemy in dungeon.DungeonEnemies.Where(e => e.Position <= beatenEnemy.Position))
            {
                enemy.IsDefeated = true;
            }

            if (dungeon.DungeonEnemies.All(e => e.IsDefeated)) dungeon.IsDefeated = true;
        }
    }

    private static int MapDungeonIndex(int sfToolsIndex, bool isShadow)
    {
        var index = (sfToolsIndex + 1) switch
        {
            >= 1 and <= 12 => sfToolsIndex + 1,
            13 => 15,
            14 => 19,
            15 => 17,
            16 => 18,
            17 => 24,
            18 => 27,
            19 => 21,
            20 => 13,
            21 => 14,
            22 => 16,
            23 => 20,
            24 => 28,
            25 => 22,
            26 => 23,
            27 => 25,
            28 => 26,
            _ => 0
        };

        return isShadow ? index + 100 : index;
    }
}