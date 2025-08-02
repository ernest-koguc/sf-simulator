export function parseDungeons(data: { light: number[], shadow: number[] }) {
  const open = -1;
  const locked = -2;

  const dungeons = {
    Light: [] as { Position: number, Current: number }[],
    Shadow: [] as { Position: number, Current: number }[],
    GuildPortal: locked,
    SoloPortal: locked,
    Tower: open,
    Twister: open,
    GuildRaid: locked,
    LoopOfIdols: locked,
    Sandstorm: locked
  }

  const light = data.light.filter((_, i) => i !== 14 && i !== 17 && i !== 31).map((value, index) => {
    return { Position: mapDungeonIndex(index, false), Current: value };
  });
  const shadow = data.shadow.filter((_, i) => i !== 14 && i !== 17 && i !== 31).map((value, index) => {
    return { Position: mapDungeonIndex(index, true), Current: value };
  });


  dungeons.Tower = data.light[14];
  dungeons.Twister = data.shadow[14];
  dungeons.SoloPortal = data.light[17];
  dungeons.LoopOfIdols = data.shadow[17];
  dungeons.Sandstorm = data.light[31];
  dungeons.Light = light;
  dungeons.Shadow = shadow;

  return dungeons;
}

export const DungeonMapping: Record<number, number> = {
  13: 15,
  14: 19,
  15: 17,
  16: 18,
  17: 24,
  18: 27,
  19: 21,
  20: 13,
  21: 14,
  22: 16,
  23: 20,
  24: 28,
  25: 22,
  26: 23,
  27: 25,
  28: 26,
  30: 29,
  31: 30,
  32: 31,
  33: 32,
  34: 33,

}
export function mapDungeonIndex(arrayIndex: number, isShadow: boolean) {
  let index = arrayIndex + 1;

  if (DungeonMapping[index]) {
    return isShadow ? DungeonMapping[index] + 100 : DungeonMapping[index];
  }

  return isShadow ? index + 100 : index;
}
