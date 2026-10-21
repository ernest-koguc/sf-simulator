import { inject, Injectable } from '@angular/core';
import {
  Expedition, ExpeditionCrossroad, ExpeditionEncounter, ExpeditionState, ExpeditionStatus, SFGameRequest,
} from '../sfgame/SFGameModels';
import {
  parseExpeditionCrossroad, parseExpeditions, parseExpeditionState,
} from '../sfgame/parsers/ExpeditionParser';
import { countItemRewards } from '../sfgame/parsers/EquipmentParser';
import { parseNumberList } from '../sfgame/parsers/ResponseParser';
import { db } from '../db';
import { SessionManager } from './SessionManager';

@Injectable({ providedIn: 'root' })
export class ExpeditionService {
  private sessionManager = inject(SessionManager);

  private updateExpeditionStateAndCrossroad(state: ExpeditionState, crossroad?: ExpeditionCrossroad) {
    if (!this.sessionManager.current()?.expeditionProgress?.isInProgress) {
      return;
    }
    this.sessionManager.updateCurrent(data => {
      const progress = data.expeditionProgress!;
      const stages = [...progress.stages];
      if (crossroad && state.Stage > 0 && !stages[state.Stage - 1]) {
        stages[state.Stage - 1] = {
          left: crossroad.Left, mid: crossroad.Mid, right: crossroad.Right, chosen: null,
        };
      }
      data.expeditionProgress = {
        ...progress,
        stageStatus: state.CurrentStatus,
        currentStage: state.Stage,
        heroism: state.CurrentHeroism,
        stages,
      };
    });
  }

  public async digestCommand(gameRequest: SFGameRequest) {
    const { data, params, req } = gameRequest;
    const previousChoices = this.sessionManager.current()?.expedition;

    // Responses with no request parameters (login/Update) still contain parser state.
    if (data['expeditions'] !== undefined) {
      const choices = parseExpeditions(parseNumberList(data['expeditions']));
      this.sessionManager.updateCurrent(current => { current.expedition = choices; });
    }
    const state = data['expeditionstate'] === undefined
      ? undefined : parseExpeditionState(parseNumberList(data['expeditionstate']));
    const crossroad = data['expeditioncrossroad'] === undefined
      ? undefined : parseExpeditionCrossroad(parseNumberList(data['expeditioncrossroad']));

    if (req === 'ExpeditionStart' && params !== null) {
      const index = Number(params) - 1;
      const expedition = (previousChoices ?? this.sessionManager.current()?.expedition)?.[index];
      if (expedition) {
        this.sessionManager.updateCurrent(current => {
          current.expeditionProgress = this.startExpedition(expedition);
        });
      }
    } else if (req === 'ExpeditionProceed' && this.sessionManager.current()?.expeditionProgress?.isInProgress) {
      this.sessionManager.updateCurrent(current => {
        const progress = current.expeditionProgress!;
        const chosen = params === '1' ? 'left' : params === '2' ? 'mid' : params === '3' ? 'right' : null;
        const stages = [...progress.stages];
        const index = progress.currentStage - 1;
        if (progress.stageStatus === ExpeditionStatus.EncounterChoosing && stages[index] && chosen) {
          stages[index] = { ...stages[index], chosen };
          current.expeditionProgress = { ...progress, stages };
        }
      });
    }

    if (state) {
      this.updateExpeditionStateAndCrossroad(state, crossroad);
    }

    const progress = this.sessionManager.current()?.expeditionProgress;
    if (progress?.isInProgress) {
      const isFinalReward = progress.currentStage === 10 && progress.stageStatus === ExpeditionStatus.Waiting;
      // Rust leaves resource amounts uninterpreted. The extension only needs
      // the presence of a chest reward for its chest-count statistic.
      const hasResourceReward = data['expeditionrewardresources'] !== undefined;
      const itemCount = data['expeditionreward'] === undefined
        ? 0 : countItemRewards(parseNumberList(data['expeditionreward']));
      this.sessionManager.updateCurrent(current => {
        current.expeditionProgress = {
          ...progress,
          chestCount: progress.chestCount + (hasResourceReward && !isFinalReward ? 1 : 0),
          itemCount: progress.itemCount + itemCount,
          finalRewardReceived: progress.finalRewardReceived || (hasResourceReward && isFinalReward),
        };
      });
      if (isFinalReward && this.sessionManager.current()?.expeditionProgress?.finalRewardReceived) {
        await this.saveExpeditionData();
      }
    }
  }

  public startExpedition(expedition: Expedition): ExpeditionProgress {
    return {
      isInProgress: true, chosenExpedition: expedition, heroism: 0, stages: [],
      currentStage: 1, stageStatus: ExpeditionStatus.EncounterChoosing,
      chestCount: 0, itemCount: 0, finalRewardReceived: false,
    };
  }

  private async saveExpeditionData() {
    const progress = this.sessionManager.current()?.expeditionProgress;
    const playerId = this.sessionManager.currentId();
    if (!progress || !playerId) {
      return;
    }
    await db.ExpeditionHistory.put({
      UniquePlayerId: playerId,
      MainTask: progress.chosenExpedition.MainTask,
      Thirst: progress.chosenExpedition.Time / 60,
      Heroism: progress.heroism,
      ChestCount: progress.chestCount,
      ItemCount: progress.itemCount,
      CreatedAt: new Date(),
    });
    this.sessionManager.updateCurrent(current => { current.expeditionProgress = null; });
  }
}

export type ExpeditionProgress = {
  isInProgress: boolean;
  chosenExpedition: Expedition;
  heroism: number;
  stages: ExpeditionStage[];
  currentStage: number;
  stageStatus: ExpeditionStatus;
  chestCount: number;
  itemCount: number;
  finalRewardReceived: boolean;
};

export type ExpeditionStage = {
  left: ExpeditionEncounter;
  mid: ExpeditionEncounter;
  right: ExpeditionEncounter;
  chosen: 'left' | 'mid' | 'right' | null;
};
