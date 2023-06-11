import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { mapToSimulationSnapshot } from '../helpers/mapper';
import { SavedSchedule } from '../models/schedule';
import { SimulationResult } from '../models/simulation-result';
import { SimulationSnapshot } from '../models/simulation-snapshot';
import { UserData } from '../models/user-data';

@Injectable({
  providedIn: 'root'
})
export class DataBaseService {

  constructor() { }

  // Saved Schedule
  public saveSchedule(schedule: SavedSchedule) {
    var storedItem = this.getFromStorage('SavedScheduleTable');
    var scheduleArray: SavedSchedule[];

    if (!storedItem)
      scheduleArray = [];
    else {
      scheduleArray = JSON.parse(storedItem);
    }
    var index = scheduleArray.findIndex(v => v.timestamp == schedule.timestamp);

    if (index >= 0)
      scheduleArray[index] = schedule;
    else
      scheduleArray.push(schedule);

    var stringData = JSON.stringify(scheduleArray);
    this.saveToStorage('SavedScheduleTable', stringData);
  }

  public getAllSchedules(): Observable<SavedSchedule[] | undefined>{
    var storedTable = this.getStoredTable<SavedSchedule[]>('SavedScheduleTable');
    return storedTable;
  }

  public removeSchedule(entity: SavedSchedule) {
    this.getAllSchedules().subscribe(t => {
      if (t) {
        t = t.filter(e => e.timestamp != entity.timestamp);
        this.saveToStorage('SavedScheduleTable', JSON.stringify(t));
      }
    });
  }

  // User Data
  public getUserData(): UserData {
    var userData = this.getFromStorage("UserData");

    if (userData)
      return JSON.parse(userData);

    return { lastSeenPatchNotes: undefined };
  }

  public updateUserData(userData: UserData) {
    this.saveToStorage("UserData", JSON.stringify(userData));
  }

  // SimulationSnapshot
  public saveSimulationSnapshot(data: SimulationResult) {
    var snapshot = mapToSimulationSnapshot(data);

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
