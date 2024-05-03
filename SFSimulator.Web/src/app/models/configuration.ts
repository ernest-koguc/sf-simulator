import { SimulationConfigForm } from "./simulation-configuration";

export class SavedConfiguration {
  constructor(name: string, simulationOptions: SimulationConfigForm) {
    this.name = name;
    this.timestamp = Date.now();
    updateSavedConfiguration(this, simulationOptions);
  }

  public timestamp: number;
  public name: string;
  public scheduleId!: number | 'Default';
  public form!: SimulationConfigForm;
}

export function updateSavedConfiguration(savedConfig: SavedConfiguration, simulationOptions: SimulationConfigForm) {
    if (simulationOptions.playstyle.schedule)
      savedConfig.scheduleId = simulationOptions.playstyle.schedule.timestamp;
    else
      savedConfig.scheduleId = 'Default';

    let form = JSON.parse(JSON.stringify(simulationOptions));
    delete form.playstyle.schedule;
    savedConfig.form = form;
}
