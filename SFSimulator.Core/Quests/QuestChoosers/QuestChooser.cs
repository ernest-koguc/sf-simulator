namespace SFSimulator.Core
{
    public class QuestChooser : IQuestChooser
    {
        public decimal Multiplier { get; set; } = 0.85M;
        public QuestChooser()
        {
        }
        public QuestChooser(decimal multiplier)
        {
            Multiplier = multiplier;
        }
        public Quest ChooseBestQuest(IEnumerable<Quest> quests, QuestPriorityType questPriority, QuestChooserAI questChooserAI, decimal? hybridRatio = 0)
        {
            return questChooserAI switch
            {
                QuestChooserAI.Simple => SimpleChooser(quests, questPriority, hybridRatio),
                QuestChooserAI.Smart => SmartChooser(quests, questPriority, hybridRatio),
                _ => throw new ArgumentException($"Wrong {nameof(questChooserAI)} value"),
            };
        }
        private Quest SimpleChooser(IEnumerable<Quest> quests, QuestPriorityType questPriority, decimal? hybridRatio)
        {
            return questPriority switch
            {
                QuestPriorityType.Gold => quests.OrderByDescending(q =>
                {
                    if (q.Item != null && q.Item.ItemSourceType == ItemSourceType.BeforeQuest)
                        return q.Gold + q.Item.GoldValue / q.Time;

                    return q.Gold / q.Time;

                }).First(),

                QuestPriorityType.Experience => quests.OrderByDescending(q => q.Experience / q.Time).First(),

                QuestPriorityType.Hybrid => quests.OrderByDescending(q => q.Experience / q.Time + q.Gold * hybridRatio / q.Time).First(),
                _ => throw new Exception("Wrong priority")
            };
        }

        private Quest SmartChooser(IEnumerable<Quest> quests, QuestPriorityType questPriority, decimal? hybridRatio)
        {

            if (questPriority == QuestPriorityType.Gold)
            {
                var transformedQuests = quests.Select(q =>
                {
                    var multiplier = (10 - q.Time) / 2.5M * Multiplier;
                    var estimatedGold = q.Gold + q.MinimumValues.Gold * 5 * multiplier;

                    if (q.Item != null && q.Item.ItemSourceType == ItemSourceType.BeforeQuest)
                        estimatedGold += q.Item.GoldValue;

                    return (q, estimatedGold);
                });
                var quest = transformedQuests.MaxBy(tuple => tuple.estimatedGold).q;
                return quest;
            }
            if (questPriority == QuestPriorityType.Experience)
            {
                var transformedQuests = quests.Select(q =>
                {
                    var multiplier = (10 - q.Time) / 2.5M * Multiplier;
                    var estimatedExperience = q.Experience + q.MinimumValues.Experience * 5 * multiplier;


                    return (q, estimatedExperience);
                });
                var quest = transformedQuests.MaxBy(tuple => tuple.estimatedExperience).q;

                return quest;
            }
            else
            {
                return quests.OrderByDescending(q =>
                {
                    if (q.Item != null && q.Item.ItemSourceType == ItemSourceType.BeforeQuest)
                        return q.Experience / q.Time + (q.Gold + q.Item.GoldValue) * hybridRatio / q.Time;

                    return q.Experience / q.Time + q.Gold * hybridRatio / q.Time;

                }).First();
            }
        }
    }
}
