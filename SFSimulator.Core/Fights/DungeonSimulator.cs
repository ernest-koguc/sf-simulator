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

    public async Task<DungeonSimulationResult> SimulateAllOpenDungeonsAsync(Character character, int iterations, int winThreshold)
    {
        if (iterations <= 0)
            throw new ArgumentOutOfRangeException(nameof(iterations));
        var dungeonEnemy = _dungeonProvider.GetDungeonEnemy(1, 1);
        return CreateResult(1, winThreshold, character, dungeonEnemy);
    }
    public DungeonSimulationResult SimulateDungeon(DungeonEnemy dungeonEnemy, RawFightable character, int iterations, int winThreshold)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var lookupContext = new (IFightableContext LeftSide, IFightableContext RightSide)[4];
        var index = 0;

        if (dungeonEnemy.Dungeon.Type.WithCompanions())
        {
            foreach (var companion in character.Companions)
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

            if (wonFights >= winThreshold)
                break;
        }

        stopwatch.Stop();

        if (Debugger.IsAttached)
        {
            Console.WriteLine(wonFights / (float)iterations * 100 + "%" + $" ({wonFights})" + $", elapsed time: {stopwatch.Elapsed}");
        }

        return CreateResult(wonFights, winThreshold, character, dungeonEnemy);
    }

    public DungeonSimulationResult SimulateDungeon(DungeonEnemy dungeonEnemy, Character character, int iterations, int winThreshold)
    {
        var wonFights = 0;
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
        var leftSideStarts = charSide.Reaction > dungeonSide.Reaction ? true : _random.NextDouble() >= 0.5D;
        IFightableContext attacker;
        IFightableContext defender;
        if (leftSideStarts)
        {
            attacker = charSide;
            defender = dungeonSide;
        }
        else
        {
            attacker = dungeonSide;
            defender = charSide;
        }
        var round = 0;

        if (attacker is IBeforeFightAttackable attackerImpl && attackerImpl.AttackBeforeFight(defender, ref round))
        {
            return leftSideStarts;
        }

        if (defender is IBeforeFightAttackable defenderImpl && defenderImpl.AttackBeforeFight(defender, ref round))
        {
            return !leftSideStarts;
        }

        bool? result = null;

        for (var i = 0; i < int.MaxValue; i++)
        {
            if (attacker.Attack(defender, ref round))
            {
                result = leftSideStarts;
                break;
            }

            if (defender.Attack(attacker, ref round))
            {
                result = !leftSideStarts;
                break;
            }
        }
        if (result is null)
        {
            throw new Exception("Dungeon simulation exceeded max iterations");
        }

        return result.Value;
    }
}
