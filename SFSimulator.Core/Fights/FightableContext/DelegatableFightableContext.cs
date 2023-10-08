namespace SFSimulator.Core;

public abstract class DelegatableFightableContext : IFightableContext
{
    public long MaxHealth { get; set; }
    public long Health { get; set; }
    public double MinimumDamage { get; set; }
    public double MaximumDamage { get; set; }
    public double CritChance { get; set; }
    public double CritMultiplier { get; set; }

    public delegate bool AttackDelegate(IAttackTakable target, ref int round);
    public delegate bool TakeAttackDelegate(double damage);
    public delegate bool WillTakeAttackDelegate();
    protected AttackDelegate AttackImplementation { get; set; } = default!;
    protected TakeAttackDelegate TakeAttackImplementation { get; set; } = default!; 
    protected WillTakeAttackDelegate WillTakeAttackImplementation { get; set; } = default!;
    public Random Random { get; set; } = default!;

    public bool Attack(IAttackTakable target, ref int round)
    {
        return AttackImplementation(target, ref round);
    }

    public bool TakeAttack(double damage)
    {
        return TakeAttackImplementation(damage);
    }

    public bool WillTakeAttack()
    {
        return WillTakeAttackImplementation();
    }
    public virtual void ResetState()
    {
        Health = MaxHealth;
    }
}
