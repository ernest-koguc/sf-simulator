import { Component, Input } from '@angular/core';
import { SimulatedDay } from '../../dto/simulation-result';
import { SlideInOutHorizontallyAnimation, SlideInOutVericallyAnimation } from '../../animation/slide-animation';
import { Chart, ChartType } from '../../dto/chart';
import { ChartServiceService } from '../../services/chart-service.service';

@Component({
  selector: 'simulation-result-charts',
  templateUrl: './simulation-result-charts.component.html',
  styleUrls: ['./simulation-result-charts.component.scss'],
  animations: [SlideInOutHorizontallyAnimation, SlideInOutVericallyAnimation]
})
export class SimulationResultChartsComponent{

  constructor(private chartService: ChartServiceService) { }

  public chartTypes: string[] = ['Total', 'Daily', 'Average'];

  private _baseStatChartCurrentDay: number = 1;
  private _xpChartCurrentDay: number = 1;

  public lastDay: number = 1;

  public chartVisible: boolean = false;

  public baseStatSliderVisibility: 'hidden' | 'visible' = 'hidden';
  private _baseStatChartType: ChartType = 'Total';

  public xpSliderVisibility: 'hidden' | 'visible' = 'hidden';
  private _xpChartType: ChartType = 'Total';
  public baseStatChart: Chart | null = null;
  public xpChart: Chart | null = null;

  private _simulatedDays?: SimulatedDay[];

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

  @Input()
  set simulatedDays(value: SimulatedDay[] | undefined) {
    if (!value)
      return;

    this._simulatedDays = value;
    this._baseStatChartCurrentDay = 1;
    this._xpChartCurrentDay = 1;
    this.lastDay = value?.length;
    this.createCharts('XP', 'Total');
    this.createCharts('Base Stat', 'Total');
    this.chartVisible = true;
  }

  public createCharts(gainType: 'XP' | 'Base Stat', chartType: ChartType) {
    if (!this._simulatedDays)
      return;

    var title = chartType + ' ' + gainType + ' Gains';

    if (gainType == 'Base Stat') {
      if (this.baseStatChartType == 'Daily')
        this.baseStatChart = this.chartService.createChart(this._simulatedDays[this.baseStatChartCurrentDay - 1].baseStatGain, this.baseStatChartType, title);
      else if (this.baseStatChartType == 'Average')
        this.baseStatChart = this.chartService.createChart(this._simulatedDays.map(d => d.baseStatGain), this.baseStatChartType, title);
      else
        this.baseStatChart = this.chartService.createChart(this._simulatedDays.map(d => d.baseStatGain), this.baseStatChartType, title);
    }
    else {
      if (this.xpChartType == 'Daily')
        this.xpChart = this.chartService.createChart(this._simulatedDays[this.xpChartCurrentDay - 1].experienceGain, this.xpChartType, title);
      else if (this.xpChartType == 'Average')
        this.xpChart = this.chartService.createChart(this._simulatedDays.map(d => d.experienceGain), this.xpChartType, title); 
      else
        this.xpChart = this.chartService.createChart(this._simulatedDays.map(d => d.experienceGain), this.xpChartType, title);
    }
  }
}
