using System.Diagnostics;

namespace SFSimulator.Core;

public class DungeonSimulator : IDungeonSimulator
{
    private readonly IDungeonProvider _dungeonProvider;
    private readonly IFightableContextFactory _dungeonableContextFactory;
    private readonly IFightStartingBehaviorProvider _fightStartingBehaviorProvider;
    private readonly IGameLogic _gameLogic;
    private readonly Random _random;

    //TODO: DungeonSimulatorOptions???? one solution for other sims as well (maybe like interface with Configure<TOptions> method?

    public DungeonSimulator(IDungeonProvider dungeonProvider, IFightableContextFactory dungeonableContextFactory, IFightStartingBehaviorProvider fightStartingBehaviorProvider, IGameLogic gameLogic, Random random)
    {
        _dungeonProvider = dungeonProvider;
        _dungeonableContextFactory = dungeonableContextFactory;
        _fightStartingBehaviorProvider = fightStartingBehaviorProvider;
        _gameLogic = gameLogic;
        _random = random;
    }

    public async Task<DungeonSimulationResult> SimulateAllOpenDungeonsAsync(Character character, int iterations, int winThreshold)
    {
        if (iterations <= 0)
            throw new ArgumentOutOfRangeException(nameof(iterations));

        var newChar = new Character()
        //{
        //    Level = 439,
        //    Armor = 4930,
        //    Strength = 1821,
        //    Dexterity = 1807,
        //    Intelligence = 49597,
        //    Constitution = 32390,
        //    Luck = 12353,
        //    MaxWeaponDmg = 2883,
        //    MinWeaponDmg = 997,
        //    SoloPortal = 50,
        //    GuildPortal = 50,
        //    Class = ClassType.Mage,
        //    HasGlovesScroll = true,
        //    HasWeaponScroll = true,
        //    RuneBonuses = new FightRuneBonuses { DamageRuneType = DamageRuneType.Lightning, DamageBonus = 60, ColdResistance = 60, FireResistance = 60, LightningResistance = 60, HealthRune = 15 },
        //    GladiatorLevel = 15
        //};
        {
            Level = 227,
            Armor = 20000,
            Strength = 500,
            Dexterity = 500,
            Intelligence = 7000,
            Constitution = 9000,
            Luck = 5000,
            FirstWeapon = new Weapon { MinDmg = 774, MaxDmg = 1449, DamageRuneType = DamageRuneType.None },
            SoloPortal = 0,
            GuildPortal = 0,
            Class = ClassType.Bard,
            HasGlovesScroll = false,
            HasWeaponScroll = true,
            HasEternityPotion = true,
            GladiatorLevel = 0
        };

        var enemy = _dungeonProvider.GetDungeonEnemy(15, 1);
        var singleFight = SimulateDungeon(enemy, newChar, iterations, winThreshold);
        return singleFight;
    }

    public DungeonSimulationResult SimulateDungeon(DungeonEnemy dungeonEnemy, Character character, int iterations, int winThreshold)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var characterInDungeon = new FightableCharacter(character);
        var characterContext = _dungeonableContextFactory.Create(characterInDungeon, dungeonEnemy);
        var dungeonContext = _dungeonableContextFactory.Create(dungeonEnemy, characterInDungeon);
        var startingBehavior = _fightStartingBehaviorProvider.GetStartingBehavior(characterInDungeon, dungeonEnemy);

        var fight = new DungeonFight(characterContext, dungeonContext, startingBehavior, _random);
        var wonFights = 0;

        //Parallel.For(0, iterations, (_, state) =>
        //{
        //    var characterWon = fight.PerformFight();
        //    if (characterWon)
        //        Interlocked.Increment(ref wonFights);

        //    if (wonFights >= winThreshold)
        //        state.Break();
        //});
        for (var i = 0; i < iterations; i++)
        {

            var characterWon = fight.PerformFight();
            if (characterWon)
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

    private DungeonSimulationResult CreateResult(int wonFights, int winTreshold, Character character, DungeonEnemy dungeonEnemy)
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
}
