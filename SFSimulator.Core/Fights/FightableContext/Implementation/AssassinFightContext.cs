namespace SFSimulator.Core;

public class AssassinFightContext : FightContextBase
{
    public double SecondMinimumDamage { get; set; }
    public double SecondMaximumDamage { get; set; }
    public AssassinFightContext()
    {
        AttackImplementation = AttackImpl;
        TakeAttackImplementation = TakeAttackImpl;
        WillTakeAttackImplementation = WillTakeAttackImpl;
    }

    private bool AttackImpl(IAttackTakable target, ref int round)
    {
        round++;

        if (target.WillTakeAttackImplementation())
        {
            var firstWeaponDamage = CalculateNormalHitDamage(MinimumDamage, MaximumDamage,
                round, CritChance, CritMultiplier, Random);
            if (target.TakeAttackImplementation(firstWeaponDamage, ref round))
            {
                return true;
            }
        }

        round++;

        if (!target.WillTakeAttackImplementation())
        {
            return false;
        }

        var secondWeaponDamage = CalculateNormalHitDamage(SecondMinimumDamage, SecondMaximumDamage,
            round, CritChance, CritMultiplier, Random);

        return target.TakeAttackImplementation(secondWeaponDamage, ref round);
    }

    private bool TakeAttackImpl(double damage, ref int round)
    {
        Health -= damage;
        return Health <= 0;
    }

    private bool WillTakeAttackImpl()
    {
        return Random.Next(1, 101) > 50;
    }
}