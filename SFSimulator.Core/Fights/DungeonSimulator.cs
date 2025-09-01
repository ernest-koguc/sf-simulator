using System.Diagnostics;

namespace SFSimulator.Core;

public class DungeonSimulator(IFightableContextFactory dungeonableContextFactory, IGameFormulasService gameFormulasService,
    IItemGenerator itemGenerator, Random random) : IDungeonSimulator
{
    private readonly IFightableContextFactory _fightableContextFactory = dungeonableContextFactory;
    private readonly IGameFormulasService _gameFormulasService = gameFormulasService;
    private readonly IItemGenerator _itemGenerator = itemGenerator;
    private readonly Random _random = random;

    public PetSimulationResult SimulatePetDungeon(PetFightable petDungeonEnemy, PetFightable playerPet, int simulationContextLevel,
        DungeonSimulationOptions options)
    {
        var lookupContext = new List<(IFightableContext LeftSide, IFightableContext RightSide)>();
        var playerPetContext = _fightableContextFactory.Create(playerPet, petDungeonEnemy);
        var petDungeonContext = _fightableContextFactory.Create(petDungeonEnemy, playerPet);
        lookupContext.Add((playerPetContext, petDungeonContext));

        DrHouse.DungeonDifferential($"Pet habitat {petDungeonEnemy.ElementType} - position {petDungeonEnemy.Position}:");

        var result = SimulateFight(lookupContext, options);

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

    public DungeonSimulationResult SimulateDungeon<T, E>(DungeonEnemy dungeonEnemy, IFightable<T> character, IFightable<E>[] companions,
        DungeonSimulationOptions options) where T : IWeaponable where E : IWeaponable
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

        DrHouse.DungeonDifferential($"{dungeonEnemy.Dungeon.Type} {dungeonEnemy.Dungeon.Name} - {dungeonEnemy.Name}:");

        var result = SimulateFight(lookupContext, options);

        return CreateDungeonSimulationResult(result.WonFights, result.Succeeded, dungeonEnemy, character.Level);
    }

    private FightSimulationResult SimulateFight(List<(IFightableContext LeftSide, IFightableContext RightSide)> lookupContext,
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

    private bool PerformSingleFight(List<(IFightableContext LeftSide, IFightableContext RightSide)> lookupContext)
    {
        long? leftoverHealth = null;
        foreach (var (CharSide, DungeonSide) in lookupContext)
        {
            if (leftoverHealth.HasValue)
                DungeonSide.Health = leftoverHealth.Value;

            var characterWon = PerformFight(CharSide, DungeonSide);
            leftoverHealth = DungeonSide.Health;
            CharSide.ResetState();
            DungeonSide.ResetState();

            if (characterWon)
                return true;
        }
        return false;
    }

    private bool PerformFight(IFightableContext charSide, IFightableContext dungeonSide)
    {
        var charSideStarts = charSide.Reaction > dungeonSide.Reaction || _random.NextDouble() < 0.5;
        var (attacker, defender) = charSideStarts ? (charSide, dungeonSide) : (dungeonSide, charSide);

        var round = 0;

        if (attacker is IBeforeFightAttackable attackerImpl && attackerImpl.AttackBeforeFight(defender, ref round))
        {
            return charSideStarts;
        }

        if (defender is IBeforeFightAttackable defenderImpl && defenderImpl.AttackBeforeFight(attacker, ref round))
        {
            return !charSideStarts;
        }

        bool? result = null;

        for (var i = 0; i < int.MaxValue; i++)
        {
            var skipRound = defender is IRoundSkipable roundSkipable && roundSkipable.WillSkipRound(ref round);

            if (!skipRound && attacker.Attack(defender, ref round))
            {
                result = charSideStarts;
                break;
            }

            skipRound = attacker is IRoundSkipable roundSkipable1 && roundSkipable1.WillSkipRound(ref round);

            if (!skipRound && defender.Attack(attacker, ref round))
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