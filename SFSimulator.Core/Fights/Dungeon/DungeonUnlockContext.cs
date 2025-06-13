namespace SFSimulator.Core;

public record DungeonUnlockContext(int CharacterLevel, IEnumerable<Dungeon> Dungeons);