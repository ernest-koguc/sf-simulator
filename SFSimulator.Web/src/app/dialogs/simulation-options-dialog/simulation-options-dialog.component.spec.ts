import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SimulationOptionsDialogComponent } from './simulation-options-dialog.component';

describe('SimulationOptionsDialogComponent', () => {
  let component: SimulationOptionsDialogComponent;
  let fixture: ComponentFixture<SimulationOptionsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SimulationOptionsDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SimulationOptionsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
