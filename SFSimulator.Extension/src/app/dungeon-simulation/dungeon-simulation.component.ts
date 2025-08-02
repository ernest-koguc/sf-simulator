import { ChangeDetectionStrategy, Component, inject, signal, WritableSignal } from '@angular/core';
import { PercentPipe } from '@angular/common';
import { DungeonSimulator, SimulateDungeonResponse } from '../dungeon-simulator.service';
import { BigNumberPipe } from "../pipes/big-number.pipe";
import { SFGameModelTracker } from '../sfgame-model-tracker.service';
import { HlmButtonDirective } from '@spartan-ng/helm/button';
import { HlmSpinnerComponent } from '@spartan-ng/helm/spinner';
import { finalize } from 'rxjs';
import { HlmIconDirective } from '@spartan-ng/helm/icon';
import { lucideRefreshCw } from '@ng-icons/lucide';
import { NgIcon, provideIcons } from '@ng-icons/core';

@Component({
  selector: 'dungeon-simulation',
  imports: [PercentPipe, BigNumberPipe, HlmButtonDirective, HlmSpinnerComponent, HlmIconDirective, NgIcon],
  providers: [provideIcons({ lucideRefreshCw })],
  templateUrl: './dungeon-simulation.component.html',
  styleUrl: './dungeon-simulation.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DungeonSimulationComponent {
  public tracker = inject(SFGameModelTracker);
  public dungeonSimulator = inject(DungeonSimulator);

  public simulationResult: WritableSignal<SimulateDungeonResponse[] | null> = signal(null);
  public isSimulating = signal(false);

  public simulateDungeons() {
    let player = this.tracker.player();
    let dungeons = this.tracker.dungeons();
    let tower = this.tracker.tower();
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
