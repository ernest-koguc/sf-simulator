namespace SFSimulator.Core;

public class FightableContextFactory : IFightableContextFactory
{
    private readonly IDamageProvider _damageProvider;
    private readonly ICritChanceProvider _critChanceProvider;
    private readonly IBonusMelodyLengthProvider _bonusMelodyLengthProvider;
    private readonly Random _random;

    public FightableContextFactory(IDamageProvider damageProvider, ICritChanceProvider critChanceProvider, IBonusMelodyLengthProvider bonusMelodyLengthProvider, Random random)
    {
        _damageProvider = damageProvider;
        _random = random;
        _critChanceProvider = critChanceProvider;
        _bonusMelodyLengthProvider = bonusMelodyLengthProvider;
    }

    public IFightableContext Create<T, E>(IFightable<T> main, IFightable<E> opponent) where T : IWeaponable where E : IWeaponable
    {
        var context = main.Class switch
        {
            ClassType.Warrior => CreateWarriorContext(),
            ClassType.Mage => CreateMageContext(),
            ClassType.Scout => CreateScoutContext(),
            ClassType.Assassin => CreateAssassinContext(main, opponent),
            ClassType.Bert => CreateShieldlessWarriorContext(),
            ClassType.BattleMage => CreateBattleMageContext(main, opponent),
            ClassType.Berserker => CreateBerserkerContext(),
            ClassType.Druid => CreateDruidContext(main, opponent),
            ClassType.DemonHunter => CreateDemonHunterContext(opponent),
            ClassType.Bard => CreateBardContext(main, opponent),
            ClassType.Paladin => CreatePaladinContext(main, opponent),
            _ => throw new ArgumentOutOfRangeException(nameof(main.Class)),
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

    private static DelegatableFightableContext CreateWarriorContext()
    {
        var context = new WarriorFightContext
        {
            BlockChance = 25
        };
        return context;
    }

    private static DelegatableFightableContext CreateShieldlessWarriorContext()
    {
        var context = new WarriorFightContext
        {
            BlockChance = 0
        };
        return context;
    }

    private static DelegatableFightableContext CreateMageContext()
    {
        var context = new MageFightContext();
        return context;
    }

    private static DelegatableFightableContext CreateScoutContext()
    {
        var context = new ScoutFightContext();
        return context;
    }

    private DelegatableFightableContext CreateAssassinContext<T, E>(IFightable<T> main, IFightable<E> opponent) where T : IWeaponable where E : IWeaponable
    {
        var context = new AssassinFightContext();
        var (Minimum, Maximum) = _damageProvider.CalculateDamage(main.SecondWeapon, main, opponent);
        context.SecondMinimumDamage = Minimum;
        context.SecondMaximumDamage = Maximum;
        return context;
    }

    private DelegatableFightableContext CreateBattleMageContext<T, E>(IFightable<T> main, IFightable<E> opponent) where T : IWeaponable where E : IWeaponable
    {
        var context = new BattleMageFightContext
        {
            FireBallDamage = _damageProvider.CalculateFireBallDamage(main, opponent)
        };
        return context;
    }

    private static DelegatableFightableContext CreateBerserkerContext()
    {
        var context = new BerserkerFightContext();
        return context;
    }

    private DelegatableFightableContext CreateDruidContext<T, E>(IFightable<T> main, IFightable<E> opponent) where T : IWeaponable where E : IWeaponable
    {
        var context = new DruidFightContext
        {
            RageCritChance = _critChanceProvider.CalculateCritChance(main, opponent, 0.75D)
        };
        return context;
    }

    private static DelegatableFightableContext CreateDemonHunterContext<T>(IFightable<T> opponent) where T : IWeaponable
    {
        var context = new DemonHunterFightContext(opponent.Class);
        return context;
    }

    private DelegatableFightableContext CreateBardContext<T, E>(IFightable<T> main, IFightable<E> opponent) where T : IWeaponable where E : IWeaponable
    {
        var bonusLength = _bonusMelodyLengthProvider.GetBonusMelodyLength(main);
        var context = new BardFightContext(opponent.Class, bonusLength);
        return context;
    }

    private DelegatableFightableContext CreatePaladinContext<T, E>(IFightable<T> main, IFightable<E> opponent) where T : IWeaponable where E : IWeaponable
    {
        var damageReduction = 0d;
        if (main.Armor > 0 && opponent.Class != ClassType.Mage)
            damageReduction = Math.Min(0.45, (double)main.Armor / opponent.Level / 100D);
        var context = new PaladinFightContext(opponent.Class)
        {
            InitialArmorReduction = damageReduction
        };
        return context;
    }
}