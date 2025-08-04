import { inject, Injectable, } from '@angular/core';
import {
  Expedition,
  ExpeditionCrossroad,
  ExpeditionEncounter,
  ExpeditionHalfTime,
  ExpeditionItemReward,
  ExpeditionRewardResources,
  ExpeditionState,
  ExpeditionStatus,
  SFGameRequest
}
  from '../sfgame/SFGameModels';
import {
  parseExpeditionCrossroad,
  parseExpeditionHalftime,
  parseExpeditionRewardItem,
  parseExpeditionRewardResources,
  parseExpeditions,
  parseExpeditionState
}
  from '../sfgame/parsers/ExpeditionParser';
import { db } from '../db';
import { SessionManager } from './SessionManager';
import { numbers } from './SFGameRequestHandler';

@Injectable({
  providedIn: 'root'
})
export class ExpeditionService {
  private sessionManager = inject(SessionManager);

  private updateExpeditionStateAndCrossroad(state: ExpeditionState, crossroad?: ExpeditionCrossroad) {
    if (this.sessionManager.current()?.expeditionProgress?.isInProgress) {
      this.sessionManager.updateCurrent(data => {
        data.expeditionProgress!.stageStatus = state.CurrentStatus;
        data.expeditionProgress!.currentStage = state.Stage;
        data.expeditionProgress!.heroism = state.CurrentHeroism;

        const currentStage = data.expeditionProgress!.currentStage - 1;
        if (crossroad && !data.expeditionProgress!.stages[currentStage]) {
          data.expeditionProgress!.stages[currentStage] = {
            left: crossroad.Left,
            mid: crossroad.Mid,
            right: crossroad.Right,
            chosen: null,
            backpack: state.Backpack,
          };
        }

        data.expeditionProgress = { ...data.expeditionProgress! };
      });
    }
  }

  private updateExpeditionHalftime(halftime: ExpeditionHalfTime) {
    if (this.sessionManager.current()?.expeditionProgress?.isInProgress) {
      this.sessionManager.updateCurrent(data => {
        data.expeditionProgress!.middleReward = halftime;
      });
    }
  }

  private updateExpeditionRewardItem(reward: ExpeditionItemReward[]) {
    if (this.sessionManager.current()?.expeditionProgress?.isInProgress) {
      this.sessionManager.updateCurrent(data => {
        if (data.expeditionProgress!.stages.length === 10) {
          data.expeditionProgress!.finalItemRewards = reward;
        } else {
          data.expeditionProgress!.middleItemRewards = reward;
        }
      });
    }
  }

  private updateExpeditionRewards(rewards: ExpeditionRewardResources) {
    if (this.sessionManager.current()?.expeditionProgress?.isInProgress) {
      this.sessionManager.updateCurrent(data => {
        if (data.expeditionProgress!.stages.length === 10 && data.expeditionProgress!.stageStatus === ExpeditionStatus.Waiting) {
          data.expeditionProgress!.finalRewards = rewards;
        } else {
          data.expeditionProgress!.chestRewards.push(rewards);
        }
        return { ...data };
      });
    }
  }

  public async digestCommand(gameRequest: SFGameRequest) {
    if (!gameRequest.params) {
      return;
    }

    const data = gameRequest.data;
    let expeditionState, expeditionCrossroad, expeditionHalftime, expeditionRewardResources, expeditionRewardItems;
    if (data['expeditions']) {
      const expeditionChoices = parseExpeditions(numbers(data['expeditions'])!);
      this.sessionManager.updateCurrent(data => data.expedition = expeditionChoices);
    }

    if (data['expeditionstate']) {
      expeditionState = parseExpeditionState(numbers(data['expeditionstate'])!);
    }

    if (data['expeditioncrossroad']) {
      expeditionCrossroad = parseExpeditionCrossroad(numbers(data['expeditioncrossroad'])!);
    }

    if (data['expeditionhalftime']) {
      expeditionHalftime = parseExpeditionHalftime(numbers(data['expeditionhalftime'])!);
    }

    if (data['expeditionrewardresources']) {
      expeditionRewardResources = parseExpeditionRewardResources(numbers(data['expeditionrewardresources'])!);
    }

    if (data['expeditionreward']) {
      expeditionRewardItems = parseExpeditionRewardItem(numbers(data['expeditionreward'])!);
    }

    if (gameRequest.req === 'ExpeditionStart') {
      const index = parseInt(gameRequest.params) - 1;
      const expedition = this.sessionManager.current()!.expedition![index];
      const expeditionProgress = this.startExpedition(expedition);
      this.sessionManager.updateCurrent(data => data.expeditionProgress = expeditionProgress);
    }
    else if (gameRequest.req === 'ExpeditionProceed' && this.sessionManager.current()?.expeditionProgress?.isInProgress) {
      this.sessionManager.updateCurrent(data => {
        if (data.expeditionProgress!.stageStatus === ExpeditionStatus.EncounterChoosing) {
          data.expeditionProgress!.stages[data.expeditionProgress!.stages.length - 1].chosen = gameRequest.params === '1' ? 'left' : gameRequest.params === '2' ? 'mid' : 'right';
        } else if (data.expeditionProgress!.stageStatus === ExpeditionStatus.RewardChoosing) {
          data.expeditionProgress!.chosenMiddleReward = gameRequest.params === '1' ? 'left' : gameRequest.params === '2' ? 'mid' : 'right';
        }
      })
    }

    if (expeditionState) {
      this.updateExpeditionStateAndCrossroad(expeditionState, expeditionCrossroad);
    }

    if (expeditionHalftime) {
      this.updateExpeditionHalftime(expeditionHalftime);
    }

    if (expeditionRewardResources) {
      this.updateExpeditionRewards(expeditionRewardResources);
    }

    if (expeditionRewardItems) {
      this.updateExpeditionRewardItem(expeditionRewardItems);
    }

    const currentExpedition = this.sessionManager.current()?.expeditionProgress;
    if (currentExpedition?.stageStatus === ExpeditionStatus.Waiting && currentExpedition.stages.length === 10) {
      await this.saveExpeditionData();
    }
  }

