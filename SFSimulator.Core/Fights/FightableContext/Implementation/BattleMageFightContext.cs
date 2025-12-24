namespace SFSimulator.Core;

public class BattleMageFightContext : FightContextBase
{
    public double FireBallDamage { get; set; }
    public BattleMageFightContext()
    {
        AttackImplementation = AttackImpl;
        TakeAttackImplementation = TakeAttackImpl;
        WillTakeAttackImplementation = WillTakeAttackImpl;
        AttackBeforeFightImplementation = AttackBeforeFight;
    }

    public bool AttackBeforeFight(IAttackTakable target, ref int round)
    {
        round++;
        return target.TakeAttackImplementation(FireBallDamage, ref round);
    }

    private bool AttackImpl(IAttackTakable target, ref int round)
    {
        round++;

        if (!target.WillTakeAttackImplementation())
            return false;

        var dmg = CalculateNormalHitDamage(MinimumDamage, MaximumDamage, round, CritChance, CritMultiplier, Random);

        return target.TakeAttackImplementation(dmg, ref round);
    }

    private bool TakeAttackImpl(double damage, ref int round)
    {
        Health -= damage;
        return Health <= 0;
    }

    private bool WillTakeAttackImpl() => true;
}