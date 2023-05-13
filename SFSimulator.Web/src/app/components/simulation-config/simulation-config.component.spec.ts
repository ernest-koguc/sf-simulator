import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SimulationConfig } from './simulation-config.component';

describe('CharacterDetailsFormComponent', () => {
  let component: SimulationConfig;
  let fixture: ComponentFixture<SimulationConfig>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SimulationConfig ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SimulationConfig);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
