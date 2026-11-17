using Microsoft.AspNetCore.Http.HttpResults;
using SFSimulator.Core;

namespace SFSimulator.Api;

public class SimulateDungeonHandler(IDungeonSimulator dungeonSimulator, IDungeonProvider dungeonProvider, IGameFormulasService gameFormulasService)
{
    // In-game array slot for each SF Tools dungeon progress index.
    // Source: sf-tools/src/data/dungeon-helper.ts, PLAYA_TO_INTERNAL_MAPPING.
    private static readonly int[] SfToolsToGameDungeonIndices =
    [
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15, 16, 18,
        19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 32, 33, 34, 35, 36
    ];

    public Results<Ok<List<SimulateDungeonResponse>>, BadRequest<string>> HandleSimulateDungeonAsync(SimulateDungeonRequest request)
    {
        if (request.Iterations > 1_000_000)
        {
            return TypedResults.BadRequest("Iterations cannot be greater than 1 000 000.");
        }

        var dungeons = MapDungeonProgress(request.DungeonState.Light, shadow: false)
            .Concat(MapDungeonProgress(request.DungeonState.Shadow, shadow: true))
            .Concat(request.DungeonState.Class.Select((progress, index) =>
                (DungeonPosition: index + 201, BeatenEnemyPosition: progress)));
        var enemies = dungeons
            .Where(dungeon => dungeon.BeatenEnemyPosition >= 0)
            .Select(dungeon => dungeonProvider.GetDungeonEnemySafe(dungeon.DungeonPosition, dungeon.BeatenEnemyPosition + 1))
            .Where(enemy => enemy != null)
            .Select(enemy => enemy!)
            .ToList();

        var player = request.Player;
        var companions = request.Companions.Select(c => new Player(
            c.Class == ClassType.Warrior ? ClassType.Bert : c.Class,
            player.Level,
            c.Strength,
            c.Dexterity,
            c.Intelligence,
            c.Constitution,
            c.Luck,
            c.Health,
            c.Armor,
            c.FirstWeapon,
            null,
            c.Reaction,
            c.CritMultiplier,
            c.LightningResistance,
            c.FireResistance,
            c.ColdResistance,
            player.GuildPortal
        )).ToArray();

        var results = new List<SimulateDungeonResponse>();
        foreach (var enemy in enemies)
        {
            var simulationResult = dungeonSimulator.SimulateDungeon(enemy, player, companions,
                new(request.Iterations, request.WinThreshold, false));

            var xpReward = gameFormulasService.GetExperienceForDungeonEnemy(enemy);
            var dungeonMetadata = new DungeonMetadata(enemy.Position, enemy.Dungeon.Name,
                enemy.Name, enemy.Dungeon.Type.WithCompanions, player.Class, xpReward);

            results.Add(new SimulateDungeonResponse(dungeonMetadata,
                simulationResult.WonFights / (decimal)request.Iterations));
        }

        //var en = dungeonProvider.GetDungeonEnemy(121, 7);
        //var res = dungeonSimulator.SimulateDungeon(en, player, companions, new(request.Iterations, request.WinThreshold, false));

        return TypedResults.Ok(results);
    }

    private static IEnumerable<(int DungeonPosition, int BeatenEnemyPosition)> MapDungeonProgress(int[] progress, bool shadow)
    {
        for (var sfToolsIndex = 0; sfToolsIndex < SfToolsToGameDungeonIndices.Length; sfToolsIndex++)
        {
            var gameIndex = SfToolsToGameDungeonIndices[sfToolsIndex];
            if (gameIndex >= progress.Length) continue;

            var dungeonPosition = DungeonProvider.MapDungeonIndex(sfToolsIndex + 1);
            yield return (dungeonPosition + (shadow ? 100 : 0), progress[gameIndex]);
        }

        // Special slots are excluded from the standard SF Tools progress arrays.
        if (progress.Length > 14)
            yield return (shadow ? -2 : -1, progress[14]);
        if (shadow && progress.Length > 17)
            yield return (-3, progress[17]);
        if (!shadow && progress.Length > 31)
            yield return (-4, progress[31]);
    }
}

public record class SimulateDungeonResponse(DungeonMetadata DungeonMetadata, decimal WinRatio);


public record class SimulateDungeonRequest(
    int Iterations, int WinThreshold,
    DungeonState DungeonState,
    Player Player,
    List<Companion> Companions
);

public record class DungeonState(int[] Light, int[] Shadow, int[] Class);

public record class Weapon(int MinDmg, int MaxDmg, RuneType RuneType, int RuneValue) : IWeaponable;

public record class Player(
    ClassType Class,
    int Level,
    int Strength,
    int Dexterity,
    int Intelligence,
    int Constitution,
    int Luck,
    long Health,
    int Armor,
    Weapon? FirstWeapon,
    Weapon? SecondWeapon,
    int Reaction,
    double CritMultiplier,
    int LightningResistance,
    int FireResistance,
    int ColdResistance,
    double GuildPortal
) : IFightable<Weapon>;

public record class Companion(
    ClassType Class,
    int Strength,
    int Dexterity,
    int Intelligence,
    int Constitution,
    int Luck,
    long Health,
    int Armor,
    Weapon? FirstWeapon,
    int Reaction,
    double CritMultiplier,
    int LightningResistance,
    int FireResistance,
    int ColdResistance
);

public record class DungeonMetadata(int Position, string DungeonName, string EnemyName, bool WithCompanions, ClassType Class, long Experience);