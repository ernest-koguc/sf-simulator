namespace SFSimulator.Core;

public class DemonHunterFightContext : FightContextBase
{
    private int ReviveCount { get; set; } = 0;
    public DemonHunterFightContext(ClassType enemyClass)
    {
        AttackImplementation = AttackImpl;

        if (enemyClass == ClassType.Mage)
            TakeAttackImplementation = NoReviveTakeAttackImpl;
        else
            TakeAttackImplementation = TakeAttackImpl;

        WillTakeAttackImplementation = WillTakeAttackImpl;
    }


    public override void ResetState()
    {
        base.ResetState();
        ReviveCount = 0;
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

        if (Health <= 0)
        {
            Revive(ref round);
        }

        return Health <= 0;
    }

    private bool NoReviveTakeAttackImpl(double damage, ref int round)
    {
        Health -= damage;
        return Health <= 0;
    }

    private bool WillTakeAttackImpl() => true;

    private void Revive(ref int round)
    {
        var currentReviveChance = 0.44 - ReviveCount * 0.11;
        if (currentReviveChance <= 0 || Random.NextDouble() >= currentReviveChance) {
            return;
        }

        Health = MaxHealth * (0.9 - ReviveCount * 0.1);

        round++;
        ReviveCount++;
    }
}