  public startExpedition(expedition: Expedition) {
    return {
      isInProgress: true,
      chosenExpedition: expedition,
      heroism: 0,
      stages: [],
      currentStage: 1,
      stageStatus: ExpeditionStatus.EncounterChoosing,
      middleReward: null,
      chestRewards: [],
      finalRewards: null,
      middleItemRewards: [],
      chosenMiddleReward: null,
      finalItemRewards: [],
    };
  }

  private async saveExpeditionData() {
    const expe = this.sessionManager.current()?.expeditionProgress!;
    this.sessionManager.updateCurrent(data => data.expeditionProgress = null);

    const pid = this.sessionManager.current()?.player?.ID;
    if (!pid) {
      throw new Error('Cannot save expedition data: Player ID is null')
    }
    const server = this.sessionManager.current()?.server;
    if (!server) {
      throw new Error('Cannot save expedition data: Server is null');
    }

    if (expe.middleReward === null) {
      throw new Error('Cannot save expedition data: Middle reward is null');
    }

    if (expe.stages.some(stage => stage.chosen === null)) {
      throw new Error('Cannot save expedition data: Not all stages have been chosen');
    }

    if (expe.chosenMiddleReward === null) {
      throw new Error('Cannot save expedition data: Middle reward choice is null');
    }

    if (expe.finalRewards === null) {
      throw new Error('Cannot save expedition data: Final rewards are null');
    }

    const expeditionHistoryItem = {
      PlayerId: pid,
      Server: server,
      MainTask: expe.chosenExpedition.MainTask,
      SideTasks: expe.chosenExpedition.SideTasks,
      Heroism: expe.heroism,
      HalfTime: expe.middleReward,
      HalfTimeChosen: expe.chosenMiddleReward,
      HalfTimeItems: expe.middleItemRewards,
      ChestRewards: expe.chestRewards,
      FinalRewards: expe.finalRewards,
      FinalItems: expe.finalItemRewards,
      Stages: expe.stages.map(stage => ({
        Left: stage.left,
        Mid: stage.mid,
        Right: stage.right,
        Chosen: stage.chosen!,
        Backpack: stage.backpack
      })),
      CreatedAt: new Date(),
    };

    await db.ExpeditionHistory.put(expeditionHistoryItem);
  }
}

export type ExpeditionProgress = {
  isInProgress: boolean,
  chosenExpedition: Expedition,
  heroism: number,
  stages: ExpeditionStage[],
  currentStage: number,
  stageStatus: ExpeditionStatus,
  middleReward: ExpeditionHalfTime | null,
  chosenMiddleReward: 'left' | 'mid' | 'right' | null,
  chestRewards: ExpeditionRewardResources[],
  finalRewards: ExpeditionRewardResources | null,
  middleItemRewards: ExpeditionItemReward[];
  finalItemRewards: ExpeditionItemReward[];
}

export type ExpeditionStage = {
  left: ExpeditionEncounter,
  mid: ExpeditionEncounter,
  right: ExpeditionEncounter,
  chosen: 'left' | 'mid' | 'right' | null,
  backpack: ExpeditionEncounter[],
}
