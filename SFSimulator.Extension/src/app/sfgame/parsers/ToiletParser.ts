import { ToiletState } from "../SFGameModels";
import { ByteParser } from "./ByteParser";

export function parseToiletState(data: number[]): ToiletState {
  const parser = new ByteParser(data);

  return {
    AmountOfItemsToSacrifice: parser.atLong(2)
  }
}
