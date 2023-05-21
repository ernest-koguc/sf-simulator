import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { addDays } from '../helpers/date-helper';
import { mapToSimulationSnaphsot } from '../helpers/mapper';
import { roundValues } from '../helpers/round-values';
import { SimulationSnapshot } from '../layout/simulation-result/simulation-result.component';
import { SimulationResult } from '../models/simulation-result';

@Injectable({
  providedIn: 'root'
})
export class DataBaseService {

  constructor() { }

  // SimulationSnapshot
  public saveSimulationSnapshot(data: SimulationResult) {
    var snapshot = mapToSimulationSnaphsot(data);

    var storedItem = this.getFromStorage('SimulationSnapshotTable');
    var snapshotArray: SimulationSnapshot[];

    if (!storedItem) {
      snapshotArray = [];
    }
    else {
      snapshotArray = JSON.parse(storedItem);
    }

    snapshotArray.push(snapshot);
    var stringData = JSON.stringify(snapshotArray);
    this.saveToStorage('SimulationSnapshotTable', stringData);
  }

  public getAllSimulationSnapshot(): Observable<SimulationSnapshot[] | undefined> {
    var storedTable = this.getStoredTable<SimulationSnapshot[]>("SimulationSnapshotTable");
    return storedTable;
  }

  public removeAllSimulationSnapshots() {
    this.removeAllFromStorage("SimulationSnapshotTable");
  }

  public removeSimulationSnapshot(entity: SimulationSnapshot) {
    this.getAllSimulationSnapshot().subscribe(t => {

      if (t) {
        t = t.filter(e => e.timestamp != entity.timestamp);
        this.saveToStorage('SimulationSnapshotTable', JSON.stringify(t));
      }
    });
  }

  // Implementation

  private getStoredTable<T>(key: string): Observable<T | undefined>{
    var storedTable = this.getFromStorage(key);
    var table; 
    if (storedTable) {
      table = JSON.parse(storedTable);
    }
    else {
      table = [];
    }
    return this.objectToObserver<T>(table);
  }

  private saveToStorage(key: string, data: string) {
    localStorage.setItem(key, data);
  }

  private getFromStorage(key: string) {
    return localStorage.getItem(key);
  }

  private removeAllFromStorage(key: string) {
    localStorage.removeItem(key);
  }

  private objectToObserver<T>(storedFile: any): Observable<T> {
    return new Observable<T>(observer => {
      observer.next(storedFile);
      observer.complete();
    });
  }
}
