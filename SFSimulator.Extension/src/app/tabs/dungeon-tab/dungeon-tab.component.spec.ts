import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { signal } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DungeonTabComponent } from './dungeon-tab.component';
import { DungeonTypeEnum } from '../../services/DungeonSimulator';
import { SessionManager } from '../../services/SessionManager';
import { WarningService } from '../../services/WarningService';
import { Fightable } from '../../sfgame/Fightable';
import { Dungeons } from '../../sfgame/SFGameModels';

describe('DungeonTabComponent simulation errors', () => {
  let fixture: ComponentFixture<DungeonTabComponent>;
  let http: HttpTestingController;
  let fightable: ReturnType<typeof signal<Fightable | null>>;

  beforeEach(async () => {
    fightable = signal({} as Fightable);

    await TestBed.configureTestingModule({
      imports: [DungeonTabComponent],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        {
          provide: SessionManager,
          useValue: {
            fightable,
            current: signal({ dungeons: {} as Dungeons }),
          },
        },
        {
          provide: WarningService,
          useValue: {
            dungeonWarnings: signal([]),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(DungeonTabComponent);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    http.verify();
  });

  async function nextRequest() {
    fixture.detectChanges();
    await fixture.whenStable();
    await new Promise(resolve => setTimeout(resolve, 550));
    fixture.detectChanges();
    return http.expectOne(request => request.url.endsWith('/api/dungeon/simulate'));
  }

  function alert(): HTMLElement | null {
    return fixture.nativeElement.querySelector('[role="alert"]');
  }

  it('shows a server failure with an icon and recovers when Retry succeeds', async () => {
    const request = await nextRequest();
    request.flush({ detail: 'Internal server details' }, { status: 503, statusText: 'Service Unavailable' });
    fixture.detectChanges();

    expect(alert()?.textContent).toContain('Simulation failed');
    expect(alert()?.textContent).toContain('temporarily unavailable');
    expect(alert()?.textContent).not.toContain('Internal server details');
    expect(alert()?.querySelector('ng-icon[name="lucideCircleAlert"]')).not.toBeNull();
    expect(fixture.nativeElement.querySelector('hlm-spinner')).toBeNull();

    alert()!.querySelector<HTMLButtonElement>('button')!.click();
    const retry = await nextRequest();

    expect(alert()).toBeNull();
    expect(fixture.nativeElement.querySelector('hlm-spinner')).not.toBeNull();
    expect(fixture.nativeElement.querySelector('button[aria-label="Refresh dungeon simulation"]').disabled).toBeTrue();

    retry.flush([{
      winRatio: 0.5,
      dungeonMetadata: {
        position: 1,
        dungeonName: 'Test dungeon',
        enemyName: 'Test enemy',
        dungeonType: DungeonTypeEnum.Default,
        class: 1,
        experience: 1000,
      },
    }]);
    fixture.detectChanges();

    expect(alert()).toBeNull();
    expect(fixture.nativeElement.querySelector('table')?.textContent).toContain('Test dungeon');
    expect(fixture.nativeElement.querySelector('hlm-spinner')).toBeNull();
  });

  it('explains connection failures', async () => {
    const request = await nextRequest();
    request.error(new ProgressEvent('error'));
    fixture.detectChanges();

    expect(alert()?.textContent).toContain('Check your connection');
  });

  it('asks users to wait when the API rate limits requests', async () => {
    const request = await nextRequest();
    request.flush(null, { status: 429, statusText: 'Too Many Requests' });
    fixture.detectChanges();

    expect(alert()?.textContent).toContain('wait a moment');
  });

  it('suggests logging in again when the API rejects character data', async () => {
    const request = await nextRequest();
    request.flush('Validation details', { status: 400, statusText: 'Bad Request' });
    fixture.detectChanges();

    expect(alert()?.textContent).toContain('Try relogging to your character again');
    expect(alert()?.textContent).not.toContain('Validation details');
  });

  it('shows a fallback for other API failures', async () => {
    const request = await nextRequest();
    request.flush(null, { status: 403, statusText: 'Forbidden' });
    fixture.detectChanges();

    expect(alert()?.textContent).toContain('Something went wrong while simulating dungeons');
  });

  it('continues simulating character changes after a failure', async () => {
    const request = await nextRequest();
    request.flush(null, { status: 500, statusText: 'Internal Server Error' });
    fixture.detectChanges();
    expect(alert()).not.toBeNull();

    fightable.set({ Level: 2 } as Fightable);
    const updated = await nextRequest();
    expect(updated.request.body.Player.Level).toBe(2);
    expect(alert()).toBeNull();

    updated.flush([]);
    fixture.detectChanges();
    expect(fixture.componentInstance.simulation().status).toBe('success');
  });

  it('does not show an API error or make a request without character data', async () => {
    fightable.set(null);
    fixture.detectChanges();
    await fixture.whenStable();
    await new Promise(resolve => setTimeout(resolve, 550));
    fixture.detectChanges();

    expect(fixture.componentInstance.simulation().status).toBe('idle');
    expect(alert()).toBeNull();
    http.expectNone(request => request.url.endsWith('/api/dungeon/simulate'));
  });
});
