namespace SFSimulator.Core
{
    public class QuestChooser : IQuestChooser
    {
        public decimal Multiplier { get; set; } = 0.85M;

        public QuestOptions QuestOptions { get; set; } = new QuestOptions(QuestPriorityType.Gold, null);

        public Quest ChooseBestQuest(IEnumerable<Quest> quests)
        {
            switch (QuestOptions.Priority)
            {
                case QuestPriorityType.Gold:
                    var transformedQuests = quests.Select(q =>
                    {
                        var multiplier = (10 - q.Time) / 2.5M * Multiplier;
                        var estimatedGold = q.Gold + q.MinimumValues.Gold * 5 * multiplier;

                        if (q.Item != null && q.Item.ItemSourceType == ItemSourceType.BeforeQuest)
                            estimatedGold += q.Item.GoldValue;

                        return (q, estimatedGold);
                    });
                    return transformedQuests.MaxBy(tuple => tuple.estimatedGold).q;
                case QuestPriorityType.Experience:
                    var transformedQuests2 = quests.Select(q =>
                    {
                        var multiplier = (10 - q.Time) / 2.5M * Multiplier;
                        var estimatedExperience = q.Experience + q.MinimumValues.Experience * 5 * multiplier;


                        return (q, estimatedExperience);
                    });
                    return transformedQuests2.MaxBy(tuple => tuple.estimatedExperience).q;
                default:
                    return quests.OrderByDescending(q =>
                    {
                        if (q.Item != null && q.Item.ItemSourceType == ItemSourceType.BeforeQuest)
                            return q.Experience / q.Time + (q.Gold + q.Item.GoldValue) * (QuestOptions.HybridRatio ?? 0) / q.Time;

                        return q.Experience / q.Time + q.Gold * (QuestOptions.HybridRatio ?? 0) / q.Time;

                    }).First();
            }
        }
    }
}
