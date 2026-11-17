import { Pets } from '../SFGameModels';
import { ByteParser } from './ByteParser';

// Pets::update: habitat order is Shadow, Light, Earth, Fire, Water.
export function parsePets(data: number[]): Pets {
  const parser = new ByteParser(data);
  return {
    Shadow: parser.unsignedAt(104),
    Light: parser.unsignedAt(105),
    Earth: parser.unsignedAt(106),
    Fire: parser.unsignedAt(107),
    Water: parser.unsignedAt(108),
    ShadowArenaFought: parser.at(223) === 1,
    LightArenaFought: parser.at(224) === 1,
    EarthArenaFought: parser.at(225) === 1,
    FireArenaFought: parser.at(226) === 1,
    WaterArenaFought: parser.at(227) === 1,
  };
}
