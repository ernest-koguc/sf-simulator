import { Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { finalize } from 'rxjs';
import { ngIfVerticalSlide } from '../../animation/slide-animation';
import { SimulationConfig } from '../../components/simulation-config/simulation-config.component';
import { ProgressBarComponent } from '../../dialogs/progress-bar-dialog/progress-bar.component';
import { SftoolsloginComponent } from '../../dialogs/sftools-login-dialog/sftoolslogin.component';
import { SimulationOptionsDialogComponent, SimulationType } from '../../dialogs/simulation-options-dialog/simulation-options-dialog.component';
import { SimulationOptionsForm } from '../../models/simulation-options';
import { SimulationResult } from '../../models/simulation-result';
import { DataBaseService } from '../../services/database.service';
import { SimulatorService } from '../../services/simulator.service';
import { SnackbarService } from '../../services/snackbar.service';

@Component({
  selector: 'app-simulator',
  templateUrl: './simulator.component.html',
  styleUrls: ['./simulator.component.scss'],
  animations: [ngIfVerticalSlide]
})
export class SimulatorComponent {
  @ViewChild(SimulationConfig)
  simulationConfigComponent!: SimulationConfig;

  constructor(private simulatorService: SimulatorService, private dataBaseService: DataBaseService, private dialog: MatDialog, private snackbar: SnackbarService) { }

  simulationConfig?: SimulationOptionsForm;

  public simulationResult?: SimulationResult;

  simulationType: SimulationType = { simulateUntil: 1, simulationType: 'Days' }

  public simulationBlocked = false;

  saveForm(form?: SimulationOptionsForm) {
    this.simulationConfig = form;
  }

  saveResult() {
    if (this.simulationResult) {
      this.dataBaseService.saveSimulationSnapshot(this.simulationResult);
      this.snackbar.createSuccessSnackBar('Saved simulation result')
    }
  }

  loginThroughSFTools() {
    this.dialog.open(SftoolsloginComponent, { autoFocus: 'dialog', enterAnimationDuration: 200, exitAnimationDuration: 200, restoreFocus: false, width: "80%", height: "80%" }).afterClosed().subscribe(data => {
      if (data)
        this.simulationConfigComponent.loadForm(data);
    });
  }

  showSimulationDialog() {
    const dialogRef = this.dialog.open(SimulationOptionsDialogComponent, { autoFocus: 'dialog', restoreFocus: false, enterAnimationDuration: 400, data: this.simulationType });
    dialogRef.afterClosed().subscribe(result => {
      if (!result || !this.simulationConfig || this.simulationBlocked)
        return;

      this.simulationType = result;
      this.simulationBlocked = true;
      var startTime = new Date().getTime();

      var progressBar = this.dialog.open(ProgressBarComponent, { autoFocus: 'dialog', disableClose: true, enterAnimationDuration: 400 });
      this.simulatorService.simulate(this.simulationType, this.simulationConfig)
        .pipe(finalize(() => {
          this.simulationBlocked = false;
          progressBar.close();
        }))
        .subscribe((v) => {
          this.simulationResult = v;
          this.snackbar.createInfoSnackbar('Simulation complete. Elapsed time: ' + (new Date().getTime() - startTime) + 'ms')
        });
    });
  }
}

