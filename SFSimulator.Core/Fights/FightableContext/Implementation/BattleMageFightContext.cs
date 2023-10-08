namespace SFSimulator.Core;

public class BattleMageFightContext : DelegatableFightableContext, IBeforeFightAttackable
{
    public double FireBallDamage { get; set; }
    public BattleMageFightContext()
    {
        AttackImplementation = AttackImpl;
        TakeAttackImplementation = TakeAttackImpl;
        WillTakeAttackImplementation = WillTakeAttackImpl;
    }

    public bool AttackBeforeFight(IAttackTakable target, ref int round)
    {
        round++;
        return target.TakeAttack(FireBallDamage);
    }

    private bool AttackImpl(IAttackTakable target, ref int round)
    {
        round++;

        if (!target.WillTakeAttack())
            return false;

        var dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage, round, CritChance, CritMultiplier, Random);

        return target.TakeAttack(dmg);
    }

    private bool TakeAttackImpl (double damage)
    {
        Health -= (long)damage;
        return Health <= 0;
    }

    private bool WillTakeAttackImpl() => true;
}
