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
    LightWorld = -1,
    Twister = 1,
    Tower = 2,
    ShadowWorld = 4
}
