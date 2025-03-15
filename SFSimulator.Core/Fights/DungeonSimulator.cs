using System.Diagnostics;

namespace SFSimulator.Core;

public class DungeonSimulator : IDungeonSimulator
{
    private readonly IFightableContextFactory _fightableContextFactory;
    private readonly IGameFormulasService _gameFormulasService;
    private readonly IItemGenerator _itemGenerator;
    private readonly Random _random;

    public DungeonSimulator(IFightableContextFactory dungeonableContextFactory, IGameFormulasService gameFormulasService, IItemGenerator itemGenerator, Random random)
    {
        _fightableContextFactory = dungeonableContextFactory;
        _gameFormulasService = gameFormulasService;
        _itemGenerator = itemGenerator;
        _random = random;
    }

    public PetSimulationResult SimulatePetDungeon(PetFightable petDungeonEnemy, PetFightable playerPet, int simulationContextLevel, int iterations, int winThreshold)
    {
        var lookupContext = new List<(IFightableContext LeftSide, IFightableContext RightSide)>();
        var playerPetContext = _fightableContextFactory.Create(playerPet, petDungeonEnemy);
        var petDungeonContext = _fightableContextFactory.Create(petDungeonEnemy, playerPet);
        lookupContext.Add((playerPetContext, petDungeonContext));

        Console.WriteLine($"Pet habitat {petDungeonEnemy.ElementType} - position {petDungeonEnemy.Position}:");
        var result = SimulateFight(lookupContext, iterations, winThreshold);

        if (result.Succeeded)
        {
            var xp = _gameFormulasService.GetExperienceForPetDungeonEnemy(simulationContextLevel);
            return new PetSimulationResult(result.WonFights, result.Succeeded, xp);
        }
        else
        {
            return PetSimulationResult.FailedResult(result.WonFights);
        }
    }

    public DungeonSimulationResult SimulateDungeon<T, E>(DungeonEnemy dungeonEnemy, IFightable<T> character, IFightable<E>[] companions, int iterations, int winThreshold)
        where T : IWeaponable where E : IWeaponable
    {
        var lookupContext = new List<(IFightableContext LeftSide, IFightableContext RightSide)>();

        if (dungeonEnemy.Dungeon.Type.WithCompanions())
        {
            foreach (var companion in companions)
            {
                var context = _fightableContextFactory.Create(companion, dungeonEnemy);
                var companionDungeonContext = _fightableContextFactory.Create(dungeonEnemy, companion);
                lookupContext.Add((context, companionDungeonContext));
            }
        }

        var characterContext = _fightableContextFactory.Create(character, dungeonEnemy);
        var dungeonContext = _fightableContextFactory.Create(dungeonEnemy, character);
        lookupContext.Add((characterContext, dungeonContext));

        Console.WriteLine($"{dungeonEnemy.Dungeon.Type} {dungeonEnemy.Dungeon.Name} - {dungeonEnemy.Name}:");
        var result = SimulateFight(lookupContext, iterations, winThreshold);

        return CreateDungeonSimulationResult(result.WonFights, result.Succeeded, dungeonEnemy, character.Level);
    }

    private FightSimulationResult SimulateFight(List<(IFightableContext LeftSide, IFightableContext RightSide)> lookupContext, int iterations, int winThreshold)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var wonFights = 0;

        for (var i = 0; i < iterations; i++)
        {
            if (PerformSingleFight(lookupContext))
                wonFights++;

            if (wonFights >= winThreshold && !Debugger.IsAttached)
                break;
        }

        stopwatch.Stop();

        var winratio = wonFights / (float)iterations;

        Console.WriteLine($"{winratio:P} WR, {wonFights} WF, elapsed time: {stopwatch.Elapsed.TotalMilliseconds}");

        return new FightSimulationResult(wonFights, wonFights >= winThreshold);
    }

    private DungeonSimulationResult CreateDungeonSimulationResult(int wonFights, bool suceeded, DungeonEnemy dungeonEnemy, int characterLevel)
    {
        if (!suceeded)
            return DungeonSimulationResult.FailedResult(wonFights, dungeonEnemy);

        var xp = _gameFormulasService.GetExperienceForDungeonEnemy(dungeonEnemy);
        var gold = _gameFormulasService.GetGoldForDungeonEnemy(dungeonEnemy);

        Item? item = null;
        if (_gameFormulasService.DoesDungeonEnemyDropItem(dungeonEnemy))
        {
            item = _itemGenerator.GenerateItem(Math.Min(dungeonEnemy.Level, characterLevel));
        }

        var result = new DungeonSimulationResult(true, xp, gold, item, wonFights, dungeonEnemy);

        return result;
    }

    private bool PerformSingleFight(List<(IFightableContext LeftSide, IFightableContext RightSide)> lookupContext)
    {
        long? leftoverHealth = null;
        foreach (var pair in lookupContext)
        {
            var charSide = pair.LeftSide;
            var dungeonSide = pair.RightSide;

            if (leftoverHealth.HasValue)
                dungeonSide.Health = leftoverHealth.Value;

            var characterWon = PerformFight(charSide, dungeonSide);
            leftoverHealth = dungeonSide.Health;
            charSide.ResetState();
            dungeonSide.ResetState();

            if (characterWon)
                return true;
        }
        return false;
    }

    private bool PerformFight(IFightableContext charSide, IFightableContext dungeonSide)
    {
        var charSideStarts = charSide.Reaction > dungeonSide.Reaction ? true : _random.NextDouble() < 0.5;
        var (attacker, defender) = charSideStarts ? (charSide, dungeonSide) : (dungeonSide, charSide);

        var round = 0;

        if (attacker is IBeforeFightAttackable attackerImpl && attackerImpl.AttackBeforeFight(defender, ref round))
        {
            return charSideStarts;
        }

        if (defender is IBeforeFightAttackable defenderImpl && defenderImpl.AttackBeforeFight(defender, ref round))
        {
            return !charSideStarts;
        }

        bool? result = null;

        for (var i = 0; i < int.MaxValue; i++)
        {
            if (attacker.Attack(defender, ref round))
            {
                result = charSideStarts;
                break;
            }

            if (defender.Attack(attacker, ref round))
            {
                result = !charSideStarts;
                break;
            }
        }
        if (result is null)
        {
            throw new Exception("Internal error in dungeon simulator causing infinite rounds in fight");
        }

        return result.Value;
    }
}