import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, ObservableInput, retry, throwError } from 'rxjs';
import { environment } from '../../environments/environment';
import { Character } from '../models/character';
import { Dungeon } from '../models/dungeon';
import { SimulateDayRequest, SimulateUntilLevelRequest } from '../models/requests';
import { SimulationConfigForm } from '../models/simulation-configuration';
import { SimulationOptions } from '../models/simulation-options';
import { DungeonResult, SimulationResult } from '../models/simulation-result';
import { SnackbarService } from './snackbar.service';


@Injectable({
  providedIn: 'root'
})
export class SimulatorService {

  constructor(private httpClient: HttpClient, private snackbarService: SnackbarService) { }

  getDungeonList(): Observable<Dungeon[]> {
    let url = environment.apiUrl + '/api/dungeons';

    let result = this.httpClient.get<Dungeon[]>(url).pipe(retry(3), catchError(e => this.handleError(e, this.snackbarService)));

    return result;
  }

  simulateDungeon(dungeonPosition: number, dungeonEnemyPosition: number, character: Character, iterations: number, winTreshold?: number): Observable<DungeonResult> {
    let url = environment.apiUrl + '/api/simulateDungeon';
    winTreshold = winTreshold ?? iterations;

    let body = {
      dungeonPosition, dungeonEnemyPosition, ...character, winTreshold, iterations: iterations
    };

    let result = this.httpClient.post<DungeonResult>(url, body).pipe(retry(3), catchError(e => this.handleError(e, this.snackbarService)));

    return result;
  }

  simulate(simulationOptions: SimulationOptions, simulationForm: SimulationConfigForm): Observable<SimulationResult> {
    if (simulationOptions.simulationType == 'Days')
      return this.simulateDays(simulationForm, simulationOptions.simulateUntil);

    return this.simulateLevels(simulationForm, simulationOptions.simulateUntil);
  }

  simulateDays(simulationOptions: SimulationConfigForm, days: number): Observable<SimulationResult> {

    let url = environment.apiUrl + '/api/simulateUntilDays';

    let body = {
      ...simulationOptions,
      daysCount: days,
    };

    let result = this.httpClient.post<SimulationResult>(url, body).pipe(retry(3), catchError(e => this.handleError(e, this.snackbarService)));

    return result;
  }
  simulateLevels(simulationOptions: SimulationConfigForm, level: number): Observable<SimulationResult> {

    let url = environment.apiUrl + '/api/simulateUntilLevel';

    let body = {
      ...simulationOptions,
      untilLevel: level,
    };

    let result = this.httpClient.post<SimulationResult>(url, body).pipe(retry(3), catchError(e => this.handleError(e, this.snackbarService)));

    return result;
  }

  private handleError(error: HttpErrorResponse, snackbarService: SnackbarService): ObservableInput<any> {
    snackbarService.createErrorSnackbar('Something bad happened with internal API. Please try again.')
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }
}
