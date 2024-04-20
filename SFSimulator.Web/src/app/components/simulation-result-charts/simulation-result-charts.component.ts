import { Component, Input } from '@angular/core';
import { SlideInOutHorizontallyAnimation } from '../../animation/slide-animation';
import { ChartConfig, ChartType } from '../../models/chart';
import { SimulationResult } from '../../models/simulation-result';
import { ChartService } from '../../services/chart.service';

@Component({
  selector: 'simulation-result-charts',
  templateUrl: './simulation-result-charts.component.html',
  styleUrls: ['./simulation-result-charts.component.scss'],
  animations: [SlideInOutHorizontallyAnimation]
})
export class SimulationResultChartsComponent{

  constructor(private chartService: ChartService) { }

  @Input()
  set simulationResult(value: SimulationResult | undefined) {
    if (!value)
      return;

    this._simulationResult = value;
    this._baseStatChartCurrentDay = 1;
    this._xpChartCurrentDay = 1;
    this.lastDay = value.days;
    this.createCharts('XP', 'Total');
    this.createCharts('Base Stat', 'Total');
  }

  public chartTypes: string[] = ['Total', 'Daily', 'Average'];
  public lastDay: number = 1;
  public baseStatSliderVisibility: 'hidden' | 'visible' = 'hidden';
  public xpSliderVisibility: 'hidden' | 'visible' = 'hidden';
  public baseStatChart: ChartConfig | null = null;
  public xpChart: ChartConfig | null = null;

  private _baseStatChartType: ChartType = 'Total';
  private _xpChartType: ChartType = 'Total';
  private _baseStatChartCurrentDay: number = 1;
  private _xpChartCurrentDay: number = 1;
  private _simulationResult?: SimulationResult;


  set baseStatChartType(value: ChartType) {
    this._baseStatChartType = value;
    this.baseStatSliderVisibility = value == 'Daily' ? 'visible' : 'hidden';
    this.createCharts('Base Stat', value);
  }
  get baseStatChartType() {
    return this._baseStatChartType;
  } 

  set xpChartType(value: ChartType) {
    this._xpChartType = value;
    this.xpSliderVisibility = value == 'Daily' ? 'visible' : 'hidden';
    this.createCharts('XP', value);
  }
  get xpChartType() {
    return this._xpChartType;
  }
  set xpChartCurrentDay(value: number) {
    this._xpChartCurrentDay = value;
    this.createCharts('XP', 'Daily');
  }
  set baseStatChartCurrentDay(value: number) {
    this._baseStatChartCurrentDay = value;
    this.createCharts('Base Stat', 'Daily');
  }
  get xpChartCurrentDay() {
    return this._xpChartCurrentDay;
  }
  get baseStatChartCurrentDay() {
    return this._baseStatChartCurrentDay;
  }


  public createCharts(gainType: 'XP' | 'Base Stat', chartType: ChartType) {
    if (!this._simulationResult)
      return;

    var title = chartType + ' ' + gainType + ' Gains';

    if (gainType == 'Base Stat') {
      if (this.baseStatChartType == 'Daily')
        this.baseStatChart = this.chartService.createChart(this._simulationResult.simulatedDays[this.baseStatChartCurrentDay - 1].baseStatGain, title, undefined, 'white');
      else if (this.baseStatChartType == 'Average')
        this.baseStatChart = this.chartService.createChart(this._simulationResult.averageGains.baseStatGain, title, undefined, 'white');
      else
        this.baseStatChart = this.chartService.createChart(this._simulationResult.totalGains.baseStatGain, title, undefined, 'white');
    }
    else {
      if (this.xpChartType == 'Daily')
        this.xpChart = this.chartService.createChart(this._simulationResult.simulatedDays[this.xpChartCurrentDay - 1].experienceGain, title, undefined, 'white');
      else if (this.xpChartType == 'Average')
        this.xpChart = this.chartService.createChart(this._simulationResult.averageGains.experienceGain, title, undefined, 'white'); 
      else
        this.xpChart = this.chartService.createChart(this._simulationResult.totalGains.experienceGain, title, undefined, 'white');
    }
  }
}
