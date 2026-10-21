import { Expedition, ExpeditionCrossroad, ExpeditionEncounter, ExpeditionState } from '../SFGameModels';
import { ByteParser, chunks, unsigned } from './ByteParser';

const ENCOUNTERS = new Set<number>(Object.values(ExpeditionEncounter).filter(v => typeof v === 'number'));
const encounter = (value: number): ExpeditionEncounter => ENCOUNTERS.has(value) ? value : ExpeditionEncounter.None;

export function parseExpeditions(data: number[]): Expedition[] {
  return Array.from(chunks(data, 8), record => ({
    MainTask: encounter(record[0]),
    // The current UI displays these three legacy task slots. The Rust
    // AvailableExpedition parser leaves words 1..3 uninterpreted.
    SideTasks: record.slice(1, 4),
    Time: unsigned(record[6], 0xffffffff, 600),
  }));
}

// gamestate/mod.rs: expeditionstate (current_floor, floor_stage, heroism).
export function parseExpeditionState(data: number[]): ExpeditionState {
  const parser = new ByteParser(data);
  return {
    Stage: parser.unsignedAt(0, 0xff),
    CurrentStatus: parser.at(2),
    CurrentHeroism: parser.at(13),
  };
}

// Expedition::update_encounters consumes pairs of encounter type / heroism.
// Only the encounter choices are displayed by the extension.
export function parseExpeditionCrossroad(data: number[]): ExpeditionCrossroad {
  const choices = Array.from(chunks(data, 2), pair => encounter(pair[0]));
  return {
    Left: choices[0] ?? ExpeditionEncounter.None,
    Mid: choices[1] ?? ExpeditionEncounter.None,
    Right: choices[2] ?? ExpeditionEncounter.None,
  };
}
