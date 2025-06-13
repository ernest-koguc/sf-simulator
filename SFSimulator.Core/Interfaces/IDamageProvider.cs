namespace SFSimulator.Core
{
    public interface IDamageProvider
    {
        (double Minimum, double Maximum) CalculateDamage<T, E>(IWeaponable? weapon, IFightable<T> attacker, IFightable<E> target, bool isSecondWeapon = false) where T : IWeaponable where E : IWeaponable;
        double CalculateFireBallDamage<T, E>(IFightable<T> main, IFightable<E> opponent) where T : IWeaponable where E : IWeaponable;
    }
}