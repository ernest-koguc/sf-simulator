namespace SFSimulator.Core;

public class BerserkerFightContext : DelegatableFightableContext, IRoundSkipable
{
    private int ChainAttackCounter { get; set; } = 0;
    private int ChainAttackCap { get; set; } = 14;
    private ClassType OpponentClass { get; set; }
    public BerserkerFightContext(ClassType opponentClass)
    {
        AttackImplementation = AttackImpl;
        TakeAttackImplementation = TakeAttackImpl;
        WillTakeAttackImplementation = WillTakeAttackImpl;
        OpponentClass = opponentClass;
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

        return target.TakeAttack(dmg, ref round);
    }

    private bool TakeAttackImpl(double damage, ref int round)
    {
        Health -= (long)damage;
        return Health <= 0;
    }

    private bool WillTakeAttackImpl() => true;

    public bool WillSkipRound(ref int round)
    {
        if (OpponentClass == ClassType.Mage)
        {
            return false;
        }

        if (ChainAttackCounter == ChainAttackCap)
        {
            ChainAttackCounter = 0;
        }
        else if (Random.Next(1, 101) > 50)
        {
            round++;
            ChainAttackCounter++;
            return true;
        }
        else
        {
            ChainAttackCounter = 0;
        }

        return false;
    }
}