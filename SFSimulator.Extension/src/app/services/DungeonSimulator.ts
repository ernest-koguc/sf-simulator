import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Dungeons } from '../sfgame/SFGameModels';
import { environment } from '../../environments/environment';
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
      Iterations: 1000000,
      WinThreshold: 100000,
      DungeonState: dungeons,
      Player: Player,
      Companions: Companions
    };
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
