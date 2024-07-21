using System.Diagnostics;

namespace SFSimulator.Core;

public class DungeonSimulator : IDungeonSimulator
{
    private readonly IDungeonProvider _dungeonProvider;
    private readonly IFightableContextFactory _dungeonableContextFactory;
    private readonly IGameLogic _gameLogic;
    private readonly Random _random;

    public DungeonSimulator(IDungeonProvider dungeonProvider, IFightableContextFactory dungeonableContextFactory, IGameLogic gameLogic, Random random)
    {
        _dungeonProvider = dungeonProvider;
        _dungeonableContextFactory = dungeonableContextFactory;
        _gameLogic = gameLogic;
        _random = random;
    }

    public DungeonSimulationResult SimulateDungeon<T, E>(DungeonEnemy dungeonEnemy, IFightable<T> character, IFightable<E>[] companions, int iterations, int winThreshold) where T : IWeaponable where E : IWeaponable
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var lookupContext = new (IFightableContext LeftSide, IFightableContext RightSide)[1 + companions.Length];
        var index = 0;

        if (dungeonEnemy.Dungeon.Type.WithCompanions())
        {
            foreach (var companion in companions)
            {
                var context = _dungeonableContextFactory.Create(companion, dungeonEnemy);
                var companionDungeonContext = _dungeonableContextFactory.Create(dungeonEnemy, companion);
                lookupContext[index] = (context, companionDungeonContext);
                index++;
            }
        }

        var characterContext = _dungeonableContextFactory.Create(character, dungeonEnemy);
        var dungeonContext = _dungeonableContextFactory.Create(dungeonEnemy, character);
        lookupContext[index] = (characterContext, dungeonContext);

        var wonFights = 0;

        for (var i = 0; i < iterations; i++)
        {
            if (PerformSingleFight(lookupContext))
                wonFights++;

            if (wonFights >= winThreshold && !Debugger.IsAttached)
                break;
        }

        stopwatch.Stop();

        if (Debugger.IsAttached)
        {
            var winratio = wonFights / (float)iterations;
            //var currentColor = Console.ForegroundColor;
            //Console.ForegroundColor = ConsoleColor.Blue;
            //Console.WriteLine($"{winratio:P} WR, {wonFights} WF, {stopwatch.Elapsed.TotalMilliseconds} ms: ({dungeonEnemy.Dungeon.Name} - {dungeonEnemy.Name})");
            //Console.ForegroundColor = currentColor;
        }

        return CreateResult(wonFights, winThreshold, character, dungeonEnemy);
    }

    private DungeonSimulationResult CreateResult<T>(int wonFights, int winTreshold, IFightable<T> character, DungeonEnemy dungeonEnemy) where T : IWeaponable
    {
        if (wonFights < winTreshold)
            return DungeonSimulationResult.FailedResult(wonFights);

        var xp = _gameLogic.GetExperienceForDungeonEnemy(character.Level, dungeonEnemy);

        var result = new DungeonSimulationResult
        {
            Succeeded = true,
            Experience = xp,
            WonFights = wonFights,
            // TODO: Add logic for gold calculations
            Gold = 0
        };

        return result;
    }

    private bool PerformSingleFight((IFightableContext LeftSide, IFightableContext RightSide)[] lookupContext)
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
        var charSideStarts = charSide.Reaction > dungeonSide.Reaction ? true : _random.NextDouble() >= 0.5D;
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