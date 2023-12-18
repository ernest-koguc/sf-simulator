import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { simulateUntilValidator } from '../../helpers/validators';
import { SimulationType } from '../../models/simulation-options';

@Component({
  selector: 'app-simulation-options-dialog',
  templateUrl: './simulation-options-dialog.component.html',
  styleUrls: ['./simulation-options-dialog.component.scss']
})
export class SimulationOptionsDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<SimulationOptionsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { level: number, baseStats: number }) {

    this.form.simulateUntil.setValidators([simulateUntilValidator(data.level, data.baseStats), Validators.required]);
    this.simulationOptions.markAllAsTouched();
    this.form.simulationType.valueChanges.subscribe(() => {
      this.form.simulateUntil.updateValueAndValidity();
    });
  }

  get form(): { simulationType: FormControl<SimulationType | null>, simulateUntil: FormControl<number | null> } {
    return this.simulationOptions.controls;
  }

  get isValid(): boolean {
    return this.form.simulateUntil.valid;
  }

  public simulationOptions = new FormGroup({
    simulationType: new FormControl<SimulationType>(SimulationType.UntilDays, [Validators.required]),
    simulateUntil: new FormControl<number>(1)
  });
  private daySimulationType = { value: SimulationType.UntilDays, name: 'Days', tooltip: 'Simulate until X days passes (max 3000 days)' };
  private levelSimulationType = { value: SimulationType.UntilLevel, name: 'Level', tooltip: 'Simulate until X level is reached (max 800 level)' };
  private baseStatsSimulationType = { value: SimulationType.UntilBaseStats, name: 'Base Stats', tooltip: 'Simulate until X base stats are reached (max 1 mln base stats)' };

  public simulationTypes = [this.daySimulationType, this.levelSimulationType, this.baseStatsSimulationType];

}


