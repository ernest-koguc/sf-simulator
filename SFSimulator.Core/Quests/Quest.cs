namespace SFSimulator.Core
{
    public class Quest
    {
        public decimal Gold { get; set; }
        public decimal Experience { get; set; }
        public decimal Time { get; set; }
        public QuestValue MinimumValues { get; private set; }
        public Item? Item { get; set; }



        public Quest(QuestValue minimumQuestValue, decimal gold, decimal experience, decimal time, Item? item = null)
        {
            MinimumValues = minimumQuestValue ?? throw new ArgumentNullException(nameof(minimumQuestValue));
            Gold = gold;
            Experience = experience;
            Time = time;
            Item = item;
        }
    }
}