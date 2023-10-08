namespace SFSimulator.Core
{
    public interface IDamageProvider
    {
        (double Minimum, double Maximum) CalculateDamage(Weapon? weapon, IFightable attacker, IFightable target, bool isSecondWeapon = false);
        double CalculateFireBallDamage(IFightable main, IFightable opponent);
    }
}