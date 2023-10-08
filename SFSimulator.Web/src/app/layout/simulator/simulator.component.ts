import { Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { finalize } from 'rxjs';
import { environment } from '../../../environments/environment';
import { SimulationResultComponent, SimulationResultComponentData } from '../../components/simulation-result/simulation-result.component';
import { SimulationConfig } from '../../components/simulation-config/simulation-config.component';
import { CreditsDialogComponent } from '../../dialogs/credits-dialog/credits-dialog.component';
import { SftoolsloginComponent } from '../../dialogs/sftools-login-dialog/sftoolslogin.component';
import { SimulationOptionsDialogComponent } from '../../dialogs/simulation-options-dialog/simulation-options-dialog.component';
import { SimulationConfigForm } from '../../models/simulation-configuration';
import { DataBaseService } from '../../services/database.service';
import { SimulatorService } from '../../services/simulator.service';
import { SnackbarService } from '../../services/snackbar.service';

@Component({
  selector: 'app-simulator',
  templateUrl: './simulator.component.html',
  styleUrls: ['./simulator.component.scss']
})
export class SimulatorComponent {
  @ViewChild(SimulationConfig)
  simulationConfigComponent!: SimulationConfig;

  @ViewChild(SimulationResultComponent)
  simulationResultComponent!: SimulationResultComponent;

  constructor(private simulatorService: SimulatorService, private dataBaseService: DataBaseService, private dialog: MatDialog, private snackbar: SnackbarService) { }

  public simulationConfig?: SimulationConfigForm;

  public simulationBlocked = false;
  public isLoading = false;
  public environment = environment;
  public result?: SimulationResultComponentData;

  public saveForm(form?: SimulationConfigForm) {
    this.simulationConfig = form;
    this.simulationBlocked = form === undefined;
  }

  public loginThroughSFTools() {
    this.dialog.open(SftoolsloginComponent, { autoFocus: 'dialog', enterAnimationDuration: 200, exitAnimationDuration: 200, restoreFocus: false, width: "80%", height: "80%" }).afterClosed().subscribe(data => {
      if (data)
        this.simulationConfigComponent.loadForm(data);
    });
  }

  public showSimulationDialog() {
    if (!this.simulationConfig) {
      this.simulationConfigComponent.simulationOptions.markAllAsTouched();
      this.simulationBlocked = true;
      return;
    }

    if (this.simulationBlocked)
      return;

    const dialogRef = this.dialog.open(SimulationOptionsDialogComponent, {
      autoFocus: 'dialog', restoreFocus: false, enterAnimationDuration: 400,
      data: { level: this.simulationConfig.level }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (!result || !this.simulationConfig || this.simulationBlocked)
        return;

      this.simulationBlocked = true;
      this.isLoading = true;
      this.result = undefined;
      let characterName = this.simulationConfig.characterName;
      let characterBefore = { level: this.simulationConfig.level!, experience: this.simulationConfig.experience!, baseStat: this.simulationConfig.baseStat! };

      this.simulatorService.simulate(result, this.simulationConfig)
        .pipe(finalize(() => {
          this.simulationBlocked = false;
          this.isLoading = false;
        }))
        .subscribe(v => {
          this.result = {
            simulationResult: v,
            characterBefore: characterBefore,
            characterName: characterName
          };
      });
    });
  }

  public showCredits() {
    this.dialog.open(CreditsDialogComponent, { autoFocus: false, enterAnimationDuration: 400 })
  }
}
