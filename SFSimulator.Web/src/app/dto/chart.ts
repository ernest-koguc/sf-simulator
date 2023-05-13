import { ChartConfiguration } from "chart.js";

export type Chart = {
  data: ChartConfiguration<'bar'>['data'];
  options: ChartConfiguration<'bar'>['options'];
  displayLegend: boolean;
  plugins: any[];
}
export type ChartType = 'Total' | 'Average' | 'Daily';
