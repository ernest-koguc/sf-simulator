namespace SFSimulator.Core;

public class DruidFightContext : DelegatableFightableContext
{
    private double EvadeChance { get; set; } = 0.35D;
    private bool IsInBearForm { get; set; } = false;
    public double RageCritChance { get; set; }
    private double RageCritMultiplierBonus { get; set; } = 3.6D;
    private double SwoopChance { get; set; } = 0.25;
    private double SwoopDamageModifier { get; set; } = 3.325D;
    
    public DruidFightContext()
    {
        AttackImplementation = AttackImpl;
        TakeAttackImplementation = TakeAttackImpl;
        WillTakeAttackImplementation = WillTakeAttackImpl;
    }
    public override void ResetState()
    {
        base.ResetState();
        SwoopChance = 0.25;
        IsInBearForm = false;
        AttackImplementation = AttackImpl;
    }

    private bool AttackInBearForm(IAttackTakable target, ref int round)
    {
        round++;

        try
        {
            if (!target.WillTakeAttack())
            {
                return false; ;
            }

            var critMultiplier = CritMultiplier + RageCritMultiplierBonus;
            var dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage, round, RageCritChance, critMultiplier, Random);

            return target.TakeAttack(dmg); 
        }
        finally
        {
            AttackImplementation = AttackImpl;
        }
    }

    private bool AttackImpl(IAttackTakable target, ref int round)
    {
        round++;

        var willSwoop = Random.NextDouble() < SwoopChance;

        if (willSwoop)
        {
            SwoopChance = Math.Min(0.5, SwoopChance + 0.05);
        }

        if (!target.WillTakeAttack())
            return false;

        double dmg;
        if (willSwoop)
        {
            dmg = DungeonableDefaultImplementation.CalculateNormalHitNoCritDamage(MinimumDamage, MaximumDamage, round, Random) * SwoopDamageModifier;
        }
        else
        {
            dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage, round, CritChance, CritMultiplier, Random);
        }


        return target.TakeAttack(dmg);
    }

    private bool TakeAttackImpl (double damage)
    {
        IsInBearForm = false;
        Health -= (long)damage;
        return Health <= 0;
    }

    private bool WillTakeAttackImpl()
    {
        if (!IsInBearForm && Random.NextDouble() < EvadeChance)
        {
            IsInBearForm = true;
            AttackImplementation = AttackInBearForm;
            return false;
        }

        return true;
    }
}
