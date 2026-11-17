import { ChangeDetectionStrategy, Component, computed, inject, signal, untracked, WritableSignal } from '@angular/core';
import { PercentPipe } from '@angular/common';
import { HlmButton } from '@spartan-ng/helm/button';
import { HlmSpinner } from '@spartan-ng/helm/spinner';
import { finalize } from 'rxjs';
import { lucideCircleAlert, lucideRefreshCw } from '@ng-icons/lucide';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { BigNumberPipe } from '../../pipes/big-number.pipe';
import { DungeonSimulator, SimulateDungeonResponse } from '../../services/DungeonSimulator';
import { SessionManager } from '../../services/SessionManager';
import { WarningService } from '../../services/WarningService';

@Component({
  selector: 'dungeon-tab',
  imports: [PercentPipe, BigNumberPipe, HlmButton, HlmSpinner, NgIcon],
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

        this.simulationResult.set(response);
      });
  }
}
