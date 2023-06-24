import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SavedConfiguration } from '../models/configuration';
import { SavedSchedule } from '../models/schedule';
import { SimulationResult } from '../models/simulation-result';
import { SavedSimulationSnapshot } from '../models/simulation-snapshot';
import { UserData } from '../models/user-data';

@Injectable({
  providedIn: 'root'
})
export class DataBaseService {
  constructor() { }

  // *********************** Saved Configuration ***********************

  // Save One
  public saveConfiguration(entity: SavedConfiguration) {
    this.saveItemInTable(entity, SavedConfigurationTableKey);
  }

  // Get All
  public getAllConfigurations(): Observable<SavedConfiguration[] | undefined> {
    var storedTable = this.getStoredTable<SavedConfiguration[]>(SavedConfigurationTableKey);
    return storedTable;
  }

  // Remove One
  public removeConfiguration(entity: SavedConfiguration) {
    this.getAllConfigurations().subscribe(t => {
      if (t) {
        t = t.filter(e => e.timestamp != entity.timestamp);
        this.saveToStorage(SavedConfigurationTableKey, JSON.stringify(t));
      }
    });
  }

  // *********************** Saved Schedule ***********************

  // Save One
  public saveSchedule(entity: SavedSchedule) {
    this.saveItemInTable(entity, SavedScheduleTableKey);
  }

  // Get Alll
  public getAllSchedules(): Observable<SavedSchedule[] | undefined>{
    var storedTable = this.getStoredTable<SavedSchedule[]>(SavedScheduleTableKey);
    return storedTable;
  }

  // Remove One
  public removeSchedule(entity: SavedSchedule) {
    this.getAllSchedules().subscribe(t => {
      if (t) {
        t = t.filter(e => e.timestamp != entity.timestamp);
        this.saveToStorage(SavedScheduleTableKey, JSON.stringify(t));
      }
    });
  }

  // *********************** User Data ***********************

  // Get One
  public getUserData(): UserData {
    var userData = this.getFromStorage(UserDataKey);

    if (userData)
      return JSON.parse(userData);

    return { lastSeenPatchNotes: undefined };
  }


  // Save One
  public saveUserData(entity: UserData) {
    this.saveToStorage(UserDataKey, JSON.stringify(entity));
  }

  // *********************** SimulationSnapshot ***********************

  // Save One
  public saveSimulationSnapshot(entity: SavedSimulationSnapshot) {

    this.saveItemInTable(entity, SimulationSnapshotTableKey);
  }

  // Get All
  public getAllSimulationSnapshot(): Observable<SavedSimulationSnapshot[] | undefined> {
    var storedTable = this.getStoredTable<SavedSimulationSnapshot[]>(SimulationSnapshotTableKey);
    return storedTable;
  }

  // Remove One
  public removeSimulationSnapshot(entity: SavedSimulationSnapshot) {
    this.getAllSimulationSnapshot().subscribe(t => {

      if (t) {
        t = t.filter(e => e.timestamp != entity.timestamp);
        this.saveToStorage(SimulationSnapshotTableKey, JSON.stringify(t));
      }
    });
  }
  // Remove All
  public removeAllSimulationSnapshots() {
    this.removeAllRecordsFromTable(SimulationSnapshotTableKey);
  }

  // *********************** Private Implementation ***********************

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

  private removeAllRecordsFromTable(key: string) {
    localStorage.removeItem(key);
  }

  private objectToObserver<T>(storedFile: any): Observable<T> {
    return new Observable<T>(observer => {
      observer.next(storedFile);
      observer.complete();
    });
  }

  private saveItemInTable<E extends { timestamp: number }>(item: E, key: string) {
    var tableInStorage = this.getFromStorage(key);

    var entityTable: E[];
    if (!tableInStorage)
      entityTable = [];
    else
      entityTable = JSON.parse(tableInStorage);

    var index = entityTable.findIndex(v => v.timestamp == item.timestamp);

    if (index >= 0)
      entityTable[index] = item;
    else
      entityTable.push(item);

    var stringData = JSON.stringify(entityTable);
    this.saveToStorage(key, stringData);
  }
}

const SavedConfigurationTableKey = 'SavedConfigurationTable'
const SavedScheduleTableKey = 'SavedScheduleTable';
const SimulationSnapshotTableKey = 'SimulationSnapshotTable';
const UserDataKey = 'UserData';
