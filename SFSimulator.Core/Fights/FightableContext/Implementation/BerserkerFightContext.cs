namespace SFSimulator.Core;

public class BerserkerFightContext : DelegatableFightableContext
{
    private int ChainAttackCounter { get; set; } = 0;
    private int ChainAttackCap { get; set; } = 15;
    public BerserkerFightContext()
    {
        AttackImplementation = AttackImpl;
        TakeAttackImplementation = TakeAttackImpl;
        WillTakeAttackImplementation = WillTakeAttackImpl;
    }

    public override void ResetState()
    {
        base.ResetState();
        ChainAttackCounter = 0;
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

    private bool WillTakeAttackImpl()
    {
        if (ChainAttackCounter == ChainAttackCap)
        {
            ChainAttackCounter = 0;
        }
        else if (Random.Next(1, 101) > 50)
        {
            ChainAttackCounter++;
            return false;
        }
        else
        {
            ChainAttackCounter = 0;
        }

        return true;
    }
}
