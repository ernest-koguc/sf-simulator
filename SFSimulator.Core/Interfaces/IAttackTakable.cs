using static SFSimulator.Core.IFightableContext;

namespace SFSimulator.Core;

public interface IAttackTakable
{
    TakeAttackDelegate TakeAttackImplementation { get; }
    WillTakeAttackDelegate WillTakeAttackImplementation { get; }
}