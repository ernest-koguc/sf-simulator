import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SFSimulatorComponent } from './sfsimulator.component';

describe('SFSimulatorComponent', () => {
  let component: SFSimulatorComponent;
  let fixture: ComponentFixture<SFSimulatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SFSimulatorComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(SFSimulatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
