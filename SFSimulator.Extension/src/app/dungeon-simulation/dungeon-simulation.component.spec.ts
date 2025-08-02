import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DungeonSimulationComponent } from './dungeon-simulation.component';

describe('DungeonSimulationComponent', () => {
  let component: DungeonSimulationComponent;
  let fixture: ComponentFixture<DungeonSimulationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DungeonSimulationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DungeonSimulationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
