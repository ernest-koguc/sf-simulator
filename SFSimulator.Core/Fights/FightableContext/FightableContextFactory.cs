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

    public IFightableContext Create(IFightable main, IFightable opponent)
    {
        var context = main.Class switch
        {
            ClassType.Warrior => CreateWarriorContext(),
            ClassType.Mage => CreateMageContext(),
            ClassType.Scout => CreateScoutContext(),
            ClassType.Assassin => CreateAssassinContext(main, opponent),
            ClassType.ShieldlessWarrior => CreateShieldlessWarriorContext(),
            ClassType.BattleMage => CreateBattleMageContext(main, opponent),
            ClassType.Berserker => CreateBerserkerContext(),
            ClassType.Druid => CreateDruidContext(main, opponent),
            ClassType.DemonHunter => CreateDemonHunterContext(opponent),
            ClassType.Bard => CreateBardContext(main, opponent),
            _ => throw new ArgumentOutOfRangeException(nameof(main.Class)),
        };
        InstantiateSharedProperties(context, main, opponent);

        return context;
    }

    private void InstantiateSharedProperties(DelegatableFightableContext context, IFightable main, IFightable opponent)
    {
        context.Random = _random;
        var (Minimum, Maximum) = _damageProvider.CalculateDamage(main.FirstWeapon, main, opponent);
        context.MinimumDamage = Minimum;
        context.MaximumDamage = Maximum;
        context.Health = main.Health;
        context.MaxHealth = main.Health;
        context.CritMultiplier = main.CritMultiplier;
        context.CritChance = _critChanceProvider.CalculateCritChance(main, opponent);
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
    private DelegatableFightableContext CreateAssassinContext(IFightable main, IFightable opponent)
    {
        var context = new AssassinFightContext();
        var (Minimum, Maximum) = _damageProvider.CalculateDamage(main.SecondWeapon, main, opponent);
        context.SecondMinimumDamage = Minimum;
        context.SecondMaximumDamage = Maximum;
        return context;
    }
    private DelegatableFightableContext CreateBattleMageContext(IFightable main, IFightable opponent)
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
    private DelegatableFightableContext CreateDruidContext(IFightable main, IFightable opponent)
    {
        var context = new DruidFightContext
        {
            RageCritChance = _critChanceProvider.CalculateCritChance(main, opponent, 0.75D)
        };
        return context;
    }
    private static DelegatableFightableContext CreateDemonHunterContext(IFightable opponent)
    {
        var context = new DemonHunterFightContext(opponent.Class);
        return context;
    }
    private DelegatableFightableContext CreateBardContext(IFightable main, IFightable opponent)
    {
        var bonusLength = _bonusMelodyLengthProvider.GetBonusMelodyLength(main);
        var context = new BardFightContext(opponent.Class, bonusLength);
        return context;
    }
}
