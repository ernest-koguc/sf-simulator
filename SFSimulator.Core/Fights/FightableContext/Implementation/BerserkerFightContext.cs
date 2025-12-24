namespace SFSimulator.Core;

public class BerserkerFightContext : FightContextBase
{
    private int ChainAttackCounter { get; set; } = 0;
    private int ChainAttackCap { get; set; } = 14;
    private ClassType OpponentClass { get; set; }

    public BerserkerFightContext(ClassType opponentClass)
    {
        AttackImplementation = AttackImpl;
        TakeAttackImplementation = TakeAttackImpl;
        WillTakeAttackImplementation = WillTakeAttackImpl;
        WillSkipRoundImplementation = WillSkipRound;
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
            // NOTE: currently there is a bug that makes berserker chain attack not increase enrage round
            //round++;
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