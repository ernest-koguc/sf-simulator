import { computed, Injectable, signal, WritableSignal } from '@angular/core';
import { PlayerModel } from '../sfgame/sfgame-parser';
import { Dungeons, Expedition, Resources, Tower, Witch } from '../sfgame/SFGameModels';
import { ExpeditionProgress } from './ExpeditionService';

@Injectable({
  providedIn: 'root'
})
export class SessionManager {
  public sessions: WritableSignal<Record<string, SessionData>> = signal({});

  public currentId: WritableSignal<string | null> = signal(null);

  public current = computed(() => {
    const current = this.currentId();
    if (!current) {
      return null;
    }

    return this.sessions()[current] || null;
  });

  public updateCurrent(update: (data: SessionData) => void) {
    const currentId = this.currentId();
    let currentData = this.current();
    if (currentId === null) {
      return;
    }

    if (currentData === null) {
      currentData = {
        player: null,
        playerName: null,
        server: null,
        guildName: null,
        dungeons: null,
        resources: null,
        tower: null,
        witch: null,
        expedition: null,
        expeditionProgress: null,
      }
    }

    update(currentData);

    this.sessions.update(sessions => {
      return { ...sessions, [currentId]: { ...currentData } };
    });
  }
}

type SessionData = {
  player: PlayerModel | null;
  playerName: string | null;
  server: string | null;
  guildName: string | null;
  dungeons: Dungeons | null;
  resources: Resources | null;
  tower: Tower | null;
  witch: Witch | null;
  expedition: Expedition[] | null;
  expeditionProgress: ExpeditionProgress | null;
}
