namespace SFSimulator.Core
{
    public class QuestChooser : IQuestChooser
    {
        public double Multiplier { get; set; } = 0.5d;
        public QuestChooser()
        {
        }
        public QuestChooser(double multiplier)
        {
            Multiplier = multiplier;
        }
        public Quest ChooseBestQuest(IEnumerable<Quest> quests, Priority questPriority, QuestChooserAI questChooserAI, float? hybridRatio = 0f)
        {
            return questChooserAI switch
            {
                QuestChooserAI.SIMPLE => SimpleChooser(quests, questPriority, hybridRatio),
                QuestChooserAI.SMART => SmartChooser(quests, questPriority, hybridRatio),
                _ => throw new ArgumentException($"Wrong {nameof(questChooserAI)} value"),
            };
        }
        private Quest SimpleChooser(IEnumerable<Quest> quests, Priority questPriority, float? hybridRatio)
        {
            return questPriority switch
            {
                Priority.GOLD => quests.OrderByDescending(q =>
                {
                    if (q.Item != null && q.Item.ItemSourceType == ItemSourceType.BeforeQuest)
                        return q.Gold + q.Item.GoldValue / q.Time;

                    return q.Gold / q.Time;

                }).First(),

                Priority.XP => quests.OrderByDescending(q => q.Experience / q.Time).First(),

                Priority.HYBRID => quests.OrderByDescending(q => q.Experience / q.Time + q.Gold * hybridRatio / q.Time).First(),
                _ => throw new Exception("Wrong priority")
            };
        }

        private Quest SmartChooser(IEnumerable<Quest> quests, Priority questPriority, float? hybridRatio)
        {

            if (questPriority == Priority.GOLD)
            {
                var transformedQuests = quests.Select(q =>
                {
                    var multiplier = (10 - q.Time) / 2.5 * Multiplier;
                    var estimatedGold = q.Gold + q.MinimumValues.Gold * 5 * multiplier;

                    if (q.Item != null && q.Item.ItemSourceType == ItemSourceType.BeforeQuest)
                        estimatedGold += q.Item.GoldValue;

                    return (q, estimatedGold);
                });
                var quest = transformedQuests.MaxBy(tuple => tuple.estimatedGold).q;
                return quest;
            }
            if (questPriority == Priority.XP)
            {
                var transformedQuests = quests.Select(q =>
                {
                    var multiplier = (10 - q.Time) / 2.5 * Multiplier;
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
