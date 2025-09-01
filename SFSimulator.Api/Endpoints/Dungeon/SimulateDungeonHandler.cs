using Microsoft.AspNetCore.Http.HttpResults;
using SFSimulator.Core;

namespace SFSimulator.Api;

public class SimulateDungeonHandler(IDungeonSimulator dungeonSimulator, IDungeonProvider dungeonProvider, IGameFormulasService gameFormulasService)
{
    public Results<Ok<List<SimulateDungeonResponse>>, BadRequest<string>> HandleSimulateDungeonAsync(SimulateDungeonRequest request)
    {
        if (request.Iterations > 1_000_000)
        {
            return TypedResults.BadRequest("Iterations cannot be greater than 1 000 000.");
        }

        dungeonProvider.InitDungeons();
        var enemies = request.DungeonLevels
            .Select(kvp => dungeonProvider.GetDungeonEnemySafe(kvp.Key, kvp.Value + 1))
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
                enemy.Name, enemy.Dungeon.Type.WithCompanions(), player.Class, xpReward);

            results.Add(new SimulateDungeonResponse(dungeonMetadata,
                simulationResult.WonFights / (decimal)request.Iterations));
        }

        //var en = dungeonProvider.GetDungeonEnemy(121, 7);
        //var res = dungeonSimulator.SimulateDungeon(en, player, companions, new(request.Iterations, request.WinThreshold, false));

        return TypedResults.Ok(results);
    }
}

public record class SimulateDungeonResponse(DungeonMetadata DungeonMetadata, decimal WinRatio);


public record class SimulateDungeonRequest(
    int Iterations, int WinThreshold,
    Dictionary<int, int> DungeonLevels,
    Player Player,
    List<Companion> Companions
);

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
