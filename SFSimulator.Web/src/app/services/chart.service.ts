import { Injectable } from '@angular/core';
import { Chart, ChartConfiguration, ChartDataset } from 'chart.js';
import { ChartConfig, ChartType } from '../models/chart';
import { BaseStatGain, ExperienceGain } from '../models/simulation-result';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import ChartDataLabels from 'chartjs-plugin-datalabels';

Chart.register(ChartDataLabels);

@Injectable({
  providedIn: 'root'
})
export class ChartService {

  constructor() {}

  public createChart(data: (BaseStatGain | ExperienceGain), title?: string, isAnimated?: false, dataLabelColor?: string, labelColor?: string): ChartConfig | null
  {
    var clonedData = JSON.parse(JSON.stringify(data));

    var sets: ChartDataset<'bar'>[];
    sets = this.getDataSets(clonedData);

    var chart: ChartConfig = {
      data: {
        labels: [""],
        datasets: sets
      },
      displayLegend: true,
      plugins: [pluginDataLabels],
      options: this.getChartOptions(title, isAnimated, dataLabelColor, labelColor)
      }

    return chart;
  }

  private getDataSets(gains: ExperienceGain | BaseStatGain): ChartDataset<'bar'>[]{

    var dataSets: ChartDataset<'bar'>[] = [];

    var k: keyof typeof gains;
    for (k in gains) {
      var value = gains[k];

      var dataSet: ChartDataset<'bar'> = {
        data: [value],
        label: k.replace("_", " "),
        backgroundColor: getDataSetColor(k),
        }
      dataSets.push(dataSet);
    }

    return dataSets;
  }

  private getChartOptions(title?: string, isAnimated?: false, dataLabelColor?: string, labelColor?: string): ChartConfiguration<'bar'>['options'] {
    var options: ChartConfiguration<'bar'>['options'] = {
      indexAxis: "x",
      animation: isAnimated,
      plugins:
      {
        datalabels: {
          anchor: 'end',
          color: dataLabelColor,
          align: 'top',
          formatter: (v, _) => {
            if (v >= 1000000000) {
              var value = (v / 1000000000).toFixed(0) + "B";
              return value;
            }

            if (v >= 1000000) {
              var value = (v / 1000000).toFixed(0) + "M";
                return value;
            }

            if (v >= 1000) {
              var value = (v/1000).toFixed(0) + "K";
              return value;
            }

            return v.toFixed(0);
          },
          font: {
            weight: 'bold',
            size: 12
          }
        },
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
          position: 'top',
          labels: {
            color: labelColor ? labelColor : 'white',
            padding: 8
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
}
export function getDataSetColor(gainType: keyof ExperienceGain | keyof BaseStatGain | 'TOTAL'): string {
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
