import { Injectable, inject, } from '@angular/core';
import { ExpeditionService } from './ExpeditionService';
import { SessionManager } from './SessionManager';
import { parseDungeons } from '../sfgame/parsers/DungeonParser';
import { parseResources } from '../sfgame/parsers/ResourcesParser';
import { parseWitch } from '../sfgame/parsers/WitchParser';
import { parseTower } from '../sfgame/parsers/TowerParser';
import { SFGameRequest } from '../sfgame/SFGameModels';
import { parsePets } from '../sfgame/parsers/PetParser';
import { parseOwnPlayerSave } from '../sfgame/parsers/OwnPlayerSaveParser';
import { parseBackpack, parseCompanionEquipment, parseEquipment } from '../sfgame/parsers/EquipmentParser';
import { parseEvents } from '../sfgame/parsers/EventParser';
import { parseDailyTaskList, parseDailyTaskRewards } from '../sfgame/parsers/DailyTasksParser';
import { parseToiletState } from '../sfgame/parsers/ToiletParser';
import { EquipmentService } from './EquipmentService';
import { parseOwnGroupSave } from '../sfgame/parsers/OwnGroupSaveParser';
import { parsePortalProgress } from '../sfgame/parsers/PortalProgressParser';
import { ByteParser } from '../sfgame/parsers/ByteParser';

@Injectable({
  providedIn: 'root'
})
export class SFGameRequestHandler {
  private expeditionService = inject(ExpeditionService);
  private sessionManager = inject(SessionManager);
  private equipmentService = inject(EquipmentService);
  private serverTime: number = Date.now();

  public async digestResponse(gameRequest: SFGameRequest) {
    const data = gameRequest.data;


    if (data['timestamp']) {
      this.serverTime = parseInt(data['timestamp']) * 1000 + ByteParser.timezoneOffset;
    }
    // TODO: instead of zillion if statements lets use some pattern matching
    if (data['ownplayersave']) {
      const save = numbers(data['ownplayersave'])!;

      for (const i of [4, 503, 504, 505, 561]) {
        save[i] = 0;
      }

      const server = gameRequest.server;
      const ownPlayerSave = parseOwnPlayerSave(save);

      const currentId = `${server}-${ownPlayerSave.ID}`;
      if (this.sessionManager.currentId() !== currentId) {
        this.sessionManager.currentId.set(currentId);
      }

      this.sessionManager.updateCurrent(current => {
        current.server = server;
        current.ownPlayerSave = ownPlayerSave;
      });

    }

    if (data['owntower']) {
      const towerData = numbers(data['owntower'])!;
      this.sessionManager.updateCurrent(data => {
        data.tower = parseTower(towerData);
      })
    }

    if (data['ownplayersaveequipment']) {
      const equipmentData = numbers(data['ownplayersaveequipment'])!;
      this.sessionManager.updateCurrent(current => {
        current.ownItems = parseEquipment(equipmentData);
      });
    }

    if (data['companionequipment']) {
      const companionData = numbers(data['companionequipment'])!;
      this.sessionManager.updateCurrent(current => {
        current.companionItems = parseCompanionEquipment(companionData);
      });
    }

    if (data['backpack']) {
      const backpackData = numbers(data['backpack'])!;
      this.sessionManager.updateCurrent(current => {
        current.backpack = parseBackpack(backpackData);
      });
    }

    if (data['dummieequipment']) {
      const dummyData = numbers(data['dummieequipment'])!;
      this.sessionManager.updateCurrent(current => {
        current.dummy = parseEquipment(dummyData);
      });
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

    if (data['owngroupsave']) {
      const groupSaveData = numbers(data['owngroupsave'])!;
      const ownGroup = parseOwnGroupSave(groupSaveData);
      this.sessionManager.updateCurrent(current => {
        current.guild = ownGroup;
      });
    }

    if (data['portalprogress']) {
      const portalProgressData = numbers(data['portalprogress'])!;
      const portalProgress = parsePortalProgress(portalProgressData, this.serverTime);
      this.sessionManager.updateCurrent(current => {
        current.portalProgress = portalProgress;
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

    if (data['ownpets']) {
      const petsData = numbers(data['ownpets'])!;
      this.sessionManager.updateCurrent(current => {
        current.pets = parsePets(petsData);
      });
    }

    if (data['tavernspecialsub']) {
      const events = parseInt(data['tavernspecialsub']);
      this.sessionManager.updateCurrent(current => {
        current.activeEvents = parseEvents(events);
      })
    }

    if (data['dailytaskrewardpreview']) {
      const dailyTasksRewards = numbers(data['dailytaskrewardpreview'])!;
      this.sessionManager.updateCurrent(current => {
        current.dailyTasksRewards = parseDailyTaskRewards(dailyTasksRewards);
      });
    }

    if (data['dailytasklist']) {
      const dailyTasks = numbers(data['dailytasklist'])!;
      this.sessionManager.updateCurrent(current => {
        current.dailyTasks = parseDailyTaskList(dailyTasks);
      });
    }

    if (data['toiletstate']) {
      const toiletState = numbers(data['toiletstate'])!;
      this.sessionManager.updateCurrent(current => {
        current.toiletState = parseToiletState(toiletState);
      });
    }

    if (gameRequest.req === 'PlayerLookAt' && data['otherplayersaveequipment'] && data['otherplayername'] && data['otherplayer']) {
      const otherPlayerEquipment = parseEquipment(numbers(data['otherplayersaveequipment'])!);
      const playerName = data['otherplayername'];
      const server = gameRequest.server;
      const otherPlayer = numbers(data['otherplayer'])!;
      await this.equipmentService.saveEquipment(otherPlayerEquipment, otherPlayer, playerName, server);
    }

    await this.expeditionService.digestCommand(gameRequest);

    return gameRequest.req === 'SFAccountCharLogin';
  }
}

export function numbers(val: string | undefined, delimiter: string | RegExp = '/') {
  return val?.split(delimiter).filter(v => v !== '').map(Number) ?? null;
}

export function mixed(val: string, delimiter: string = '/'): (number | string)[] {
  return val?.split(delimiter).filter(v => v !== '') ?? null;
}
