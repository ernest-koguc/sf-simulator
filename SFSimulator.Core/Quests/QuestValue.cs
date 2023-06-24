namespace SFSimulator.Core
{
    public class QuestValue
    {
        public decimal Gold { get; set; }
        public decimal Experience { get; set; }
        public QuestValue(decimal gold, decimal experience)
        {
            Gold = gold;
            Experience = experience;
        }
    }
}