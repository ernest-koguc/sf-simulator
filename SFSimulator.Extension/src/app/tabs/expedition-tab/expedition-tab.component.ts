import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import {
  HlmTabsComponent,
  HlmTabsListComponent,
  HlmTabsContentDirective,
  HlmTabsTriggerDirective
} from '@spartan-ng/helm/tabs';
import { ExpeditionEncounter } from '../../sfgame/SFGameModels';
import { SessionManager } from '../../services/SessionManager';
import { liveQuery } from 'dexie';
import { db, ExpeditionHistoryItem } from '../../db';
import { AsyncPipe, DatePipe, DecimalPipe } from '@angular/common';
import { HlmTableImports } from '@spartan-ng/helm/table';
import { HlmButtonDirective } from '@spartan-ng/helm/button';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { HlmIconDirective } from '@spartan-ng/helm/icon';
import { lucideX } from '@ng-icons/lucide';
import { toSignal } from '@angular/core/rxjs-interop';
import { Observable } from 'rxjs';

@Component({
  selector: 'expedition-tab',
  imports: [HlmTabsComponent,
    HlmTabsListComponent,
    HlmTabsContentDirective,
    HlmTabsTriggerDirective,
    AsyncPipe,
    DatePipe,
    DecimalPipe,
    HlmTableImports,
    HlmButtonDirective,
    NgIcon,
    HlmIconDirective
  ],
  providers: [
    provideIcons({ lucideX }),
  ],
  templateUrl: './expedition-tab.component.html',
  styleUrl: './expedition-tab.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExpeditionTabComponent {
  public sessionManager = inject(SessionManager);
  public expeditionProgress = computed(() => this.sessionManager.current()?.expeditionProgress);
  public expeditions = computed(() => this.sessionManager.current()?.expedition);

  public savedExpeditions = computed(() => {
    const upid = this.sessionManager.currentId();

    return liveQuery(() => db.ExpeditionHistory.filter(e => e.UniquePlayerId === upid).toArray());
  });

  public getAssetUrl(encounterId: ExpeditionEncounter) {
    return `https://sfsimulator.xyz/external/expedition/encounter${encounterId.toString()}.png`;
  }

  public getAvgChest(expes: ExpeditionHistoryItem[]) {
    if (expes.length === 0) {
      return 0;
    }
    const totalChests = expes.reduce((sum, exp) => sum + exp.ChestRewards.length, 0);
    return totalChests / expes.length;
  }

  public getAvgHeroism(expes: ExpeditionHistoryItem[]) {
    if (expes.length === 0) {
      return 0;
    }
    const totalHeroism = expes.reduce((sum, exp) => sum + exp.Heroism, 0);
    return totalHeroism / expes.length;
  }

  public getItemsPerThirst(expes: ExpeditionHistoryItem[]) {
    if (expes.length === 0) {
      return 0;
    }
    const totalItems = expes.reduce((sum, exp) => sum + exp.HalfTimeItems.length + exp.FinalItems.length, 0);
    const totalThirst = expes.reduce((sum, exp) => sum + exp.Thirst, 0);
    return totalItems / totalThirst * 20;
  }

  public getStatsPerExpeditionType(expes: ExpeditionHistoryItem[]) {
    const types = expes.map(e => e.MainTask).filter((value, index, self) => self.indexOf(value) === index);
    const stats = [];
    for (let type of types) {
      const typeExpes = expes.filter(e => e.MainTask === type);

      const avgHeroism = this.getAvgHeroism(typeExpes);
      const avgChests = this.getAvgChest(typeExpes);

      stats.push({
        Id: typeExpes[typeExpes.length - 1].Id!,
        MainTask: type,
        AvgHeroism: avgHeroism,
        AvgChests: avgChests,
        Count: typeExpes.length
      });
    }

    return stats;
  }

  public async deleteExpedition(id: number) {
    await db.ExpeditionHistory.delete(id);
  }

  public cast(value: any): ExpeditionHistoryItem[] | null {
    return value;
  }
}
