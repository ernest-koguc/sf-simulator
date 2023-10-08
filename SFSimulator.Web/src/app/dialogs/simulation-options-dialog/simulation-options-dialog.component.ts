import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
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
    @Inject(MAT_DIALOG_DATA) public data: { level: number }) {

    this.form.simulateUntil.setValidators([simulateUntilValidator(data.level), Validators.required]);
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

  private daySimulationType = { value: 'Days', tooltip: 'Simulate until X days passes (max 3000 days)' };
  private levelSimulationType = { value: 'Level', tooltip: 'Simulate until X level is reached (max 800 level)' };
  public simulationOptions = new FormGroup({
    simulationType: new FormControl<SimulationType>('Days', [Validators.required]),
    simulateUntil: new FormControl<number>(1)
  });

  public simulationTypes = [this.daySimulationType, this.levelSimulationType];
}


