namespace SFSimulator.Core;

public class FightableContextFactory(IDamageProvider damageProvider, ICritChanceProvider critChanceProvider,
        IBonusMelodyLengthProvider bonusMelodyLengthProvider, Random random) : IFightableContextFactory
{
    private readonly IDamageProvider _damageProvider = damageProvider;
    private readonly ICritChanceProvider _critChanceProvider = critChanceProvider;
    private readonly IBonusMelodyLengthProvider _bonusMelodyLengthProvider = bonusMelodyLengthProvider;
    private readonly Random _random = random;

    public IFightableContext Create<T, E>(IFightable<T> main, IFightable<E> opponent) where T : IWeaponable where E : IWeaponable
    {
        DelegatableFightableContext context = main.Class switch
        {
            ClassType.Warrior => CreateWarriorContext(),
            ClassType.Mage => CreateMageContext(),
            ClassType.Scout => CreateScoutContext(),
            ClassType.Assassin => CreateAssassinContext(main, opponent),
            ClassType.Bert => CreateShieldlessWarriorContext(),
            ClassType.BattleMage => CreateBattleMageContext(main, opponent),
            ClassType.Berserker => CreateBerserkerContext(main, opponent),
            ClassType.Druid => CreateDruidContext(main, opponent),
            ClassType.DemonHunter => CreateDemonHunterContext(opponent),
            ClassType.Bard => CreateBardContext(main, opponent),
            ClassType.Necromancer => CreateNecromancerContext(main, opponent),
            ClassType.Paladin => CreatePaladinContext(main, opponent),
            ClassType.PlagueDoctor => CreatePlagueDoctorContext(main, opponent),
            _ => throw new ArgumentOutOfRangeException(nameof(main), $"{main.Class} is not supported in FightableContext"),
        };
        InstantiateSharedProperties(context, main, opponent);

        return context;
    }

    private void InstantiateSharedProperties<T, E>(DelegatableFightableContext context, IFightable<T> main, IFightable<E> opponent) where T : IWeaponable where E : IWeaponable
    {
        context.Random = _random;
        var (Minimum, Maximum) = _damageProvider.CalculateDamage(main.FirstWeapon, main, opponent);
        context.MinimumDamage = Minimum;
        context.MaximumDamage = Maximum;
        context.Health = main.Health;
        context.MaxHealth = main.Health;
        context.CritMultiplier = main.CritMultiplier;
        context.CritChance = _critChanceProvider.CalculateCritChance(main, opponent);
        context.Reaction = main.Reaction;
    }

    private static WarriorFightContext CreateWarriorContext()
    {
        var context = new WarriorFightContext
        {
            BlockChance = 25
        };
        return context;
    }

    private static WarriorFightContext CreateShieldlessWarriorContext()
    {
        var context = new WarriorFightContext
        {
            BlockChance = 0
        };
        return context;
    }

    private static MageFightContext CreateMageContext()
    {
        var context = new MageFightContext();
        return context;
    }

    private static ScoutFightContext CreateScoutContext()
    {
        var context = new ScoutFightContext();
        return context;
    }

    private AssassinFightContext CreateAssassinContext<T, E>(IFightable<T> main, IFightable<E> opponent) where T : IWeaponable where E : IWeaponable
    {
        var context = new AssassinFightContext();
        var (Minimum, Maximum) = _damageProvider.CalculateDamage(main.SecondWeapon, main, opponent, true);
        context.SecondMinimumDamage = Minimum;
        context.SecondMaximumDamage = Maximum;
        return context;
    }

    private BattleMageFightContext CreateBattleMageContext<T, E>(IFightable<T> main, IFightable<E> opponent) where T : IWeaponable where E : IWeaponable
    {
        var context = new BattleMageFightContext
        {
            FireBallDamage = _damageProvider.CalculateFireBallDamage(main, opponent)
        };
        return context;
    }

    private static BerserkerFightContext CreateBerserkerContext<T, E>(IFightable<T> main, IFightable<E> opponent) where T : IWeaponable where E : IWeaponable
    {
        var context = new BerserkerFightContext(opponent.Class);
        return context;
    }

    private DruidFightContext CreateDruidContext<T, E>(IFightable<T> main, IFightable<E> opponent) where T : IWeaponable where E : IWeaponable
    {
        var context = new DruidFightContext(opponent.Class)
        {
            RageCritChance = _critChanceProvider.CalculateCritChance(main, opponent, 0.75D, 0.1)
        };
        return context;
    }

    private static DemonHunterFightContext CreateDemonHunterContext<T>(IFightable<T> opponent) where T : IWeaponable
    {
        var context = new DemonHunterFightContext(opponent.Class);
        return context;
    }

    private BardFightContext CreateBardContext<T, E>(IFightable<T> main, IFightable<E> opponent) where T : IWeaponable where E : IWeaponable
    {
        var bonusLength = _bonusMelodyLengthProvider.GetBonusMelodyLength(main);
        var context = new BardFightContext(opponent.Class, bonusLength);
        return context;
    }

    private NecromancerFightContext CreateNecromancerContext<T, E>(IFightable<T> main, IFightable<E> opponent)
        where T : IWeaponable
        where E : IWeaponable
    {

        var context = new NecromancerFightContext(opponent.Class)
        {
            BaseDamageMultiplier = _damageProvider.CalculateDamageMultiplier(main, opponent)
        };
        return context;
    }

    private PaladinFightContext CreatePaladinContext<T, E>(IFightable<T> main, IFightable<E> opponent) where T : IWeaponable where E : IWeaponable
    {
        var damageReduction = _damageProvider.CalculateDamageReduction(opponent, main);
        var context = new PaladinFightContext(opponent.Class)
        {
            InitialArmorReduction = damageReduction
        };
        return context;
    }

    private PlagueDoctorFightContext CreatePlagueDoctorContext<T, E>(IFightable<T> main, IFightable<E> opponent) where T : IWeaponable where E : IWeaponable
    {
        var baseDmgMultiplier = _damageProvider.CalculateDamageMultiplier(main, opponent);
        var context = new PlagueDoctorFightContext(opponent.Class, baseDmgMultiplier);
        return context;
    }
}