export type Dungeon = {
  name: string,
  position: number,
  type: DungeonType,
  enemies: DungeonEnemy[] 
}

export type DungeonEnemy = {
  name: string,
  position: number,
  level: number,
  isDefeated: boolean,
  isUnlocked: boolean,
  class: number
}
export enum DungeonType
{
    Twister,
    Tower,
    LightWorld,
    ShadowWorld,
    SoloPortal,
    GuildPortal,
    GuildRaid
}
