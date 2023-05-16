namespace SFSimulator.Core
{
    public class QuestValue
    {
        public double Gold { get; set; }
        public double Experience { get; set; }
        public QuestValue(double gold, int experience)
        {
            Gold = gold;
            Experience = experience;
        }
    }
}