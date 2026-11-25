import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { PercentPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { HlmAlertImports } from '@spartan-ng/helm/alert';
import { HlmButton } from '@spartan-ng/helm/button';
import { HlmSpinner } from '@spartan-ng/helm/spinner';
import { catchError, distinctUntilChanged, map, of, startWith, switchMap } from 'rxjs';
import { lucideCircleAlert, lucideRefreshCw } from '@ng-icons/lucide';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { BigNumberPipe } from '../../pipes/big-number.pipe';
import { DungeonSimulator, DungeonTypeEnum } from '../../services/DungeonSimulator';
import { SessionManager } from '../../services/SessionManager';
import { WarningService } from '../../services/WarningService';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { debounceWithMaxWait } from '../../utils/debounceWithMaxWait';

@Component({
  selector: 'dungeon-tab',
  imports: [PercentPipe, BigNumberPipe, HlmAlertImports, HlmButton, HlmSpinner, NgIcon],
  providers: [provideIcons({ lucideRefreshCw, lucideCircleAlert })],
  templateUrl: './dungeon-tab.component.html',
  styleUrl: './dungeon-tab.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DungeonTabComponent {
  public dungeonSimulator = inject(DungeonSimulator);
  public sessionManager = inject(SessionManager);
  public warningService = inject(WarningService);
  public warnings = this.warningService.dungeonWarnings;

  readonly dungeonTypeToColorMap: Record<DungeonTypeEnum, string | undefined> =
    {
      [DungeonTypeEnum.Default]: 'text-yellow-50',
      [DungeonTypeEnum.Shadow]: 'text-purple-300',
      [DungeonTypeEnum.Class]: 'text-indigo-300',
      [DungeonTypeEnum.Twister]: 'text-indigo-200',
      [DungeonTypeEnum.Tower]: 'text-amber-200',
      [DungeonTypeEnum.LoopOfIdols]: 'text-rose-300',
      [DungeonTypeEnum.Sandstorm]: 'text-yellow-200',
    };

  private readonly simulationInput = computed(() => ({
    fightable: this.sessionManager.fightable(),
    dungeons: this.sessionManager.current()?.dungeons,
    manualTrigger: this.manualTrigger(),
  }));

  private readonly manualTrigger = signal(0);

  triggerSimulation() {
    this.manualTrigger.update(t => t + 1);
  }

  private readonly simulation$ = toObservable(this.simulationInput).pipe(
    distinctUntilChanged((a, b) =>
      a.fightable === b.fightable &&
      a.dungeons === b.dungeons &&
      a.manualTrigger === b.manualTrigger
    ),

    debounceWithMaxWait(500, 5000),

    switchMap(({ fightable, dungeons }) => {
      if (!fightable || !dungeons) {
        return of({
          status: 'idle' as const,
          data: null
        });
      }

      return this.dungeonSimulator
        .simulateOpenDungeons(fightable, dungeons)
        .pipe(
          map(response => ({
            status: 'success' as const,
            data: [...response].sort((a, b) =>
              a.dungeonMetadata.experience * a.winRatio >
                b.dungeonMetadata.experience * b.winRatio ||
                (b.winRatio <= 5 && a.winRatio > b.winRatio * 10)
                ? -1
                : 1
            )
          })),

          catchError((error: unknown) => of({
            status: 'error' as const,
            data: null,
            message: this.getSimulationErrorMessage(error)
          })),

          startWith({
            status: 'loading' as const,
            data: null
          })
        );
    })
  );

  readonly simulation = toSignal(this.simulation$, {
    initialValue: {
      status: 'idle' as const,
      data: null
    }
  });

  readonly isSimulating = computed(
    () => this.simulation().status === 'loading'
  );

  readonly simulationResult = computed(
    () => this.simulation().data
  );

  readonly simulationError = computed(() => {
    const simulation = this.simulation();
    return simulation.status === 'error' ? simulation.message : null;
  });

  private getSimulationErrorMessage(error: unknown): string {
    if (error instanceof HttpErrorResponse) {
      switch (error.status) {
        case 0:
          return 'Could not reach the simulation server. Check your connection and try again.';
        case 400:
          return 'The character data could not be processed. Try relogging to your character again.';
        case 408:
        case 504:
          return 'The simulation timed out. Please try again.';
        case 429:
          return 'Too many simulation requests. Please wait a moment and try again.';
      }

      if (error.status >= 500) {
        return 'The simulation server is temporarily unavailable. Please try again shortly.';
      }
    }

    return 'Something went wrong while simulating dungeons. Please try again.';
  }
}
