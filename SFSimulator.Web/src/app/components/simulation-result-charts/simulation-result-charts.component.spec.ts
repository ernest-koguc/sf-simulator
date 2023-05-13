import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SimulationResultChartsComponent } from './simulation-result-charts.component';

describe('SimulationResultChartsComponent', () => {
  let component: SimulationResultChartsComponent;
  let fixture: ComponentFixture<SimulationResultChartsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SimulationResultChartsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SimulationResultChartsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
