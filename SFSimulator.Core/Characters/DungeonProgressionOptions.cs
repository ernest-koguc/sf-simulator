namespace SFSimulator.Core;

public class DungeonProgressionOptions
{
    public double InstaKillPercentage { get; set; }
    public int DungeonIterations { get; set; }
    public bool DoDungeonPause { get; set; }
    public int? DungeonPauseStartLevel { get; set; }
    public int? DungeonPauseEndLevel { get; set; }
}