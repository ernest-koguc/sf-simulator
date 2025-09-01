import { Pets } from "../SFGameModels";
import { ByteParser } from "./ByteParser";

export function parsePets(petsData: number[]): Pets {
  const dataType = new ByteParser(petsData);
  dataType.skip(2);

  const petLevels = dataType.sub(100);

  let shadowCount = 0;
  let lightCount = 0;
  let earthCount = 0;
  let fireCount = 0;
  let waterCount = 0;
  let shadowLevel = 0;
  let lightLevel = 0;
  let earthLevel = 0;
  let fireLevel = 0;
  let waterLevel = 0;

  if (petLevels.length) {
    for (let i = 0; i < 20; i++) {
      shadowCount += petLevels[i] > 0 ? 1 : 0;
      shadowLevel += petLevels[i];
    }

    for (let i = 0; i < 20; i++) {
      lightCount += petLevels[i + 20] > 0 ? 1 : 0;
      lightLevel += petLevels[i + 20];
    }

    for (let i = 0; i < 20; i++) {
      earthCount += petLevels[i + 40] > 0 ? 1 : 0;
      earthLevel += petLevels[i + 40];
    }

    for (let i = 0; i < 20; i++) {
      fireCount += petLevels[i + 60] > 0 ? 1 : 0;
      fireLevel += petLevels[i + 60];
    }

    for (let i = 0; i < 20; i++) {
      waterCount += petLevels[i + 80] > 0 ? 1 : 0;
      waterLevel += petLevels[i + 80];
    }
  }

  dataType.skip(1);
  const totalCount = dataType.long();
  const shadow = dataType.long();
  const light = dataType.long();
  const earth = dataType.long();
  const fire = dataType.long();
  const water = dataType.long();


  dataType.skip(101);
  const petDungeons = dataType.sub(5);
  dataType.skip(18);
  const petRank = dataType.long();
  const petHonor = dataType.long();
  dataType.skip(29);
  const petTotalLevel = shadowLevel + lightLevel + fireLevel + earthLevel + waterLevel;
  return {
    Levels: petLevels,
    ShadowLevels: petLevels.slice(0, 20),
    LightLevels: petLevels.slice(20, 40),
    EarthLevels: petLevels.slice(40, 60),
    FireLevels: petLevels.slice(60, 80),
    WaterLevels: petLevels.slice(80, 100),
    ShadowCount: shadowCount,
    LightCount: lightCount,
    EarthCount: earthCount,
    FireCount: fireCount,
    WaterCount: waterCount,
    ShadowLevel: shadowLevel,
    LightLevel: lightLevel,
    EarthLevel: earthLevel,
    FireLevel: fireLevel,
    WaterLevel: waterLevel,
    TotalCount: totalCount,
    Shadow: shadow,
    Light: light,
    Earth: earth,
    Fire: fire,
    Water: water,
    Rank: petRank,
    Honor: petHonor,
    TotalLevel: petTotalLevel,
    Dungeons: petDungeons,
    ShadowArenaFought: dataType.atLong(223) === 1,
    LightArenaFought: dataType.atLong(224) === 1,
    EarthArenaFought: dataType.atLong(225) === 1,
    FireArenaFought: dataType.atLong(226) === 1,
    WaterArenaFought: dataType.atLong(227) === 1,
  };
}
