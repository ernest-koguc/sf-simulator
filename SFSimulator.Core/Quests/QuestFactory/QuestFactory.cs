namespace SFSimulator.Core;

public class QuestFactory : IQuestFactory
{
    private readonly IQuestHelper _questHelper;
    private readonly Random _random;
    public QuestFactory(IQuestHelper questHelper, IItemGenerator itemGenerator, Random random)
    {
        _questHelper = questHelper;
        _random = random;
    }

    public Quest CreateTimeMachineQuest(QuestValue minimumQuestValue, decimal thirst, MountType mountType = MountType.Griffin)
    {
        var time = _random.Next(1, 5);
        var convertedTime = _questHelper.GetTime(time, mountType);
        var randomMultiplier = (decimal)(_random.NextDouble() + _random.Next(1, 5));
        var gold = randomMultiplier * minimumQuestValue.Gold * time;

        var experience = (6 - randomMultiplier) * minimumQuestValue.Experience * time;

        gold = gold < 0 ? 0 : gold;
        experience = experience < 0 ? 0 : experience;
        gold = gold > 16500000 ? 16500000 : gold;

        //Truncate too long quests time and also their xp and gold
        if (convertedTime > thirst)
        {
            gold *= thirst / convertedTime;
            experience *= thirst / convertedTime;
            convertedTime = thirst;
        }

        return new Quest(minimumQuestValue, gold, experience, convertedTime);
    }
}