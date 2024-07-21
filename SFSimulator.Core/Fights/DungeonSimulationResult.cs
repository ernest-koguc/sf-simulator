namespace SFSimulator.Core
{
    public class DungeonSimulationResult
    {
        public bool Succeeded { get; set; }
        public long Experience { get; set; }
        public decimal Gold { get; set; }
        public int WonFights { get; set; }

        public static DungeonSimulationResult FailedResult(int wonFights) => new() { Succeeded = false, Experience = 0, Gold = 0, WonFights = wonFights };
    }
}