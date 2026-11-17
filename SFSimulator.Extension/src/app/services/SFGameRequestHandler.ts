import { Injectable, inject } from '@angular/core';
import { SessionData, SessionManager } from './SessionManager';
import { SFGameRequest } from '../sfgame/SFGameModels';
import { parseDungeons } from '../sfgame/parsers/DungeonParser';
import { parseWitch } from '../sfgame/parsers/WitchParser';
import { parseTower } from '../sfgame/parsers/TowerParser';
import { parsePets } from '../sfgame/parsers/PetParser';
import { parseCharacterClass, parseOwnPlayerSave } from '../sfgame/parsers/OwnPlayerSaveParser';
import { parseCompanionEquipment, parseEquipment } from '../sfgame/parsers/EquipmentParser';
import { parseEvents } from '../sfgame/parsers/EventParser';
import { parseDailyTaskList, parseDailyTaskRewards } from '../sfgame/parsers/DailyTasksParser';
import { parseToiletState } from '../sfgame/parsers/ToiletParser';
import { parseOwnGroupSave } from '../sfgame/parsers/OwnGroupSaveParser';
import { parsePortalProgress } from '../sfgame/parsers/PortalProgressParser';
import { parseNumberList, parseResponseList } from '../sfgame/parsers/ResponseParser';
import {
  parseArenaFights, parseCanAttackHydra, parseCharacterStatus, parseDiceGamesRemaining,
  parseHasEternalPotion, parsePotions, parseWheelSpins,
} from '../sfgame/parsers/CharacterStatusParser';

@Injectable({ providedIn: 'root' })
export class SFGameRequestHandler {
  private sessionManager = inject(SessionManager);
  private serverOffsets = new Map<string, number>();

  public async digestResponse(gameRequest: SFGameRequest) {
    const { data, server } = gameRequest;
    const receivedAt = Date.now();
    if (data['timestamp'] !== undefined) {
      const timestamp = Number(data['timestamp']);
      if (!Number.isInteger(timestamp) || timestamp <= 0) {
        throw new TypeError('Invalid server timestamp');
      }
      this.serverOffsets.set(server, timestamp * 1000 - receivedAt);
    }
    // Store the clock difference, so queued updates do not freeze server time.
    const serverTime = receivedAt + (this.serverOffsets.get(server)
      ?? -new Date(receivedAt).getTimezoneOffset() * 60_000);

    // Resolve identity before applying any response keys, regardless of key order.
    if (data['ownplayersavecharacter'] !== undefined) {
      const ownPlayerSave = parseOwnPlayerSave(parseNumberList(data['ownplayersavecharacter']));
      this.sessionManager.currentId.set(`${server}-${ownPlayerSave.ID}`);
      this.sessionManager.updateCurrent(current => {
        current.server = server;
        current.ownPlayerSave = ownPlayerSave;
      });
    }

    if (!this.sessionManager.currentId()?.startsWith(`${server}-`)) {
      return false;
    }

    const update = <K extends keyof SessionData>(
      key: string, field: K, parse: (words: number[]) => SessionData[K],
    ) => {
      const value = data[key];
      if (value === undefined) {
        return;
      }
      const parsed = parse(parseNumberList(value));
      if (parsed !== null) {
        this.sessionManager.updateCurrent(current => { current[field] = parsed; });
      }
    };

    if (data['ownplayername'] !== undefined) {
      this.sessionManager.updateCurrent(current => { current.playerName = data['ownplayername']; });
    }

    update('owntower', 'tower', parseTower);
    update('ownplayersaveequipment', 'ownItems', parseEquipment);
    update('companionequipment', 'companionItems', words => words.length ? parseCompanionEquipment(words) : null);
    update('ownplayersavepotions', 'hasEternalPotion', parseHasEternalPotion);
    update('ownplayersavepotions', 'potions', parsePotions);
    update('characterstatus', 'characterStatus', parseCharacterStatus);
    update('wheel', 'wheelSpinsToday', parseWheelSpins);
    update('dice', 'diceGamesRemaining', parseDiceGamesRemaining);
    update('arena', 'arenaFightsToday', parseArenaFights);
    update('charactergroup', 'canAttackHydra', parseCanAttackHydra);
    if (data['owngroupsave'] !== undefined) {
      const guild = parseOwnGroupSave(parseResponseList(data['owngroupsave']));
      this.sessionManager.updateCurrent(current => { current.guild = guild; });
    }
    update('portalprogress', 'portalProgress', words => parsePortalProgress(words, serverTime));

    if (data['dungeonprogresslight'] !== undefined
      || data['dungeonprogressshadow'] !== undefined
      || data['dungeonprogressclass'] !== undefined) {
      const dungeonData = {
        light: data['dungeonprogresslight'] === undefined ? undefined : parseNumberList(data['dungeonprogresslight']),
        shadow: data['dungeonprogressshadow'] === undefined ? undefined : parseNumberList(data['dungeonprogressshadow']),
        class: data['dungeonprogressclass'] === undefined ? undefined : parseNumberList(data['dungeonprogressclass'])
      };
      this.sessionManager.updateCurrent(current => {
        current.dungeons = parseDungeons(dungeonData);
      });
    }

    update('witch', 'witch', parseWitch);
    update('ownpets', 'pets', parsePets);
    update('events', 'activeEvents', parseEvents);
    update('dailytaskrewardpreview', 'dailyTasksRewards', parseDailyTaskRewards);
    update('dailytasklist', 'dailyTasks', parseDailyTaskList);
    update('toiletstate', 'toiletState', parseToiletState);

    if (data['owngroupname'] === '') {
      this.sessionManager.updateCurrent(current => {
        current.guild = null;
        current.canAttackHydra = null;
      });
    }

    return gameRequest.req === 'SFAccountCharLogin';
  }
}
