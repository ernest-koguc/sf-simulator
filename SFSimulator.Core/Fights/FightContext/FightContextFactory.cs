namespace SFSimulator.Core;

public class FightContextFactory(IDamageProvider damageProvider, ICritChanceProvider critChanceProvider) : IFightContextFactory
{
    public FightContext Create<T, E>(IFightable<T> main, IFightable<E> opponent, CombatRandom random)
        where T : IWeaponable where E : IWeaponable
    {
        var context = new FightContext
        {
            Class = main.Class,
            OpponentClass = opponent.Class,
            Random = random
        };

        context.InitializeClassState(main, opponent, damageProvider, critChanceProvider);

        (context.MinimumDamage, context.MaximumDamage) = damageProvider.CalculateDamage(main.FirstWeapon, main, opponent);
        context.Health = main.Health;
        context.MaxHealth = main.Health;
        context.CritMultiplier = main.CritMultiplier;
        context.CritChance = critChanceProvider.CalculateCritChance(main, opponent);
        context.Reaction = main.Reaction;
        return context;
    }
}