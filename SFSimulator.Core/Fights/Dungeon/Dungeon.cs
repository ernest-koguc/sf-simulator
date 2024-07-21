namespace SFSimulator.Core
{
    public class Dungeon
    {
        public required List<DungeonEnemy> DungeonEnemies { get; set; }
        public required string Name { get; set; }
        public required int Position { get; set; }
        public bool IsUnlocked { get; set; } = false;
        public bool IsDefeated { get; set; } = false;
        public DungeonTypeEnum Type { get; set; } = DungeonTypeEnum.Default;
        public Func<DungeonUnlockContext, bool> UnlockResolve { get; set; } = (_) => true;
    }
}