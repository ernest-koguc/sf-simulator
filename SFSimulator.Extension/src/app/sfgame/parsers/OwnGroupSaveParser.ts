import { Guild } from "../SFGameModels";
import { ByteParser } from "./ByteParser";

export function parseOwnGroupSave(data: number[]): Guild {
  const byteParser = new ByteParser(data);

  const gid = byteParser.long();
  byteParser.skip(5);
  const totalTreasure = byteParser.short();
  const portalLife = byteParser.short();

  const totalInstructor = byteParser.short();
  const portal = byteParser.short();

  const honor = byteParser.atLong(13);
  const raids = byteParser.atLong(8);
  const petId = byteParser.atLong(377);
  const petMaxLevel = byteParser.atLong(378);

  //const resetTime = byteParser.getDate(2);
  //const lastPortalFight = byteParser.getDate(178);//byteParser.getDate(165);

  const groupSave = {
    Id: gid,
    TotalTreasure: totalTreasure,
    PortalLife: portalLife,
    TotalInstructor: totalInstructor,
    Portal: portal,
    Honor: honor,
    Raids: raids,
    PetId: petId,
    PetMaxLevel: petMaxLevel,
  }

  return groupSave;
}
