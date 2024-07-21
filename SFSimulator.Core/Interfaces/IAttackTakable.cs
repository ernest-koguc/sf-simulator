namespace SFSimulator.Core
{
    public interface IAttackTakable
    {
        bool TakeAttack(double damage);
        bool WillTakeAttack();
    }
}