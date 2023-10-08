export type Dungeon = {
  name: string,
  position: number,
  type: number,
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
