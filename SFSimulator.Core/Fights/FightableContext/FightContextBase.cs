using static SFSimulator.Core.IFightableContext;

namespace SFSimulator.Core;

public abstract class FightContextBase : IFightableContext
{
    public int Reaction { get; set; }
    public double MaxHealth { get; set; }
    public double Health { get; set; }
    public double MinimumDamage { get; set; }
    public double MaximumDamage { get; set; }
    public double CritChance { get; set; }
    public double CritMultiplier { get; set; }

    public AttackDelegate AttackImplementation { get; protected set; } = default!;
    public TakeAttackDelegate TakeAttackImplementation { get; protected set; } = default!;
    public WillTakeAttackDelegate WillTakeAttackImplementation { get; protected set; } = default!;

    public AttackBeforeFightDelegate? AttackBeforeFightImplementation { get; protected set; }

    public WillSkipRoundDelegate? WillSkipRoundImplementation { get; protected set; }

    public Random Random { get; set; } = default!;

    public virtual void ResetState()
    {
        Health = MaxHealth;
    }

    public static double CalculateNormalHitDamage(double minDmg, double maxDmg, int round, double critChance, double critMultiplier, Random random)
    {
        var baseDamage = random.NextDouble() * (1 + maxDmg - minDmg) + minDmg;
        var dmg = baseDamage * (1 + (round - 1) * (1.0 / 6.0));

        if (random.NextDouble() < critChance)
                dmg *= critMultiplier;

        return dmg;
    }
}