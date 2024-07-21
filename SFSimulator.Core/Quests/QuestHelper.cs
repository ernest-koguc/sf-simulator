namespace SFSimulator.Core
{
    public class QuestHelper : IQuestHelper
    {
        public decimal GetTime(int questSegments, MountType mountType)
        {
            var mountFactor = 1M;
            switch (mountType)
            {
                case MountType.None:
                    mountFactor = 1M;
                    break;
                case MountType.Pig:
                    mountFactor = 0.9M;
                    break;
                case MountType.Horse:
                    mountFactor = 0.8M;
                    break;
                case MountType.Tiger:
                    mountFactor = 0.7M;
                    break;
                case MountType.Griffin:
                    mountFactor = 0.5M;
                    break;
            }


            return questSegments switch
            {
                1 => 5 * mountFactor,
                2 => 10 * mountFactor,
                3 => 15 * mountFactor,
                4 => 20 * mountFactor,
                _ => throw new ArgumentOutOfRangeException(nameof(questSegments)),
            };
        }
    }
}