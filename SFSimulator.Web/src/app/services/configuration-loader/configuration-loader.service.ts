import { Injectable } from '@angular/core';
import {  Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConfigurationLoaderService {

  constructor() { }

  public getStoredFile(key: string): Observable<string> {
    const storedFile = this.getFromStorage(key);
    if (storedFile) {
      return this.objectToObserver<string>(storedFile);
    }
    else
      return new Observable<string>();
  }

  public saveToStorage(key: string, data: string) {
    localStorage.setItem(key, data);
  }

  private getFromStorage(key: string) {
    return localStorage.getItem(key);
  }

  private objectToObserver<T>(storedFile: T): Observable<T> {
    return new Observable<T>(observer => {
      observer.next(storedFile);
      observer.complete();
    });
  }
}
