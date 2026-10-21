import { Companion, Tower } from '../SFGameModels';
import { ByteParser } from './ByteParser';
import { parseAttributes } from './OwnPlayerSaveParser';

export function parseTower(data: number[]): Tower {
  const parser = new ByteParser(data);
  const hasUnderworld = parser.unsignedAt(448, 0xff) > 0;
  const companion = (index: number): Companion => {
    // gamestate/mod.rs: owntower. Five attributes at companion start + 4.
    const start = 3 + index * 148;
    return {
      Level: parser.at(start),
      ...parseAttributes(parser, start + 4),
    };
  };
  return {
    Underworld: {
      // Underworld::update and UnderworldBuildingType::GladiatorTrainer.
      Gladiator: hasUnderworld ? parser.unsignedAt(454, 0xff) : 0,
      TimeMachineThirst: hasUnderworld ? parser.unsignedAt(473) : 0,
    },
    Companions: {
      Bert: companion(0),
      Mark: companion(1),
      Kunigunde: companion(2),
    },
  };
}
