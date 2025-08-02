import { Injectable, signal, WritableSignal } from '@angular/core';
import { Expedition, ExpeditionCrossroad, ExpeditionEncounter, ExpeditionHalfTime, ExpeditionItemReward, ExpeditionRewardResources, ExpeditionState, ExpeditionStatus, SFGameRequest } from '../sfgame/SFGameModels';
import { numbers } from '../sfgame-model-tracker.service';
import { parseExpeditionCrossroad, parseExpeditionHalftime, parseExpeditionRewardItem, parseExpeditionRewardResources, parseExpeditions, parseExpeditionState } from '../sfgame/parsers/ExpeditionParser';
import { db } from '../db';

@Injectable({
  providedIn: 'root'
})
export class ExpeditionService {
  public expeditionChoices: WritableSignal<Expedition[]> = signal([]);
  public current: WritableSignal<PlayerExpedition | null> = signal(null);

  private pid: number | null = null;

  public switchPlayer(pid: number) {
    // TODO: we should have some player session storing to keep data between logouts
    this.current.set(null);
    this.pid = pid;
  }

  private updateExpeditionChoices(choices: Expedition[]) {
    this.expeditionChoices.set(choices);;
  }

  private updateExpeditionStateAndCrossroad(state: ExpeditionState, crossroad?: ExpeditionCrossroad) {
    if (this.current()?.isInProgress) {
      this.current.update(e => {
        e = e as PlayerExpedition;
        e.stageStatus = state.CurrentStatus;
        e.currentStage = state.Stage;
        e.heroism = state.CurrentHeroism;

        if (crossroad) {
          e.stages[e.currentStage - 1] = {
            left: crossroad.Left,
            mid: crossroad.Mid,
            right: crossroad.Right,
            chosen: null,
            backpack: state.Backpack,
          };
        }

        return { ...e };
      });
    }
  }

  private updateExpeditionHalftime(halftime: ExpeditionHalfTime) {
    if (this.current()?.isInProgress) {
      this.current.update(e => {
        e = e as PlayerExpedition;
        e.middleReward = halftime;
        return { ...e };
      });
    }
  }

  private updateExpeditionRewardItem(reward: ExpeditionItemReward[]) {
    if (this.current()?.isInProgress) {
      this.current.update(e => {
        e = e as PlayerExpedition;
        if (e.stages.length === 10) {
          e.finalItemRewards = reward;
        } else {
          e.middleItemRewards = reward;
        }
        return { ...e };
      });
    }
  }

  private updateExpeditionRewards(rewards: ExpeditionRewardResources) {
    if (this.current()?.isInProgress) {
      this.current.update(e => {
        e = e as PlayerExpedition;
        if (e.stages.length === 10 && e.stageStatus === ExpeditionStatus.Waiting) {
          e.finalRewards = rewards;
        } else {
          e.chestRewards.push(rewards);
        }
        return { ...e };
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
      this.updateExpeditionChoices(expeditionChoices);
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
      const expedition = this.expeditionChoices()[index];
      this.startExpedition(expedition);
    }
    else if (gameRequest.req === 'ExpeditionProceed' && this.current()?.isInProgress) {
      this.current.update(e => {
        e = e as PlayerExpedition;
        if (e.stageStatus === ExpeditionStatus.EncounterChoosing) {
          e.stages[e.stages.length - 1].chosen = gameRequest.params === '1' ? 'left' : gameRequest.params === '2' ? 'mid' : 'right';
        } else if (e.stageStatus === ExpeditionStatus.RewardChoosing) {
          e.chosenMiddleReward = gameRequest.params === '1' ? 'left' : gameRequest.params === '2' ? 'mid' : 'right';
        }
        return { ...e };
      });
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

    const currentExpedition = this.current();
    if (currentExpedition?.stageStatus === ExpeditionStatus.Waiting && currentExpedition.stages.length === 10) {
      await this.saveExpeditionData();
    }
  }

  public startExpedition(expedition: Expedition) {
    this.current.set({
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
    });
  }

  private async saveExpeditionData() {
    const expe = this.current()!;
    this.current.set(null);

    if (this.pid === null) {
      throw new Error('Cannot save expedition data: Player ID is null')
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
      PlayerId: this.pid!,
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

type PlayerExpedition = {
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

type ExpeditionStage = {
  left: ExpeditionEncounter,
  mid: ExpeditionEncounter,
  right: ExpeditionEncounter,
  chosen: 'left' | 'mid' | 'right' | null,
  backpack: ExpeditionEncounter[],
}
