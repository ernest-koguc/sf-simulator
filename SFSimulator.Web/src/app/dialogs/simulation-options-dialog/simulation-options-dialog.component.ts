import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-simulation-options-dialog',
  templateUrl: './simulation-options-dialog.component.html',
  styleUrls: ['./simulation-options-dialog.component.scss']
})
export class SimulationOptionsDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<SimulationOptionsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: SimulationType,
  ) { }

  private daySimulationType = { value: 'Days', tooltip: 'Simulate until X days passes (max 3000 days)' };
  private levelSimulationType = { value: 'Level', tooltip: 'Simulate until X level is reached (max 800 level)' };

  public simulationTypes = [this.daySimulationType, this.levelSimulationType];
  simulationType = 'Days';
  simulateUntil = 0;
  maxValue = 3000;
}

export interface SimulationType {
  simulationType: 'Days' | 'Level';
  simulateUntil: number;
}
