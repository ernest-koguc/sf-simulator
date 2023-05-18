import { Injectable } from '@angular/core';
import { ChartConfiguration, ChartDataset } from 'chart.js';
import { Chart, ChartType } from '../dto/chart';
import { BaseStatGain, ExperienceGain } from '../dto/simulation-result';

@Injectable({
  providedIn: 'root'
})
export class ChartServiceService {

  constructor() { }

  public createChart(data: (BaseStatGain | ExperienceGain | BaseStatGain[] | ExperienceGain[]), chartType: ChartType, title: string): Chart | null
  {
    var clonedData = JSON.parse(JSON.stringify(data));

    var sets: ChartDataset<'bar'>[];

    if (Array.isArray(clonedData) && clonedData.length < 1) {
      return null;
    }

    if (Array.isArray(clonedData) && chartType == 'Total') {
      var gain = clonedData.pop();
      var k: keyof (typeof gain);

      for (k in gain) {
        clonedData.forEach(value => gain[k] += value[k]);
      }

      sets = this.getDataSets(gain);
    }
    else if (Array.isArray(clonedData) && chartType == 'Average') {
      var gain = clonedData.pop();
      var k: keyof (typeof gain);

      for (k in gain) {
        clonedData.forEach(value => gain[k] += value[k]);
        gain[k] /= clonedData.length + 1;
      }

      sets = this.getDataSets(gain);
    }
    else {
      sets = this.getDataSets(clonedData);
    }

    var chart: Chart = {
      data: {
        labels: [""],
        datasets: sets
      },
      displayLegend: true,
      plugins: [],
      options: this.getChartOptions(title)
      }

    return chart;
  }

  private getDataSets(xpGain: ExperienceGain | BaseStatGain): ChartDataset<'bar'>[]{

    var dataSets: ChartDataset<'bar'>[] = [];
    var total: number = 0;

    var k: keyof typeof xpGain;
    for (k in xpGain) {
      var value = xpGain[k];

      var dataSet: ChartDataset<'bar'> = {
        data: [value],
        label: k.replace("_", " "),
        backgroundColor: this.getDataSetColor(k)
        }
      dataSets.push(dataSet);
      total += value;
    }

    var totalDataSet: ChartDataset<'bar'> = {
      data: [total],
      label: "TOTAL",
      backgroundColor: this.getDataSetColor('TOTAL')
    }

    dataSets.push(totalDataSet);

    return dataSets;
  }

  private getChartOptions(title: string): ChartConfiguration<'bar'>['options'] {
    var options: ChartConfiguration<'bar'>['options'] = {
      indexAxis: "x",
      plugins:
      {
        tooltip:
        {
          intersect: false
        },
        title:
        {
          display: true,
          text: title,
          color: '#0088cc',
          font:
          {
            size: 14
          }
        },
        legend:
        {
          labels: {
            color: 'white'
            }
        }
      },
      scales:
      {
        x:
        {
          beginAtZero: true,
          min: 0,
          offset: true,
          ticks:
          {
            autoSkip: false,
            color: 'white',
          }
        },
        y:
        {
          beginAtZero: true,
          min: 0,
          offset: true,
          ticks:
          {
            autoSkip: false,
            color: 'white'
          }
        }
      }
    };
    return options;
  }
  private getDataSetColor(gainType: keyof ExperienceGain | keyof BaseStatGain | 'TOTAL'): string {
    switch (gainType) {
      case 'QUEST':
        return '#0bb4ff';
      case 'ARENA':
        return '#b3d4ff';
      case 'DAILY_MISSION':
        return '#50e991';
      case 'ACADEMY':
        return '#8BC34A';
      case 'TIME_MACHINE':
        return '#9b19f5';
      case 'WHEEL':
        return '#dc0ab4';
      case 'CALENDAR':
        return '#e60049';
      case 'GUARD':
        return '#00bfa0';
      case 'GOLD_PIT':
        return '#e6d800';
      case 'GEM':
        return '#3F51B5';
      case 'ITEM':
        return '#EC407A';
      case 'DICE_GAME':
        return '#8BC34A';
      case 'GUILD_FIGHT':
        return '#EC407A';
      case 'TOTAL':
        return '#ffa300';
    }
  }
}
