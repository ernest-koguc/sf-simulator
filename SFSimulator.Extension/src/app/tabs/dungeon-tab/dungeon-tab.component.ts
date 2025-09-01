import { ChangeDetectionStrategy, Component, computed, inject, signal, untracked, WritableSignal } from '@angular/core';
import { PercentPipe } from '@angular/common';
import { HlmButtonDirective } from '@spartan-ng/helm/button';
import { HlmSpinnerComponent } from '@spartan-ng/helm/spinner';
import { finalize } from 'rxjs';
import { HlmIconDirective } from '@spartan-ng/helm/icon';
import { lucideCircleAlert, lucideRefreshCw } from '@ng-icons/lucide';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { BigNumberPipe } from '../../pipes/big-number.pipe';
import { DungeonSimulator, SimulateDungeonResponse } from '../../services/DungeonSimulator';
import { SessionManager } from '../../services/SessionManager';
import { WarningService } from '../../services/WarningService';
import { db } from '../../db';

@Component({
  selector: 'dungeon-tab',
  imports: [PercentPipe, BigNumberPipe, HlmButtonDirective, HlmSpinnerComponent, HlmIconDirective, NgIcon],
  providers: [provideIcons({ lucideRefreshCw, lucideCircleAlert })],
  templateUrl: './dungeon-tab.component.html',
  styleUrl: './dungeon-tab.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DungeonTabComponent {
  public dungeonSimulator = inject(DungeonSimulator);
  public sessionManager = inject(SessionManager);
  public warningService = inject(WarningService);
  public warnings = computed(() => ([...this.warningService.ownMissingScrolls(), ...this.warningService.companionsMissingScrolls()]));

  public simulationResult: WritableSignal<SimulateDungeonResponse[] | null> = signal(null);
  public isSimulating: WritableSignal<boolean> = signal(false);

  public async copyToJson() {

    const records = await db.EquipmentGathering.filter(v => true).toArray();

    const items = records
      .map(record => record.Items.map(item => ({ UniquePlayerId: record.UniquePlayerId, Class: record.Class, ...item })))
      .reduce((a, b) => a.concat(b), [])
      .filter(item => item.ItemQuality >= 350 && item.Armor != 0)
      .map(item => `${item.UniquePlayerId}\t${item.Type}\t${item.ItemQuality}\t${item.Class}\t${item.Armor}\t${item.MinDmg}\t${item.MaxDmg}`)
      .join('\n');

    // copy to clipboard as josn
    await navigator.clipboard.writeText(items);
  }

  public simulateDungeons() {
    let fightable = this.sessionManager.fightable();
    let dungeons = this.sessionManager.current()?.dungeons;
    if (!fightable || !dungeons) {
      return;
    }

    this.isSimulating.set(true);
    this.simulationResult.set(null);

    this.dungeonSimulator.simulateOpenDungeons(fightable, dungeons)
      .pipe(finalize(() => {
        this.isSimulating.set(false);
      }))
      .subscribe(response => {
        response.sort((a, b) => a.dungeonMetadata.experience * a.winRatio > b.dungeonMetadata.experience * b.winRatio
          || (b.winRatio <= 5 && a.winRatio > b.winRatio * 10) ? -1 : 1);

        untracked(() => {
          this.simulationResult.set(response);
        });
      });
  }
}
