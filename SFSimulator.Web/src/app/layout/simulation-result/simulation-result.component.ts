import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { RemoveRecordDialogComponent } from '../../dialogs/remove-record-dialog/remove-record-dialog.component';
import { ChartConfig } from '../../models/chart';
import { BaseStatGain, BaseStatKeys, Character, ExperienceGain, ExperienceKeys } from '../../models/simulation-result';
import { ChartService } from '../../services/chart.service';
import { DataBaseService } from '../../services/database.service';

@Component({
  selector: 'app-simulation-result',
  templateUrl: './simulation-result.component.html',
  styleUrls: ['./simulation-result.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '410px' })),
      state('charts', style({ height: '640px'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
      transition('expanded <=> charts', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
      transition('charts => collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)'))
    ]),
  ],
})
export class SimulationResultComponent {
  constructor(public chartService: ChartService, private databaseService: DataBaseService, private matDialog: MatDialog) {
    this.databaseService.getAllSimulationSnapshot().subscribe(data => {
      if (data) {
        this.dataSource = data;
        return;
      }
      this.dataSource = [];
    });
  }

  avgBaseStatChart: ChartConfig | null = null;
  avgXPChart: ChartConfig | null = null;
  totalBaseStatChart: ChartConfig | null = null;
  totalXPChart: ChartConfig | null = null;

  dataSource: SimulationSnapshot[] = [];

  columnsToDisplay = [
    { header: 'Character Name', column: 'characterName' },
    { header: 'Start', column: 'startDate'},
    { header: 'End', column: 'endDate'},
    { header: 'Days Passed', column: 'daysPassed'},
    { header: 'Level Difference', column: 'levelDifference'},
    { header: 'Base Stat Difference', column: 'baseStatDifference' }]
  columnsToDisplayWithExpand = [...this.columnsToDisplay.map(o => o.column), 'expand'];
  expandedElement!: SimulationSnapshot | null;
  baseStatKeys = BaseStatKeys;
  experienceKeys = ExperienceKeys;

  private getChartData() {
    this.avgBaseStatChart = this.chartService.createChart(this.expandedElement!.avgBaseStatGain, 'Average Base Stat', false, 'black', 'black');
    this.totalBaseStatChart = this.chartService.createChart(this.expandedElement!.totalBaseStatGain, 'Total Base Stat', false, 'black', 'black');
    this.avgXPChart = this.chartService.createChart(this.expandedElement!.avgXPGain, 'Average Experience', false, 'black', 'black');
    this.totalXPChart = this.chartService.createChart(this.expandedElement!.totalXPGain, 'Total Experience', false, 'black', 'black');
  }

  public showRemoveItemDialog(element?: SimulationSnapshot) {
    if (element) {
      this.databaseService.removeSimulationSnapshot(element);
      this.dataSource = this.dataSource.filter(e => e != element);
      return
    }

    this.matDialog.open(RemoveRecordDialogComponent, { autoFocus: false, restoreFocus: false }).afterClosed().subscribe(v => {
      if (v !== true)
        return;

      this.databaseService.removeAllSimulationSnapshots();
      this.dataSource = [];

    });
  }

  onElementClick(element: any) {
    this.expandedElement = this.expandedElement === element ? null : element;
    this.chartsEnabled = false;
    if (this.expandedElement) {
      this.getChartData();
    }
  }

  public chartsEnabled = false;

  toggleCharts() {
    this.chartsEnabled = !this.chartsEnabled;
  }
}
export interface ExpandedElement {
  element: SimulationSnapshot | null,
  avgBaseStatChart: ChartConfig | null,
  avgXPChart: ChartConfig | null;
  totalBaseStatChart: ChartConfig | null,
  totalXPChart: ChartConfig | null
}
export interface SimulationSnapshot {
  timestamp: number;
  characterName: string;
  startDate: string;
  endDate: string;
  daysPassed: number;
  levelDifference: string;
  baseStatDifference: string;
  description: string;
  characterBeforeSimulation: Character;
  characterAfterSimulation: Character;
  avgBaseStatGain: BaseStatGain;
  avgXPGain: ExperienceGain;
  totalBaseStatGain: BaseStatGain;
  totalXPGain: ExperienceGain;
}
