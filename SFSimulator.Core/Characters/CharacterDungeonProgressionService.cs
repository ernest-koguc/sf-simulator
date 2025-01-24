using System.Diagnostics;

namespace SFSimulator.Core;

public class CharacterDungeonProgressionService(IDungeonProvider dungeonProvider,
        IDungeonSimulator dungeonSimulator, ItemReequiperService itemReequiper)
{
    public delegate void OnDungeonKill(DungeonSimulationResult result);
    public DungeonProgressionOptions Options { get; set; } = new DungeonProgressionOptions(0.01D, DefaultDungeonIterations);
    private static int DefaultDungeonIterations => Debugger.IsAttached ? 1000 : 1000;
    public ReequipOptions ReequipOptions { set => itemReequiper.Options = value; }

    public IEnumerable<Dungeon> GetDungeons(SimulationContext simulationContext)
    {
        return dungeonProvider.GetAllDungeons(simulationContext);
    }

    public void ResetProgress()
    {
        dungeonProvider.InitDungeons();
    }

    public void ProgressThrough(SimulationContext simulationOptions, OnDungeonKill onDungeonKill, int day, List<DungeonEnemy>? dungeonEnemies = null)
    {
        Reequip(simulationOptions, day);

        UnlockDungeons(simulationOptions);

        var enemiesToIterate = dungeonEnemies ?? dungeonProvider.GetFightablesDungeonEnemies(simulationOptions);
        var dungeonsKilled = 0;

        foreach (var enemy in enemiesToIterate)
        {
            // If the enemy was actually defeated in a side loop then skip
            if (enemy.IsDefeated) continue;
            var winTreshold = (int)(Options.DungeonIterations * Options.InstaKillPercentage);
            var result = dungeonSimulator.SimulateDungeon(enemy, simulationOptions, simulationOptions.Companions, Options.DungeonIterations, winTreshold);

            if (!result.Succeeded) continue;

            dungeonsKilled++;
            OnDungeonEnemyKilled(result, enemy, onDungeonKill);


            var nextEnemy = enemy.Dungeon.DungeonEnemies.OrderBy(e => e.Position).FirstOrDefault(e => !e.IsDefeated);
            if (nextEnemy is not null)
                ProgressThrough(simulationOptions, onDungeonKill, day, [nextEnemy]);
        }

        if (dungeonsKilled == 0)
            return;

        ProgressThrough(simulationOptions, onDungeonKill, day);
    }

    private void Reequip(SimulationContext simulationOptions, int day)
    {
        if (itemReequiper.ShouldReequip(simulationOptions.Level)) itemReequiper.ReequipCharacter(simulationOptions, day);
        if (itemReequiper.ShouldReequipCompanions(simulationOptions.Level)) itemReequiper.ReequipCompanions(simulationOptions, day);
    }

    private void UnlockDungeons(SimulationContext simulationOptions)
    {
        var dungeons = dungeonProvider.GetAllDungeons(simulationOptions);
        var lockedDungeons = dungeons.Where(d => !d.IsUnlocked);
        var dungeonUnlockContext = new DungeonUnlockContext(simulationOptions.Level, dungeons);

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

    public void InitCharacterDungeonState(SimulationContext simulationOptions)
    {
        var dungeonData = simulationOptions.DungeonsData;
        if (dungeonData is null) return;

        var lightWorld = dungeonData.Normal.Select((currentPosition, dungeonIndex) => (currentPosition, MapDungeonIndex(dungeonIndex, false)));
        var shadowWorld = dungeonData.Shadow.Select((currentPosition, dungeonIndex) => (currentPosition, MapDungeonIndex(dungeonIndex, true)));
        var loopOfIdols = (dungeonData.Youtube, 130);
        var tower = (dungeonData.Tower, 98);
        var twister = (dungeonData.Twister, 99);
        var combinedDungeons = lightWorld.Union(shadowWorld).Append(tower).Append(loopOfIdols).Append(twister);

        var dungeons = dungeonProvider.GetAllDungeons(simulationOptions);

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
            14 => 17,
            15 => 19,
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