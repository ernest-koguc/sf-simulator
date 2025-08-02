import { ChangeDetectionStrategy, Component, ViewEncapsulation, computed, inject, viewChild } from '@angular/core';
import {
  HlmAccordionContentComponent,
  HlmAccordionDirective,
  HlmAccordionItemDirective,
  HlmAccordionTriggerDirective,
} from '@spartan-ng/helm/accordion';
import { SFGameModelTracker } from './sfgame-model-tracker.service';
import { DungeonSimulator } from './dungeon-simulator.service';
import { DungeonSimulationComponent } from "./dungeon-simulation/dungeon-simulation.component";
import { ExpeditionPreviewComponent } from './expedition-preview/expedition-preview.component';
import { ExpeditionService } from './services/expedition.service';
import { SFGameRequest } from './sfgame/SFGameModels';
import { TabBtnComponent } from './tab-btn/tab-btn.component';

@Component({
  selector: 'x-tool',
  imports: [
    HlmAccordionDirective,
    HlmAccordionItemDirective,
    HlmAccordionTriggerDirective,
    HlmAccordionContentComponent,
    DungeonSimulationComponent,
    ExpeditionPreviewComponent,
    TabBtnComponent,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  encapsulation: ViewEncapsulation.ShadowDom,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppComponent {
  public dungeonSimulation = viewChild(DungeonSimulationComponent);

  public tracker = inject(SFGameModelTracker);
  public dungeonSimulator = inject(DungeonSimulator);
  public expeditionService = inject(ExpeditionService);

  public loggedInAs = computed(() => this.tracker.playerName());
  private last = Promise.resolve();

  constructor() {
    document.addEventListener('SFCommand' as any, async (event: CustomEvent) => {
      this.last = this.last.then(() => this.queueCommand(event.detail));
    });
  }

  private async queueCommand(gameRequest: SFGameRequest) {
    if (await this.tracker.digestResponse(gameRequest)) {
      this.dungeonSimulation()?.simulateDungeons();
      this.expeditionService.switchPlayer(this.tracker.player()!.ID);
    }
  }
}
