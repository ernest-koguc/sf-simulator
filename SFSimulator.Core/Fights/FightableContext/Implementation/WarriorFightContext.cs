namespace SFSimulator.Core;

public class WarriorFightContext : DelegatableFightableContext
{
    public int BlockChance { get; set; }
    public WarriorFightContext()
    {
        AttackImplementation = AttackImpl;
        TakeAttackImplementation = TakeAttackImpl;
        WillTakeAttackImplementation = WillTakeAttackImpl;
    }

    private bool AttackImpl(IAttackTakable target, ref int round)
    {
        round++;

        if (!target.WillTakeAttack())
            return false;

        var dmg = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage, round, CritChance, CritMultiplier, Random);

        return target.TakeAttack(dmg);
    }

    private bool TakeAttackImpl(double damage)
    {
        Health -= (long)damage;
        return Health <= 0;
    }

    private bool WillTakeAttackImpl()
    {
        return Random.Next(1, 101) > BlockChance;
    }
}