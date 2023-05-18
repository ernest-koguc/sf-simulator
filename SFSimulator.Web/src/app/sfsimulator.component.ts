import { Component, ViewChild } from '@angular/core';
import { SimulationOptionsForm } from './dto/simulation-options';
import { SimulationResult } from './dto/simulation-result';
import { ngIfVerticalSlide } from './animation/slide-animation';
import { SimulatorService } from './services/simulator.service';
import { MatDialog } from '@angular/material/dialog';
import { SimulationOptionsDialogComponent, SimulationType } from './dialogs/simulation-options-dialog/simulation-options-dialog.component';
import { ProgressBarComponent } from './dialogs/progress-bar/progress-bar.component';
import { ConfigurationLoaderService } from './services/configuration-loader.service';
import { SimulationConfig } from './components/simulation-config/simulation-config.component';
import { SftoolsloginComponent } from './dialogs/sftoolslogin/sftoolslogin.component';
import { finalize } from 'rxjs';
import { SnackbarService } from './services/snackbar.service';

@Component({
  selector: 'sfsimulator',
  templateUrl: './sfsimulator.component.html',
  styleUrls: ['./sfsimulator.component.scss'],
  animations: [ngIfVerticalSlide]
})
export class SFSimulatorComponent {
  @ViewChild(SimulationConfig)
  simulationConfigComponent!: SimulationConfig;

  constructor(private simulatorService: SimulatorService, private configurationLoader: ConfigurationLoaderService, private dialog: MatDialog, private snackbar: SnackbarService) { }

  simulationConfig?: SimulationOptionsForm;

  public simulationResult?: SimulationResult;

  simulationType: SimulationType = { simulateUntil: 1, simulationType: 'Days' }

  public simulationBlocked = false;

  saveForm(form?: SimulationOptionsForm) {
    this.simulationConfig = form;
  }

  saveConfiguration() {
    this.configurationLoader.saveToStorage("data", JSON.stringify(this.simulationConfigComponent.simulationOptions.getRawValue()))
  }
  loginThroughSFTools() {
    this.dialog.open(SftoolsloginComponent, { autoFocus: 'dialog', restoreFocus: false, width: "80%", height: "80%" }).afterClosed().subscribe(data => {
      if (data)
        this.simulationConfigComponent.loadForm(data);
    });
  }

  showDialog() {
    console.log(this.simulationConfig);
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
