
using static SFSimulator.Core.IFightableContext;

namespace SFSimulator.Core
{
    public interface IAttackable
    {
        AttackDelegate AttackImplementation { get; }
    }
}