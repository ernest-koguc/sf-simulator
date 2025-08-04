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
import { db } from '../../db';
import { AsyncPipe, DatePipe } from '@angular/common';

@Component({
  selector: 'expedition-tab',
  imports: [HlmTabsComponent, HlmTabsListComponent, HlmTabsContentDirective, HlmTabsTriggerDirective, AsyncPipe, DatePipe],
  templateUrl: './expedition-tab.component.html',
  styleUrl: './expedition-tab.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExpeditionTabComponent {
  public sessionManager = inject(SessionManager);
  public expeditionProgress = computed(() => this.sessionManager.current()?.expeditionProgress);
  public expeditions = computed(() => this.sessionManager.current()?.expedition);

  public savedExpeditions = liveQuery(() => {
    const session = this.sessionManager.current();
    const server = session?.server;
    const pid = session?.player?.ID;
    db.ExpeditionHistory.filter(i => i.Server === server && i.PlayerId === pid).toArray()
  });

  public getAssetUrl(encounterId: ExpeditionEncounter) {
    return `https://sfsimulator.xyz/external/expedition/encounter${encounterId.toString()}.png`;
  }
}
