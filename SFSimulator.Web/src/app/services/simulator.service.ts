import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, ObservableInput, retry, throwError } from 'rxjs';
import { environment } from '../../environments/environment';
import { Character, Companion } from '../models/character';
import { Dungeon } from '../models/dungeon';
import { SimulationConfigForm } from '../models/simulation-configuration';
import { SimulationOptions, SimulationType } from '../models/simulation-options';
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

  simulateDungeon(dungeonPosition: number, dungeonEnemyPosition: number, character: Character, companions: Companion[], iterations: number, winTreshold?: number): Observable<DungeonResult> {
    let url = environment.apiUrl + '/api/simulateDungeon';
    winTreshold = winTreshold ?? iterations;

    let body = {
      dungeonPosition, dungeonEnemyPosition, ...character, companions, winTreshold, iterations: iterations
    };

    let result = this.httpClient.post<DungeonResult>(url, body).pipe(retry(3), catchError(e => this.handleError(e, this.snackbarService)));

    return result;
  }

  simulate(simulationOptions: SimulationOptions, simulationForm: SimulationConfigForm): Observable<SimulationResult> {
    let body = {
      ...simulationForm,
      type: simulationOptions.simulationType,
      simulateUntil: simulationOptions.simulateUntil,
    };

    let subscription: Observable<SimulationResult>

    switch (simulationOptions.simulationType) {
      case SimulationType.UntilLevel:
        subscription = this.simulateLevels(body);
        break;
      case SimulationType.UntilBaseStats:
        subscription =this.simulateBaseStats(body);
        break;
      case SimulationType.UntilDays:
      default:
        subscription = this.simulateDays(body);
        break;
    }

    return subscription.pipe(retry(3), catchError(e => this.handleError(e, this.snackbarService)));
  }

  simulateDays(body: any): Observable<SimulationResult> {

    let url = environment.apiUrl + '/api/simulateUntilDays';
    let result = this.httpClient.post<SimulationResult>(url, body);

    return result;
  }
  simulateLevels(body: any): Observable<SimulationResult> {

    let url = environment.apiUrl + '/api/simulateUntilLevel';
    let result = this.httpClient.post<SimulationResult>(url, body);

    return result;
  }
  simulateBaseStats(body: any): Observable<SimulationResult> {

    let url = environment.apiUrl + '/api/simulateUntilBaseStats';
    let result = this.httpClient.post<SimulationResult>(url, body);

    return result;
  }
  private handleError(error: HttpErrorResponse, snackbarService: SnackbarService): ObservableInput<any> {
    snackbarService.createErrorSnackbar('Something bad happened with internal API. Please try again.')
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }
}
