import { Dungeons } from '../SFGameModels';

export function parseDungeons(
  data: { light?: number[]; shadow?: number[]; class?: number[] },
  previous?: Dungeons | null,
): Dungeons {
  return {
    Light: updateProgress(data.light, previous?.Light),
    Shadow: updateProgress(data.shadow, previous?.Shadow),
    Class: updateProgress(data.class, previous?.Class),
  };
}

function updateProgress(progress: number[] | undefined, previous: number[] = []): number[] {
  return progress?.length ? [...progress, ...previous.slice(progress.length)] : previous;
}
