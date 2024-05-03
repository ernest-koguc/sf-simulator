import { SavedSimulationSnapshot, SimulationSnapshotTableRecord } from "../models/simulation-snapshot";

export function mapToSimulationSnapshotTableRecord(data: SavedSimulationSnapshot): SimulationSnapshotTableRecord {
  return { ...data, chartsEnabled: false, avgBaseStatChart: null, totalBaseStatChart: null, avgXPChart: null, totalXPChart: null }
}

export function mapToLowerCase(data: any) {
    var key, keys = Object.keys(data);
    var n = keys.length;
    var mappedData: any = {};
    while (n--) {
      key = keys[n][0].toLowerCase();
      var lowerCaseName = key + keys[n].substring(1);
      let property = data[keys[n]];
      if (property !== null && typeof property !== 'string' && Object.keys(property).length) {
        mappedData[lowerCaseName] = mapToLowerCase(property);
      }
      else {
        mappedData[lowerCaseName] = property;
      }
    }

    return mappedData;
}
