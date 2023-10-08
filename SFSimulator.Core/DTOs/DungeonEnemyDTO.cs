namespace SFSimulator.Core;

public class DungeonEnemyDTO
{
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }
    public int Level { get; set; }
    public bool IsUnlocked { get; set; }
    public bool IsDefeated { get; set; }
    public ClassType Class { get; set; }
}
