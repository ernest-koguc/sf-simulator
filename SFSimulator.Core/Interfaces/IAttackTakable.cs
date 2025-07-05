namespace SFSimulator.Core;

public interface IAttackTakable
{
    bool TakeAttack(double damage, ref int round);
    bool WillTakeAttack();
}