import { ChangeDetectionStrategy, Component, computed, inject, signal, WritableSignal } from '@angular/core';
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
import { ScrollType } from '../../sfgame/SFGameModels';

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
  public warnings = computed(() => {
    const player = this.sessionManager.current()?.player;
    if (!player) {
      return [];
    }

    const warnings = [];

    const witch = this.sessionManager.current()?.witch;
    const isCritScrollUnlocked = witch?.Scrolls.find(s => s.Type === ScrollType.Crit)?.Unlocked && player.Level >= 66;
    if (isCritScrollUnlocked === true && !player.Items.Wpn1?.HasEnchantment && !player.Items.Wpn2?.HasEnchantment) {
      warnings.push('Weapon scroll missing!');
    }

    const isReactionScrollUnlocked = witch?.Scrolls.find(s => s.Type === ScrollType.Reaction)?.Unlocked && player.Level >= 66;
    if (isReactionScrollUnlocked === true && !player.Items.Hand?.HasEnchantment) {
      warnings.push('Gloves scroll missing!');
    }

    // TODO: Do same for companions

    return warnings;
  });

  public simulationResult: WritableSignal<SimulateDungeonResponse[] | null> = signal(null);
  public isSimulating = signal(false);

  public simulateDungeons() {
    let player = this.sessionManager.current()?.player;
    let dungeons = this.sessionManager.current()?.dungeons;
    let tower = this.sessionManager.current()?.tower;
    if (!player || !dungeons || !tower) {
      return;
    }

    this.isSimulating.set(true);
    this.dungeonSimulator.simulateOpenDungeons(player, dungeons, tower).pipe(finalize(() => this.isSimulating.set(false))).subscribe({
      next: (response) => {
        response.sort((a, b) => a.dungeonMetadata.experience * a.winRatio > b.dungeonMetadata.experience * b.winRatio
          || (b.winRatio <= 5 && a.winRatio > b.winRatio * 10) ? -1 : 1);
        this.simulationResult.set(response);
      },
      error: (err) => {
        console.error('Error simulating dungeon:', err);
        this.simulationResult.set(null);
      },
    });
  }
}
