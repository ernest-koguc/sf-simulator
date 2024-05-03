import { Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { finalize } from 'rxjs';
import { environment } from '../../../environments/environment';
import { SimulationResultComponent, SimulationResultComponentData } from '../../components/simulation-result/simulation-result.component';
import { SimulationConfig } from '../../components/simulation-config/simulation-config.component';
import { CreditsDialogComponent } from '../../dialogs/credits-dialog/credits-dialog.component';
import { SftoolsloginComponent, DataScope } from '../../dialogs/sftools-login-dialog/sftoolslogin.component';
import { SimulationOptionsDialogComponent } from '../../dialogs/simulation-options-dialog/simulation-options-dialog.component';
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

  constructor(private simulatorService: SimulatorService, private dialog: MatDialog, private snackbar: SnackbarService) { }

  public simulationBlocked = false;
  public isLoading = false;
  public environment = environment;
  public result?: SimulationResultComponentData;


  public loginThroughSFTools() {
    this.dialog.open(SftoolsloginComponent, { autoFocus: 'dialog', enterAnimationDuration: 200, exitAnimationDuration: 200, restoreFocus: false, width: "80%", height: "80%", data: DataScope.All }).afterClosed().subscribe(data => {
      if (data !== undefined && data !== null && data.error === undefined)
        this.simulationConfigComponent.loadEndpoint(data);
      else if (data.error === undefined) {
        this.snackbar.createErrorSnackbar(data.error);
        console.log(data.error);
      }
    });
  }

  public showSimulationDialog() {
    let simulationConfig = this.simulationConfigComponent.getSimulationOptions();
    if (simulationConfig === undefined) {
      return;
    }

    if (this.simulationBlocked)
      return;

    const dialogRef = this.dialog.open(SimulationOptionsDialogComponent, {
      autoFocus: 'dialog', restoreFocus: false, enterAnimationDuration: 400,
      data: { level: simulationConfig.account.level, baseStats: simulationConfig.account.baseStat }
    });
    dialogRef.afterClosed().subscribe(simulationType => {
      if (!simulationType || !simulationConfig || this.simulationBlocked)
        return;

      this.simulationBlocked = true;
      this.isLoading = true;
      this.result = undefined;
      let characterName = simulationConfig.account.characterName;
      let characterBefore = { level: simulationConfig.account.level!, experience: simulationConfig.account.experience!, baseStat: simulationConfig.account.baseStat! };

      let time = Date.now();
      this.simulatorService.simulate(simulationType, simulationConfig)
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
          let now = Date.now() - time;
          this.snackbar.createInfoSnackbar(`Simulation successful, elapsed time ${now} ms`);
      });
    });
  }

  public showCredits() {
    this.dialog.open(CreditsDialogComponent, { autoFocus: false, enterAnimationDuration: 400 })
  }
}
