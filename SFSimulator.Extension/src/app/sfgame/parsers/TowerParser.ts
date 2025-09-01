import { Tower } from "../SFGameModels";
import { ByteParser } from "./ByteParser";
import { getAttributes } from "./OwnPlayerSaveParser";

export function parseTower(towerData: number[]): Tower {
  let dataType = new ByteParser(towerData.slice(448));
  const heart = dataType.long();
  const gate = dataType.long();
  const goldPit = dataType.long();
  const extractor = dataType.long();
  const goblinPit = dataType.long();
  const torture = dataType.long();
  const gladiator = dataType.long();
  const trollBlock = dataType.long();
  const timeMachine = dataType.long();
  const keeper = dataType.long();
  dataType.skip(1);
  const extractorSouls = dataType.long();
  const extractorMax = dataType.long();
  const maxSouls = dataType.long();
  dataType.skip(1);
  const extractorHourly = dataType.long();
  const goldPitGold = dataType.long() / 100;
  const goldPitMax = dataType.long() / 100;
  const goldPitHourly = dataType.long() / 100;
  dataType.skip(6);
  const timeMachineThirst = dataType.long();
  const timeMachineMax = dataType.long();
  const timeMachineDaily = dataType.long();
  const underworld = {
    Heart: heart,
    Gate: gate,
    GoldPit: goldPit,
    Extractor: extractor,
    GoblinPit: goblinPit,
    Torture: torture,
    Gladiator: gladiator,
    TrollBlock: trollBlock,
    TimeMachine: timeMachine,
    Keeper: keeper,
    ExtractorSouls: extractorSouls,
    ExtractorMax: extractorMax,
    MaxSouls: maxSouls,
    ExtractorHourly: extractorHourly,
    GoldPitGold: goldPitGold,
    GoldPitMax: goldPitMax,
    GoldPitHourly: goldPitHourly,
    TimeMachineThirst: timeMachineThirst,
    TimeMachineMax: timeMachineMax,
    TimeMachineDaily: timeMachineDaily
  }

  dataType = new ByteParser(towerData);

  dataType.skip(3);
  const bert = parseCompanion(dataType);
  dataType.skip(120);

  dataType.skip(6);
  const mark = parseCompanion(dataType);
  dataType.skip(120);

  dataType.skip(6);
  const kuni = parseCompanion(dataType);
  dataType.skip(120);

  return {
    Underworld: underworld,
    Companions: {
      Bert: bert,
      Mark: mark,
      Kunigunde: kuni,
    }
  }
}

function parseCompanion(companionData: ByteParser) {

  companionData.skip(4)

  const attributes = getAttributes(companionData);

  const data = {
    Armor: companionData.long(),
    Damage: {
      Min: companionData.long(),
      Max: companionData.long()
    },
    Strength: attributes.Strength,
    Dexterity: attributes.Dexterity,
    Intelligence: attributes.Intelligence,
    Constitution: attributes.Constitution,
    Luck: attributes.Luck,
  }

  return data;
}

