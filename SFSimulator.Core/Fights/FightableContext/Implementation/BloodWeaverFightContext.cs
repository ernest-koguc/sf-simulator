namespace SFSimulator.Core;
public class BloodWeaverFightContext : FightContextBase
{
    private int BlockChance { get; set; } = 0;
    private int BloodPactStacks { get; set; } = 0;
    public double DaggerDmgMultiplier { get; init; }
    public double ScytheDmgMultiplier { get; init; }
    public BloodWeaverFightContext(ClassType enemyClass)
    {
        if (enemyClass == ClassType.Mage)
        {
            AttackImplementation = AttackVsMageImpl;
            TakeAttackImplementation = TakeAttackVsMageImpl;
        }
        else
        {
            AttackImplementation = AttackImpl;
            TakeAttackImplementation = TakeAttackImpl;
        }

        WillTakeAttackImplementation = WillTakeAttackImpl;
    }

    public override void ResetState()
    {
        base.ResetState();
        BlockChance = 0;
        BloodPactStacks = 0;
    }

    private bool AttackImpl(IAttackTakable target, ref int round)
    {
        round++;

        var dmgMultiplier = 1D;
        if (ReviveCount > 0 && Random.Next(1, 101) > 50)
        {
            dmgMultiplier = ScytheDmgMultiplier;
        }
        else if (ReviveCount == 0 && Random.Next(1, 101) <= 30)
        {
            if (target.WillTakeAttackImplementation())
            {
                var normalHitDmg = CalculateNormalHitDamage(MinimumDamage, MaximumDamage, round, CritChance, CritMultiplier, Random);
                if (target.TakeAttackImplementation(normalHitDmg, ref round))
                    return true;
            }

            round++;

            TakeAttackImplementation(Health * 0.1D, ref round);
            dmgMultiplier = DaggerDmgMultiplier;
            BloodPactStacks = Math.Min(3, BloodPactStacks + 1);
        }

        if (!target.WillTakeAttackImplementation())
        {
            return false;
        }

        var dmg = CalculateNormalHitDamage(MinimumDamage, MaximumDamage, round, CritChance, CritMultiplier, Random) * dmgMultiplier;

        return target.TakeAttackImplementation(dmg, ref round);
    }

    private bool AttackVsMageImpl(IAttackTakable target, ref int round)
    {
        round++;

        var dmg = CalculateNormalHitDamage(MinimumDamage, MaximumDamage, round, CritChance, CritMultiplier, Random);

        return target.TakeAttackImplementation(dmg, ref round);
    }

    private bool TakeAttackImpl(double damage, ref int round)
    {
        Health -= damage;
        if (Health <= 0 && ReviveCount == 0)
        {
            ReviveCount++;
            BlockChance = 30;
            Health = MaxHealth * (0.4D + (BloodPactStacks * 0.1D));
            round++;
        }

        return Health <= 0;
    }
    private bool TakeAttackVsMageImpl(double damage, ref int round)
    {
        Health -= damage;
        return Health <= 0;
    }

    private bool WillTakeAttackImpl()
    {
        return Random.Next(1, 101) > BlockChance;
    }
}