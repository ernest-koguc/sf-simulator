namespace SFSimulator.Core;

public class DungeonDTO
{
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }
    public DungeonType Type { get; set; }
    public List<DungeonEnemyDTO> Enemies { get; set; } = null!;
}
