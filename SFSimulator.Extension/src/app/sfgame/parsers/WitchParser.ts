import { _between } from "../sfgame-parser";
import { Witch } from "../SFGameModels";
import { ByteParser } from "./ByteParser";

export function parseWitch(witchData: number[]): Witch {
  const dataType = new ByteParser(witchData);
  let witchStage = dataType.long();
  let witchItems = dataType.long();
  const witchItemsNext = Math.max(0, dataType.long());
  const witchTodayItemForCauldron = dataType.long();
  witchItems = Math.min(witchItems, witchItemsNext);

  dataType.skip(2);

  const witchFinish = dataType.long() * 1000; //+ data.offset;
  dataType.skip(1);

  const witchScrolls = [];
  for (let i = 0; i < 9; i++) {
    dataType.skip(1);

    const type = dataType.long();
    const date = dataType.long() * 1000;// + data.offset;

    witchScrolls.push({
      Date: date,
      Type: type,
      Unlocked: _between(date, 0, (new Date()).getTime())
    });
  }

  witchStage = witchScrolls.filter(s => s.Unlocked).length;
  return {
    Stage: witchStage,
    Items: witchItems,
    ItemsNext: witchItemsNext,
    ItemForCauldron: witchTodayItemForCauldron,
    Finish: witchFinish,
    Scrolls: witchScrolls
  };
}
