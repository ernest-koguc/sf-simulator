namespace SFSimulator.Core;

public class DemonHunterFightContext : DelegatableFightableContext
{
    private double ReviveChance { get; set; } = 0.44;
    private double HpReviveModifier { get; set; } = 0.9;
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
        ReviveChance = 0.44;
        HpReviveModifier = 0.9;
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

        if (Health <= 0)
        {
            Revive(ref round);
        }

        return Health <= 0;
    }

    private bool NoReviveTakeAttackImpl(double damage, ref int round)
    {
        Health -= (long)damage;
        return Health <= 0;
    }

    private bool WillTakeAttackImpl() => true;

    private void Revive(ref int round)
    {
        if (ReviveChance > 0 && Random.NextDouble() >= ReviveChance)
        {
            return;
        }

        round++;

        Health = (long)(MaxHealth * HpReviveModifier);
        ReviveChance -= 0.11;
        HpReviveModifier -= 0.1;
    }
}