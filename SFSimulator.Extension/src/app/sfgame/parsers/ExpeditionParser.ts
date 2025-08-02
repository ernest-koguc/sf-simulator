import { Expedition, ExpeditionCrossroad, ExpeditionEncounter, ExpeditionHalfTime, ExpeditionItemReward, ExpeditionRewardResources, ExpeditionState, ResourceType } from "../SFGameModels";

export function parseExpeditions(expeditionData: number[]): Expedition[] {
  const firstExpedition = {
    MainTask: expeditionData[0],
    SideTasks: [expeditionData[1], expeditionData[2], expeditionData[3]],
    Locations: [expeditionData[4], expeditionData[5]],
    Time: expeditionData[6],
    Flag: expeditionData[7], // 7 - this is 0 - nothing, 1 - pet egg, 2 - location for the daily task
  };

  const secondExpedition = {
    MainTask: expeditionData[8],
    SideTasks: [expeditionData[9], expeditionData[10], expeditionData[11]],
    Locations: [expeditionData[12], expeditionData[13]], // 12 and 13 is the location ids
    Time: expeditionData[14],
    Flag: expeditionData[15], // 15 - this is 0 - nothing, 1 - pet egg, 2 - location for the daily task
  }

  return [firstExpedition, secondExpedition];
}

export function parseExpeditionState(expeditionStateData: number[]): ExpeditionState {
  const expeditionState = {
    Stage: expeditionStateData[0],
    CurrentLocation: expeditionStateData[1],
    CurrentStatus: expeditionStateData[2],// 2 - status - 1 for encounter choice, 2 for boss fight, 3 for reward choosing (midway), 4 for wait time
    MainTask: expeditionStateData[3],
    SideTasks: [expeditionStateData[4], expeditionStateData[5], expeditionStateData[6]],
    MainTaskFinishedCounter: expeditionStateData[7],
    MainTaskMaxCounter: expeditionStateData[8],
    Backpack: [expeditionStateData[9], expeditionStateData[10], expeditionStateData[11], expeditionStateData[12]],
    CurrentHeroism: expeditionStateData[13],
    CurrentStars: expeditionStateData[14],
    WaitingStarted: expeditionStateData[15],
    WaitingUntil: expeditionStateData[16],
    TaskBonus: expeditionStateData[17],
    TaskPenalty: expeditionStateData[18],
    BonusForRepeatableTask: expeditionStateData[19],
    BonusLimitForRepeatableTask: expeditionStateData[20],
  }

  return expeditionState;
}

export function parseExpeditionCrossroad(expeditionCrossroadData: number[]): ExpeditionCrossroad {
  const crossroad = {
    Left: expeditionCrossroadData[0],
    LeftHeroism: expeditionCrossroadData[1],
    Mid: expeditionCrossroadData[2] ?? ExpeditionEncounter.None,
    MidHeroism: expeditionCrossroadData[3] ?? 0,
    Right: expeditionCrossroadData[4] ?? ExpeditionEncounter.None,
    RightHeroism: expeditionCrossroadData[5] ?? 0,
  }

  return crossroad;
}

export function parseExpeditionHalftime(expeditionHalftimeData: number[]): ExpeditionHalfTime {
  // [0] is the monster id for some reason
  return {
    LeftReward: {
      Type: expeditionHalftimeData[1] ?? ResourceType.None,
      Amount: expeditionHalftimeData[2] ?? 0,
    },
    MidReward: {
      Type: expeditionHalftimeData[3] ?? ResourceType.None,
      Amount: expeditionHalftimeData[4] ?? 0,
    },
    RightReward: {
      Type: expeditionHalftimeData[5] ?? ResourceType.None,
      Amount: expeditionHalftimeData[6] ?? 0,
    }
  }
}

export function parseExpeditionRewardResources(expeditionRewardResources: number[]): ExpeditionRewardResources {
  const rewards = [];
  const length = expeditionRewardResources[0] / 2;

  for (let i = 0; i < length; i++) {
    rewards.push({ Type: expeditionRewardResources[2 * i + 1], Amount: expeditionRewardResources[2 * i + 2], });
  }

  return {
    Resources: rewards
  }
}

export function parseExpeditionRewardItem(expeditionRewardItem: number[]): ExpeditionItemReward[] {
  const itemsCount = expeditionRewardItem.length / 12;

  const items = [];

  for (let i = 0; i < itemsCount; i++) {
    items.push({ GoldValue: expeditionRewardItem[10 + (i * 12)], })
  }

  return items;
}
