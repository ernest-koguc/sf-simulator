namespace SFSimulator.Core;

public class FightStartingBehaviorProvider : IFightStartingBehaviorProvider
{
    public StartingBehaviorEnum GetStartingBehavior(IFightable main, IFightable opponent)
    {
        if (main.Reaction == opponent.Reaction)
        {
            return StartingBehaviorEnum.Random;
        }

        return main.Reaction > opponent.Reaction ? StartingBehaviorEnum.Character : StartingBehaviorEnum.Opponent;
    }
}
