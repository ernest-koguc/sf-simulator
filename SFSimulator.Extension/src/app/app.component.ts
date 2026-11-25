/// <reference types="chrome" />
import { ChangeDetectionStrategy, Component, type EffectCleanupRegisterFn, ViewEncapsulation, afterRenderEffect, computed, inject, signal, untracked, viewChild } from '@angular/core';
import { HlmBadge } from '@spartan-ng/helm/badge';
import manifest from '../../extension/manifest.json';
import { SFGameRequest } from './sfgame/SFGameModels';
import { SFGameRequestHandler } from './services/SFGameRequestHandler';
import { DungeonSimulator } from './services/DungeonSimulator';
import { SessionManager } from './services/SessionManager';
import { SettingsService, WindowPosition } from './services/SettingsService';
import { DungeonTabComponent } from './tabs/dungeon-tab/dungeon-tab.component';
import { DailiesTabComponent } from './tabs/dailies-tab/dailies-tab.component';
import { lucideHistory, lucideHome, lucideMaximize2, lucideMinus, lucideSettings } from '@ng-icons/lucide';
import { provideIcons, NgIcon } from '@ng-icons/core';
import {
  CdkDrag,
  CdkDragEnd,
  CdkDragHandle,
} from '@angular/cdk/drag-drop';
import { HlmTabsImports } from '@spartan-ng/helm/tabs';
import { HlmButtonImports } from '@spartan-ng/helm/button';
import { SfLabLogo } from './sf-lab-logo/sf-lab-logo.component';
import { captureExtensionError } from './monitoring/sentry';
import { HomeTabComponent } from './tabs/home-tab/home-tab.component';
import { TabContentComponent } from './tabs/tab-content/tab-content.component';
import { LogsTabComponent } from './tabs/logs-tab/logs-tab.component';
import { SettingsTabComponent } from './tabs/settings-tab/settings-tab.component';

@Component({
  selector: 'sf-lab',
  imports: [
    SfLabLogo,
    NgIcon,
    HlmBadge,
    HlmButtonImports,
    HlmTabsImports,
    HomeTabComponent,
    DungeonTabComponent,
    DailiesTabComponent,
    LogsTabComponent,
    SettingsTabComponent,
    CdkDrag,
    CdkDragHandle,
    TabContentComponent
  ],
  providers: [provideIcons({ lucideHistory, lucideHome, lucideMaximize2, lucideMinus, lucideSettings })],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  encapsulation: ViewEncapsulation.ShadowDom,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppComponent {
  public readonly version = manifest.version;

  public dungeonTab = viewChild(DungeonTabComponent);

  public tracker = inject(SFGameRequestHandler);
  public dungeonSimulator = inject(DungeonSimulator);
  public sessionManager = inject(SessionManager);
  public readonly settings = inject(SettingsService);
  public readonly minimized = signal(true);

  private readonly windowDrag = viewChild(CdkDrag);

  public loggedInAs = computed(() => this.sessionManager.current()?.playerName);
  private last = Promise.resolve();

  constructor() {
    afterRenderEffect({
      mixedReadWrite: (onCleanup) => {
        const drag = this.windowDrag();
        if (!drag) {
          return;
        }

        if (this.minimized()) {
          drag.setFreeDragPosition({ x: 0, y: 0 });
        } else {
          this.observeWindowSize(drag, onCleanup);
        }
      },
    });

    document.addEventListener('SFCommand' as any, async (event: CustomEvent) => {
      this.last = this.last.then(() => this.queueCommand(event.detail)).catch((error) => {
        captureExtensionError(error, 'game-response');
        console.error('SFLab: Error while processing SFCommand: ' + error);
        console.error("Logging before rethrow:", error.stack);
        console.error(JSON.stringify(event.detail));
      });
    });
  }

  public savePosition(event: CdkDragEnd): void {
    const position = event.source.getFreeDragPosition();
    const element = event.source.getRootElement();
    const boundary = element.parentElement;

    if (!boundary) {
      return;
    }

    const space = this.getAvailableSpace(element, boundary);
    const current = this.settings.position();

    this.settings.setPosition({
      x: space.x > 0 ? position.x / space.x : current?.x ?? 1,
      y: space.y > 0 ? position.y / space.y : current?.y ?? 0,
    });
  }

  private observeWindowSize(drag: CdkDrag, onCleanup: EffectCleanupRegisterFn): void {
    const element = drag.getRootElement();
    const boundary = element.parentElement;

    if (!boundary) {
      return;
    }

    const updatePosition = () => {
      if (boundary.clientWidth === 0 || boundary.clientHeight === 0) {
        return;
      }

      const space = this.getAvailableSpace(element, boundary);
      this.settings.initializePosition({ x: space.x - 24, y: 80 }, space);
      const position = this.settings.position();
      drag.setFreeDragPosition({
        x: (position?.x ?? 1) * space.x,
        y: (position?.y ?? 0) * space.y,
      });
    };

    const observer = new ResizeObserver(updatePosition);
    observer.observe(boundary);
    observer.observe(element);
    onCleanup(() => observer.disconnect());
    untracked(updatePosition);
  }

  private getAvailableSpace(element: HTMLElement, boundary: HTMLElement): WindowPosition {
    return {
      x: Math.max(0, boundary.clientWidth - element.offsetWidth),
      y: Math.max(0, boundary.clientHeight - element.offsetHeight),
    };
  }

  private async queueCommand(gameRequest: SFGameRequest) {
    await this.tracker.digestResponse(gameRequest)
  }
}
