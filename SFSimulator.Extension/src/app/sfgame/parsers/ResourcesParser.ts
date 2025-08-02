import { ByteParser } from "./ByteParser";

export function parseResources(data: number[]) {
  const resourcesData = new ByteParser(data);
  resourcesData.skip(1);

  return {
    Mushrooms: resourcesData.long(),
    Gold: resourcesData.long(),
    LuckyCoins: resourcesData.long(),
    Hourglass: resourcesData.long(),
    Wood: resourcesData.long(),
    SecretWood: resourcesData.long(),
    Stone: resourcesData.long(),
    SecretStone: resourcesData.long(),
    Metal: resourcesData.long(),
    Crystals: resourcesData.long(),
    Souls: resourcesData.long(),
    ShadowFood: resourcesData.long(),
    LightFood: resourcesData.long(),
    EarthFood: resourcesData.long(),
    FireFood: resourcesData.long(),
    WaterFood: resourcesData.long()
  }
}

