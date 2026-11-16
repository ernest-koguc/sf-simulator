using System.Diagnostics;

namespace SFSimulator.Core;

public class DungeonSimulator(IFightContextFactory dungeonableContextFactory, IGameFormulasService gameFormulasService,
    IItemGenerator itemGenerator) : IDungeonSimulator
{
    private readonly IFightContextFactory _fightableContextFactory = dungeonableContextFactory;
    private readonly IGameFormulasService _gameFormulasService = gameFormulasService;
    private readonly IItemGenerator _itemGenerator = itemGenerator;

    public PetSimulationResult SimulatePetDungeon(PetFightable petDungeonEnemy, PetFightable playerPet, int simulationContextLevel,
        DungeonSimulationOptions options)
    {
        var random = new CombatRandom();
        var playerPetContext = _fightableContextFactory.Create(playerPet, petDungeonEnemy, random);
        var petDungeonContext = _fightableContextFactory.Create(petDungeonEnemy, playerPet, random);

        DrHouse.DungeonDifferential($"Pet habitat {petDungeonEnemy.ElementType} - position {petDungeonEnemy.Position}:");

        var result = SimulateFight([(playerPetContext, petDungeonContext)], options);

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

    public Task<DungeonSimulationResult> SimulateDungeonAsync<T, E>(DungeonEnemy dungeonEnemy, IFightable<T> character, IFightable<E>[] companions, DungeonSimulationOptions options) where T : IWeaponable where E : IWeaponable
    {
        return Task.Run(() => SimulateDungeon(dungeonEnemy, character, companions, options));
    }

    public DungeonSimulationResult SimulateDungeon<T, E>(DungeonEnemy dungeonEnemy, IFightable<T> character, IFightable<E>[] companions,
        DungeonSimulationOptions options) where T : IWeaponable where E : IWeaponable
    {
        var random = new CombatRandom();
        var companionCount = dungeonEnemy.Dungeon.Type.WithCompanions ? companions.Length : 0;
        var lookupContext = new (FightContext LeftSide, FightContext RightSide)[companionCount + 1];

        if (dungeonEnemy.Dungeon.Type.WithCompanions)
        {
            for (var i = 0; i < companions.Length; i++)
            {
                var context = _fightableContextFactory.Create(companions[i], dungeonEnemy, random);
                var companionDungeonContext = _fightableContextFactory.Create(dungeonEnemy, companions[i], random);
                lookupContext[i] = (context, companionDungeonContext);
            }
        }

        var characterContext = _fightableContextFactory.Create(character, dungeonEnemy, random);
        var dungeonContext = _fightableContextFactory.Create(dungeonEnemy, character, random);
        lookupContext[companionCount] = (characterContext, dungeonContext);

        DrHouse.DungeonDifferential($"{dungeonEnemy.Dungeon.Type} {dungeonEnemy.Dungeon.Name} - {dungeonEnemy.Name}:");

        var result = SimulateFight(lookupContext, options);

        return CreateDungeonSimulationResult(result.WonFights, result.Succeeded, dungeonEnemy, character.Level);
    }

    // TODO: at one point we should consider either renaming DungeonSimulator or moving this out to different interface
    public FightSimulationResult SimulateArenaFight<T, E>(IFightable<T> left, IFightable<E> right, DungeonSimulationOptions options)
        where T : IWeaponable
        where E : IWeaponable
    {
        var random = new CombatRandom();
        var leftContext = _fightableContextFactory.Create(left, right, random);
        var rightContext = _fightableContextFactory.Create(right, left, random);

        return SimulateFight([(leftContext, rightContext)], options);
    }


    private FightSimulationResult SimulateFight((FightContext LeftSide, FightContext RightSide)[] lookupContext,
        DungeonSimulationOptions options)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var wonFights = 0;

        for (var i = 1; i <= options.Iterations; i++)
        {
            if (PerformSingleFight(lookupContext))
                wonFights++;

            // If we reached the win threshold and we are not debugging, we can stop early
            if (wonFights >= options.WinThreshold && options.Optimise)
            {
                break;
            }

            // If we are debugging and we know that we won't reach the win threshold, we can stop early
            if (wonFights + (options.Iterations - i) < options.WinThreshold && options.Optimise)
            {
                break;
            }
        }

        stopwatch.Stop();

        var winratio = wonFights / (float)options.Iterations;

        DrHouse.DungeonDifferential($"{winratio:P} WR, {wonFights} WF, elapsed time: {stopwatch.Elapsed.TotalMilliseconds}");

        return new FightSimulationResult(wonFights, wonFights >= options.WinThreshold);
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

    private bool PerformSingleFight((FightContext LeftSide, FightContext RightSide)[] lookupContext)
    {
        double? leftoverHealth = null;
        foreach (ref var pair in lookupContext.AsSpan())
        {
            ref var charSide = ref pair.LeftSide;
            ref var dungeonSide = ref pair.RightSide;
            if (leftoverHealth.HasValue)
                dungeonSide.Health = leftoverHealth.Value;

            var characterWon = PerformFight(ref charSide, ref dungeonSide);
            leftoverHealth = dungeonSide.Health;
            charSide.ResetState();
            dungeonSide.ResetState();

            if (characterWon)
                return true;
        }
        return false;
    }

    private bool PerformFight(ref FightContext charSide, ref FightContext enemySide)
    {
        var charSideStarts = charSide.Reaction > enemySide.Reaction ||
                            (charSide.Reaction == enemySide.Reaction
                             && charSide.Random.NextDouble() < 0.5);

        ref var attacker = ref (charSideStarts ? ref charSide : ref enemySide);
        ref var defender = ref (charSideStarts ? ref enemySide : ref charSide);

        var round = 0;

        if (attacker.AttackBeforeFight(ref defender, ref round))
        {
            return charSideStarts;
        }

        if (defender.AttackBeforeFight(ref attacker, ref round))
        {
            return !charSideStarts;
        }

        bool? result = null;

        for (var i = 0; i < int.MaxValue; i++)
        {
            var skipRound = defender.WillSkipRound(ref round);

            if (!skipRound && attacker.Attack(ref defender, ref round))
            {
                result = charSideStarts;
                break;
            }

            skipRound = attacker.WillSkipRound(ref round);

            if (!skipRound && defender.Attack(ref attacker, ref round))
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