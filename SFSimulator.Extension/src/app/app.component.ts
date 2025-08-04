import { ChangeDetectionStrategy, Component, ViewEncapsulation, computed, inject, viewChild } from '@angular/core';
import {
  HlmAccordionContentComponent,
  HlmAccordionDirective,
  HlmAccordionItemDirective,
  HlmAccordionTriggerDirective,
} from '@spartan-ng/helm/accordion';
import { SFGameRequest } from './sfgame/SFGameModels';
import { TabBtnComponent } from './tab-btn/tab-btn.component';
import { SFGameRequestHandler } from './services/SFGameRequestHandler';
import { DungeonSimulator } from './services/DungeonSimulator';
import { SessionManager } from './services/SessionManager';
import { DungeonTabComponent } from './tabs/dungeon-tab/dungeon-tab.component';
import { ExpeditionTabComponent } from './tabs/expedition-tab/expedition-tab.component';

@Component({
  selector: 'x-tool',
  imports: [
    HlmAccordionDirective,
    HlmAccordionItemDirective,
    HlmAccordionTriggerDirective,
    HlmAccordionContentComponent,
    DungeonTabComponent,
    ExpeditionTabComponent,
    TabBtnComponent,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  encapsulation: ViewEncapsulation.ShadowDom,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppComponent {
  public dungeonTab = viewChild(DungeonTabComponent);

  public tracker = inject(SFGameRequestHandler);
  public dungeonSimulator = inject(DungeonSimulator);
  public sessionManager = inject(SessionManager);


  public loggedInAs = computed(() => this.sessionManager.current()?.playerName);
  private last = Promise.resolve();

  constructor() {
    document.addEventListener('SFCommand' as any, async (event: CustomEvent) => {
      this.last = this.last.then(() => this.queueCommand(event.detail)).catch((error) => {
        console.error('XTool: Error processing SFCommand - ' + error);
      });
    });
  }

  private async queueCommand(gameRequest: SFGameRequest) {
    if (await this.tracker.digestResponse(gameRequest)) {
      this.dungeonTab()?.simulateDungeons();
    }
  }
}
