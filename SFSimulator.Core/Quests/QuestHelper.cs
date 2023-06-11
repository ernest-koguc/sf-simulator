namespace SFSimulator.Core
{
    public class QuestHelper : IQuestHelper
    {
        public float GetTime(int questSegments, MountType mountType)
        {
            var mountFactor = 1f;
            switch (mountType)
            {
                case MountType.None:
                    mountFactor = 1f;
                    break;
                case MountType.Pig:
                    mountFactor = 0.9f;
                    break;
                case MountType.Horse:
                    mountFactor = 0.8f;
                    break;
                case MountType.Tiger:
                    mountFactor = 0.7f;
                    break;
                case MountType.Griffin:
                    mountFactor = 0.5f;
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
