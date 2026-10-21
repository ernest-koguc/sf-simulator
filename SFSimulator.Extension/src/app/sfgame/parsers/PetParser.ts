import { Pets } from '../SFGameModels';
import { ByteParser } from './ByteParser';

// Pets::update: habitat order is Shadow, Light, Earth, Fire, Water.
export function parsePets(data: number[]): Pets {
  const parser = new ByteParser(data);
  return {
    ShadowArenaFought: parser.at(223) === 1,
    LightArenaFought: parser.at(224) === 1,
    EarthArenaFought: parser.at(225) === 1,
    FireArenaFought: parser.at(226) === 1,
    WaterArenaFought: parser.at(227) === 1,
  };
}
