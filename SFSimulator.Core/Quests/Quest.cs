using QuestSimulator.Items;

namespace QuestSimulator.Quests
{
    public class Quest
    {
        public double Gold { get; set; }
        public double Experience { get; set; }
        public double Time { get; set; }
        public QuestValue MinimumValues { get; private set; }
        public Item? Item { get; set; }



        public Quest(QuestValue minimumQuestValue, double gold, double experience, double time, Item? item = null)
        {
            MinimumValues = minimumQuestValue ?? throw new ArgumentNullException(nameof(minimumQuestValue));
            Gold = gold;
            Experience = experience;
            Time = time;
            Item = item;
        }
    }
}
