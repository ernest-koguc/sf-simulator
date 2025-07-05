namespace SFSimulator.Core;
public interface IRoundSkipable
{
    bool WillSkipRound(ref int round);
}
