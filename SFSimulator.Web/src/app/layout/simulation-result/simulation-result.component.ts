import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { RemoveRecordDialogComponent } from '../../dialogs/remove-record-dialog/remove-record-dialog.component';
import { mapToSimulationSnapshotTableRecord } from '../../helpers/mapper';
import { BaseStatKeys, ExperienceKeys } from '../../models/simulation-result';
import { SimulationSnapshotTableRecord } from '../../models/simulation-snapshot';
import { ChartService } from '../../services/chart.service';
import { DataBaseService } from '../../services/database.service';
import { SnackbarService } from '../../services/snackbar.service';

@Component({
  selector: 'app-simulation-result',
  templateUrl: './simulation-result.component.html',
  styleUrls: ['./simulation-result.component.scss']})
export class SimulationResultComponent {
  constructor(private dataBaseService: DataBaseService, private chartService: ChartService, private matDialog: MatDialog, private snackBarService: SnackbarService) {
    this.dataBaseService.getAllSimulationSnapshot().subscribe(v => {
      if (v)
        this.dataSource = v.map(s => mapToSimulationSnapshotTableRecord(s))
    });
  }

  public dataSource!: SimulationSnapshotTableRecord[];
  public baseStatKeys = BaseStatKeys;
  public baseStatWidth = 1 / this.baseStatKeys.length * 100 + "%";
  public experienceKeys = ExperienceKeys;
  public experienceWidth = 1 / this.experienceKeys.length * 100 + "%";

  public removeElement(element?: SimulationSnapshotTableRecord) {
    if (element) {
      this.dataBaseService.removeSimulationSnapshot(element);
      this.dataSource = this.dataSource.filter(e => e != element);
      this.snackBarService.createErrorSnackbar("Simulation result was removed");
      return;
    }

    this.matDialog.open(RemoveRecordDialogComponent, { autoFocus: false, restoreFocus: false }).afterClosed().subscribe(v => {
      if (v !== true)
        return;

      this.dataBaseService.removeAllSimulationSnapshots();
      this.dataSource = [];
      this.snackBarService.createErrorSnackbar("All results were removed!");

    });
  }

  showCharts(snapshot: SimulationSnapshotTableRecord) {
    snapshot.chartsEnabled = !snapshot.chartsEnabled;
    if (!snapshot.chartsEnabled)
      return;

    snapshot.avgBaseStatChart = this.chartService.createChart(snapshot.avgBaseStatGain, 'Average Base Stat');
    snapshot.totalBaseStatChart = this.chartService.createChart(snapshot.totalBaseStatGain, 'Total Base Stat');
    snapshot.avgXPChart = this.chartService.createChart(snapshot.avgXPGain, 'Average Experience');
    snapshot.totalXPChart = this.chartService.createChart(snapshot.totalXPGain, 'Total Experience');
  }


}
