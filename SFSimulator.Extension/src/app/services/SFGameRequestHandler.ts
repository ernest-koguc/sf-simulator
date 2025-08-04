import { Injectable, inject, } from '@angular/core';
import { ExpeditionService } from './ExpeditionService';
import { SessionManager } from './SessionManager';
import { parseDungeons } from '../sfgame/parsers/DungeonParser';
import { parseResources } from '../sfgame/parsers/ResourcesParser';
import { parseWitch } from '../sfgame/parsers/WitchParser';
import { DataModel, PlayerModel } from '../sfgame/sfgame-parser';
import { parseTower } from '../sfgame/parsers/TowerParser';
import { SFGameRequest } from '../sfgame/SFGameModels';

@Injectable({
  providedIn: 'root'
})
export class SFGameRequestHandler {
  private expeditionService = inject(ExpeditionService);
  private sessionManager = inject(SessionManager);

  public async digestResponse(gameRequest: SFGameRequest) {
    const data = gameRequest.data;

    const save = numbers(data['ownplayersave']);

    if (save) {
      const model: DataModel = {
        ownsave: save,
        units: numbers(data['unitlevel'], /\/|,/),
        pets: numbers(data['ownpets']),
        chest: numbers(data['fortresschest']),
        dummy: numbers(data['dummies']),
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
      const server = gameRequest.server;

      this.sessionManager.currentId.set(`${server}-${playerModel.ID}`);
      this.sessionManager.updateCurrent(data => {
        data.player = playerModel;
        data.server = server;
      })

      if (playerModel && data['owntower']) {
        const towerData = numbers(data['owntower'])!;
        const tower = parseTower(towerData, playerModel);
        this.sessionManager.updateCurrent(data => {
          data.tower = tower;
        })
      }

    }

    if (data['ownplayername']) {
      this.sessionManager.updateCurrent(current => {
        current.playerName = data['ownplayername'];
      })
    }

    if (data['owngroupname']) {
      this.sessionManager.updateCurrent(current => {
        current.guildName = data['owngroupname'];
      });
    }

    if (data['dungeonprogresslight'] && data['dungeonprogressshadow']) {
      const dungeonData = {
        light: numbers(data['dungeonprogresslight'])!,
        shadow: numbers(data['dungeonprogressshadow'])!
      };

      this.sessionManager.updateCurrent(current => {
        current.dungeons = parseDungeons(dungeonData);
      })
    }

    if (data['resources']) {
      const resourcesData = numbers(data['resources'])!;
      this.sessionManager.updateCurrent(current => {
        current.resources = parseResources(resourcesData);
      });
    }

    if (data['witch']) {
      const witchData = numbers(data['witch'])!;
      this.sessionManager.updateCurrent(current => {
        current.witch = parseWitch(witchData);
      });
    }

    await this.expeditionService.digestCommand(gameRequest);

    return gameRequest.req === 'SFAccountCharLogin';
  }
}

export function numbers(val: string | undefined, delimiter: string | RegExp = '/') {
  return val?.split(delimiter).filter(v => v !== '').map(Number) ?? null;
}
