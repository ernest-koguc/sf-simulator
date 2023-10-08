namespace SFSimulator.Core;

public class AssassinFightContext : DelegatableFightableContext
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

        if (target.WillTakeAttack())
        {
            var firstWeaponDamage = DungeonableDefaultImplementation.CalculateNormalHitDamage(MinimumDamage, MaximumDamage, round, CritChance, CritMultiplier, Random); 
            if (target.TakeAttack(firstWeaponDamage))
                return true;
        }

        round++;
        
        if (!target.WillTakeAttack())
            return false;

        var secondWeaponDamage = DungeonableDefaultImplementation.CalculateNormalHitDamage(SecondMinimumDamage, SecondMaximumDamage, round, CritChance, CritMultiplier, Random);

        return target.TakeAttack(secondWeaponDamage);
    }

    private bool TakeAttackImpl (double damage)
    {
        Health -= (long)damage;
        return Health <= 0;
    }

    private bool WillTakeAttackImpl()
    {
        return Random.Next(1, 101) > 50;
    }
}
