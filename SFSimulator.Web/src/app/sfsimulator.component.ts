import { Component, ViewChild } from '@angular/core';
import { SimulationOptionsForm } from './dto/simulation-options';
import { SimulationResult } from './dto/simulation-result';
import { ngIfVerticalSlide } from './animation/slide-animation';
import { SimulatorService } from './services/simulator/simulator.service';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { SimulationOptionsDialogComponent, SimulationType } from './dialogs/simulation-options-dialog/simulation-options-dialog.component';
import { ProgressBarComponent } from './dialogs/progress-bar/progress-bar.component';
import { ConfigurationLoaderService } from './services/configuration-loader/configuration-loader.service';
import { SimulationConfig } from './components/simulation-config/simulation-config.component';
import { DomSanitizer } from '@angular/platform-browser';
import { SftoolsloginComponent } from './dialogs/sftoolslogin/sftoolslogin.component';

@Component({
  selector: 'sfsimulator',
  templateUrl: './sfsimulator.component.html',
  styleUrls: ['./sfsimulator.component.scss'],
  animations: [ngIfVerticalSlide]
})
export class SFSimulatorComponent {
  @ViewChild(SimulationConfig)
  simulationConfigComponent!: SimulationConfig;

  constructor(private simulatorService: SimulatorService, private configurationLoader: ConfigurationLoaderService, private dialog: MatDialog, private sanitizer: DomSanitizer) { }

  simulationConfig?: SimulationOptionsForm;

  private simulationBlocked = false;

  public simulationResult?: SimulationResult;

  saveForm(form?: SimulationOptionsForm) {
    this.simulationConfig = form;
  }

  simulationType: SimulationType = { simulateUntil: 1, simulationType: 'Days' }

  saveConfiguration() {
    this.configurationLoader.saveToStorage("data", JSON.stringify(this.simulationConfigComponent.simulationOptions.getRawValue()))
  }
  loginThroughSFTools() {
    this.dialog.open(SftoolsloginComponent, { autoFocus: 'dialog', width: "80%", height: "80%" }).afterClosed().subscribe(data => {
      if (data)
        this.loadConfiguration(data);
    });
  }
  private loadConfiguration(data: any) {
    this.simulationConfigComponent.loadForm(data);
  }

  showDialog() {
    if (!this.simulationConfig || this.simulationBlocked)
      return;

    const dialogRef = this.dialog.open(SimulationOptionsDialogComponent, { autoFocus: 'dialog', enterAnimationDuration: 400, data: this.simulationType });
    dialogRef.afterClosed().subscribe(result => {
      if (result != null && this.simulationConfig) {
        this.simulationType = result;

        if (!this.simulationBlocked) {
          this.simulationBlocked = true;
          const progressBar = this.dialog.open(ProgressBarComponent, { autoFocus: 'dialog', disableClose: true, enterAnimationDuration: 400 });
          this.simulatorService.simulate(this.simulationType, this.simulationConfig).subscribe(v => {
            this.simulationResult = v
            this.simulationBlocked = false;
            progressBar.close();
          }, _ => {
            this.simulationBlocked = false;
            progressBar.close();
          });
        }
      }
    });
  }
}
