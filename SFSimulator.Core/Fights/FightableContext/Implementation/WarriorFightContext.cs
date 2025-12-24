namespace SFSimulator.Core;

public class WarriorFightContext : FightContextBase
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

    private bool WillTakeAttackImpl()
    {
        return Random.Next(1, 101) > BlockChance;
    }
}