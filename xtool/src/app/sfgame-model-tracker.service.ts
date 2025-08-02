import { Injectable, WritableSignal, inject, signal } from '@angular/core';
import { DataModel, PlayerModel } from './sfgame/sfgame-parser';
import { Dungeons, Resources, SFGameRequest, Tower } from './sfgame/SFGameModels';
import { parseDungeons } from './sfgame/parsers/DungeonParser';
import { parseResources } from './sfgame/parsers/ResourcesParser';
import { parseTower } from './sfgame/parsers/TowerParser';
import { ExpeditionService } from './services/expedition.service';

@Injectable({
  providedIn: 'root'
})
export class SFGameModelTracker {
  public player: WritableSignal<PlayerModel | null> = signal(null);
  public playerName: WritableSignal<string | null> = signal(null);
  public guildName: WritableSignal<string | null> = signal(null);
  public dungeons: WritableSignal<Dungeons | null> = signal(null);
  public resources: WritableSignal<Resources | null> = signal(null);
  public tower: WritableSignal<Tower | null> = signal(null);

  private expeditionService = inject(ExpeditionService);

  public async digestResponse(gameRequest: SFGameRequest) {
    const data = gameRequest.data;

    if (data['ownplayername']) {
      this.playerName.set(data['ownplayername']);
    }

    if (data['owngroupname']) {
      this.guildName.set(data['owngroupname']);
    }

    if (data['dungeonprogresslight'] && data['dungeonprogressshadow']) {
      const dungeonData = {
        light: numbers(data['dungeonprogresslight'])!,
        shadow: numbers(data['dungeonprogressshadow'])!
      };

      this.dungeons.set(parseDungeons(dungeonData));
    }

    if (data['resources']) {
      const resourcesData = numbers(data['resources'])!;
      this.resources.set(parseResources(resourcesData));
    }

    await this.expeditionService.digestCommand(gameRequest);

    const save = numbers(data['ownplayersave']);

    if (!save) {
      return;
    }

    const model: DataModel = {
      ownsave: save,
      units: numbers(data['unitlevel'], /\/|,/),
      pets: numbers(data['ownpets']),
      chest: numbers(data['fortresschest']),
      dummy: numbers(data['dummies']),
      witch: numbers(data['witch']),
      idle: numbers(data['idle']),
      calendar: numbers(data['calenderinfo']),
      dailyTasks: numbers(data['dailytasklist']),
      dailyTasksRewards: numbers(data['dailytaskrewardpreview']),
      eventTasks: numbers(data['eventtasklist']),
      eventTasksRewards: numbers(data['eventtaskrewardpreview']),
    };

    for (const i of [4, 503, 504, 505, 561]) {
      model.ownsave[i] = 0;
    }

    const playerModel = new PlayerModel(model, null);
    this.player.set(playerModel);

    const player = this.player();
    if (player && data['owntower']) {
      const towerData = numbers(data['owntower'])!;
      const tower = parseTower(towerData, player);
      this.tower.set(tower);
    }

    return gameRequest.req === 'SFAccountCharLogin';
  }
}

export function numbers(val: string | undefined, delimiter: string | RegExp = '/') {
  return val?.split(delimiter).filter(v => v !== '').map(Number) ?? null;
}
