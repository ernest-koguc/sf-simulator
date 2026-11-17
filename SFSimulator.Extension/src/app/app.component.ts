/// <reference types="chrome" />
import { Attribute, ChangeDetectionStrategy, Component, DestroyRef, ElementRef, ViewEncapsulation, afterNextRender, computed, inject, signal, viewChild } from '@angular/core';
import { HlmBadge } from '@spartan-ng/helm/badge';
import manifest from '../../extension/manifest.json';
import {
  HlmAccordionContent,
  HlmAccordion,
  HlmAccordionItem,
  HlmAccordionTrigger,
} from '@spartan-ng/helm/accordion';
import { SFGameRequest } from './sfgame/SFGameModels';
import { SFGameRequestHandler } from './services/SFGameRequestHandler';
import { DungeonSimulator } from './services/DungeonSimulator';
import { SessionManager } from './services/SessionManager';
import { SettingsService, WindowPosition } from './services/SettingsService';
import { DungeonTabComponent } from './tabs/dungeon-tab/dungeon-tab.component';
import { DailiesTabComponent } from './tabs/dailies-tab/dailies-tab.component';
import { lucideHistory, lucideHome, lucideSettings } from '@ng-icons/lucide';
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

@Component({
  selector: 'sf-lab',
  imports: [
    SfLabLogo,
    NgIcon,
    HlmBadge,
    HlmButtonImports,
    HlmTabsImports,
    DungeonTabComponent,
    DailiesTabComponent,
    CdkDrag,
    CdkDragHandle
  ],
  providers: [provideIcons({ lucideSettings, lucideHome, lucideHistory })],
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

  private readonly windowDrag = viewChild(CdkDrag);
  private readonly availableSpace = signal<WindowPosition>({ x: 0, y: 0 });
  private readonly destroyRef = inject(DestroyRef);

  public readonly dragPosition = computed(() => {
    const position = this.settings.position();
    const space = this.availableSpace();

    return {
      x: (position?.x ?? 1) * space.x,
      y: (position?.y ?? 0) * space.y,
    };
  });

  public loggedInAs = computed(() => this.sessionManager.current()?.playerName);
  private last = Promise.resolve();

  constructor() {
    afterNextRender(() => this.observeWindowSize());

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

    const space = {
      x: Math.max(0, boundary.clientWidth - element.offsetWidth),
      y: Math.max(0, boundary.clientHeight - element.offsetHeight),
    };
    const current = this.settings.position();

    this.availableSpace.set(space);
    this.settings.setPosition({
      x: space.x > 0 ? position.x / space.x : current?.x ?? 1,
      y: space.y > 0 ? position.y / space.y : current?.y ?? 0,
    });
  }

  private observeWindowSize(): void {
    const element = this.windowDrag()?.getRootElement();
    const boundary = element?.parentElement;

    if (!element || !boundary) {
      return;
    }

    const updateSize = () => {
      if (boundary.clientWidth === 0 || boundary.clientHeight === 0) {
        return;
      }

      const space = {
        x: Math.max(0, boundary.clientWidth - element.offsetWidth),
        y: Math.max(0, boundary.clientHeight - element.offsetHeight),
      };

      this.availableSpace.set(space);
      this.settings.initializePosition({ x: space.x - 24, y: 80 }, space);
    };

    const observer = new ResizeObserver(updateSize);
    observer.observe(boundary);
    this.destroyRef.onDestroy(() => observer.disconnect());
    updateSize();
  }

  private async queueCommand(gameRequest: SFGameRequest) {
    if (await this.tracker.digestResponse(gameRequest)) {
      this.dungeonTab()?.simulateDungeons();
    }
  }
}
