import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Dungeons } from '../sfgame/SFGameModels';
import { environment } from '../../environments/environment.development';
import { Fightable } from '../sfgame/Fightable';

@Injectable({
  providedIn: 'root'
})
export class DungeonSimulator {
  private httpClient = inject(HttpClient);

  public simulateOpenDungeons(fightable: Fightable, dungeons: Dungeons) {
    let body = this.createBody(dungeons, fightable);

    return this.httpClient.post<SimulateDungeonResponse[]>(`${environment.apiUrl}/api/dungeon/simulate`, body);
  }

  private createBody(dungeons: Dungeons, fightable: Fightable) {
    const { Companions, ...Player } = fightable
    return {
      Iterations: 100000,
      WinThreshold: 100000,
      DungeonLevels: {
        ...this.normalizeDungeons(dungeons),
      },
      Player: Player,
      Companions: Companions
    };
  }

  private normalizeDungeons(dungeons: Dungeons) {
    const light: Record<number, number> = {};
    for (let i = 0; i < dungeons.Light.length; i++) {
      const dung = dungeons.Light[i];
      light[dung.Position] = dung.Current;

    }

    const shadow: Record<number, number> = {};
    for (let i = 0; i < dungeons.Shadow.length; i++) {
      const dung = dungeons.Shadow[i];
      shadow[dung.Position] = dung.Current;

    }

    const dungeonLevels = {
      ...light,
      ...shadow,
      "-1": dungeons.Tower,
      "-2": dungeons.Twister,
      "-3": dungeons.LoopOfIdols,
      "-4": dungeons.Sandstorm,
    };
    return dungeonLevels;
  }
}

export type SimulateDungeonResponse = {
  winRatio: number,
  dungeonMetadata: {
    position: number,
    dungeonName: string,
    enemyName: string,
    withCompanions: boolean,
    class: number,
    experience: number
  }
}
