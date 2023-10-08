namespace SFSimulator.Core
{
    public interface IAttackable
    {
        bool Attack(IAttackTakable target, ref int round);
    }
}
