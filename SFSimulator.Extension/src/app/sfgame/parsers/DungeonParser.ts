import { Dungeons } from '../SFGameModels';

export function parseDungeons(data: { light?: number[]; shadow?: number[], class?: number[] }): Dungeons {
  return {
    Light: data.light ?? [],
    Shadow: data.shadow ?? [],
    Class: data.class ?? []
  };
}
